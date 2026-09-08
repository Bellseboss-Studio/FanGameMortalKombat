using System.Reflection;
using Bellseboss.Pery.Scripts.Input;
using NUnit.Framework;
using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEngine;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// PR2 contract tests for the STABLE gameplay-yaw reference
    /// (spec player-v2-movement-orientation-alignment "stable yaw reference",
    /// design "stable gameplay yaw reference/pivot", tasks Phase 3.2-3.6).
    ///
    /// The rendered Cinemachine vcam transform is the DAMPED output
    /// (TrackerSettings.PositionDamping (1,1,1) + RotationComposer Damping): while
    /// the player strafes, the vcam position lags and the aim swings, so reading
    /// vcam.transform.forward as the gameplay basis feeds the lag back into
    /// movement/rotation (tremble). The stable reference instead derives the
    /// UNDAMPED steady-state yaw from the vcam's raw follow geometry
    /// (follow target + follow offset + look-at target) and exposes it through its
    /// own transform.forward — decoupled from the damped vcam output. Both
    /// movement and rotation consume it through the SAME shared helper
    /// (InputMovementCustomV2.BuildCanonicalDirection), helper byte-identical.
    ///
    /// Contract under test:
    ///   - stable forward == vcam steady-state yaw (WorldSpace follow and static
    ///     no-follow-body rigs — the two shipped cameraMain shapes)
    ///   - damping/lag on the vcam transform NEVER shifts the gameplay basis
    ///   - camera pitch and LookAt vertical offset are ignored
    ///   - null vcam / null follow / degenerate geometry -> degenerate forward,
    ///     and through the helper -> zero canonical direction (facing unchanged)
    ///   - the pivot transform exposes the stable forward and the shared helper
    ///     consumes it end to end
    ///   - Configure wiring feeds the stable reference (NOT the damped vcam
    ///     GameObject) into both MovementRigidbodyV2 and RotationCharacterV2
    /// </summary>
    public class PlayerV2StableYawContractTests
    {
        private const float Epsilon = 1e-4f;

        private static readonly Vector3 WorldSpaceOffset = new Vector3(0f, 2f, -5f);
        private static readonly Vector3 DampedIdlePose = new Vector3(0f, 3f, -5f);

        private static void AssertApprox(Vector3 actual, Vector3 expected, string message)
        {
            Assert.AreEqual(expected.x, actual.x, Epsilon, message);
            Assert.AreEqual(expected.y, actual.y, Epsilon, message);
            Assert.AreEqual(expected.z, actual.z, Epsilon, message);
        }

        /// <summary>
        /// Builds a new-style CinemachineCamera + CinemachineFollow rig whose
        /// TRANSFORM is placed at the given (damped) pose. The transform is the
        /// simulated damped output — the stable reference must IGNORE it.
        /// </summary>
        private static GameObject CreateFollowVcam(
            Transform follow, Transform lookAt,
            Vector3 followOffset, BindingMode binding,
            Vector3 dampedPosition, Quaternion dampedRotation)
        {
            var go = new GameObject("Vcam");
            go.transform.SetPositionAndRotation(dampedPosition, dampedRotation);

            var cam = go.AddComponent<CinemachineCamera>();
            cam.Follow = follow;
            if (lookAt != null)
            {
                cam.LookAt = lookAt;
            }

            var followComponent = go.AddComponent<CinemachineFollow>();
            followComponent.FollowOffset = followOffset;
            followComponent.TrackerSettings.BindingMode = binding;
            return go;
        }

        /// <summary>Static camera rig (03_Game cameraMain shape): no follow body.</summary>
        private static GameObject CreateStaticVcam(Transform follow, Vector3 posePosition, Quaternion poseRotation)
        {
            var go = new GameObject("Vcam");
            go.transform.SetPositionAndRotation(posePosition, poseRotation);
            var cam = go.AddComponent<CinemachineCamera>();
            cam.Follow = follow; // LookAt left unset -> falls back to Follow (03_Game semantics)
            return go;
        }

        private static Transform CreateTarget(string name, Vector3 position)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            return go.transform;
        }

        private static object ReadPrivateField(object instance, System.Type type, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, $"Field {fieldName} must exist on {type.Name}.");
            return field.GetValue(instance);
        }

        [Test]
        public void ComputeStableForward_WorldSpaceFollow_MatchesSteadyStateYaw()
        {
            // Camera steady state: follow.position + (0,2,-5) -> lookAt(follow)
            // forward XZ = (0,0,1). This is the yaw the camera WOULD have without
            // damping — constant under player translation (the tremble fix).
            var follow = CreateTarget("Player", new Vector3(0f, 1f, 0f));
            var vcam = CreateFollowVcam(follow, follow, WorldSpaceOffset, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);

            var forward = StableYawReference.ComputeStableForward(vcam.GetComponent<CinemachineVirtualCameraBase>());

            AssertApprox(forward, new Vector3(0f, 0f, 1f),
                "WorldSpace follow must expose the undamped steady-state yaw (constant offset direction).");
        }

        [Test]
        public void ComputeStableForward_Yaw45_WhenLookAtOffsetFromFollow()
        {
            // LookAt laterally offset from the follow target -> the steady-state
            // aim rotates 45°: forward = (lookAt - desiredCamPos) XZ.
            var follow = CreateTarget("Player", Vector3.zero);
            var lookAt = CreateTarget("LookTarget", new Vector3(5f, 0f, 0f));
            var vcam = CreateFollowVcam(follow, lookAt, WorldSpaceOffset, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);
            var expected = new Vector3(5f, 0f, 5f).normalized; // (5,0,0) - (0,2,-5) -> XZ (5,5)

            var forward = StableYawReference.ComputeStableForward(vcam.GetComponent<CinemachineVirtualCameraBase>());

            AssertApprox(forward, expected,
                "A laterally offset look-at target must rotate the stable yaw by the full geometry, not the damped pose.");
        }

        [Test]
        public void ComputeStableForward_IgnoresDampedVcamTransform_DecouplingProof()
        {
            // THE decoupling proof: same follow/lookAt/offset, but the vcam
            // TRANSFORM is parked at a wildly different "damped" pose (lagged
            // position + swung yaw). The stable forward must be IDENTICAL — the
            // gameplay basis never reads the damped output.
            var follow = CreateTarget("Player", new Vector3(0f, 1f, 0f));
            var undamped = CreateFollowVcam(follow, follow, WorldSpaceOffset, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);
            var dampedPose = new Vector3(2.5f, 3f, -7.5f);   // lagged position
            var dampedYaw = Quaternion.Euler(10f, 15f, 0f);  // swung aim
            var damped = CreateFollowVcam(follow, follow, WorldSpaceOffset, BindingMode.WorldSpace,
                dampedPose, dampedYaw);

            var cleanForward = StableYawReference.ComputeStableForward(undamped.GetComponent<CinemachineVirtualCameraBase>());
            var dampedForward = StableYawReference.ComputeStableForward(damped.GetComponent<CinemachineVirtualCameraBase>());

            AssertApprox(dampedForward, cleanForward,
                "PositionDamping/Composer lag on the vcam transform must NEVER shift the gameplay basis.");
            AssertApprox(dampedForward, new Vector3(0f, 0f, 1f),
                "The decoupled basis must still be the undamped steady-state yaw.");
        }

        [Test]
        public void ComputeStableForward_IgnoresPitchAndLookAtVerticalOffset()
        {
            // Camera pitch and LookAt vertical offset must not participate: only
            // the XZ geometry defines the stable yaw.
            var follow = CreateTarget("Player", Vector3.zero);
            var lookAtHigh = CreateTarget("LookTargetHigh", new Vector3(5f, 3f, 0f));
            var lookAtLevel = CreateTarget("LookTargetLevel", new Vector3(5f, 0f, 0f));
            var pitchedPose = Quaternion.Euler(30f, 0f, 0f);
            var high = CreateFollowVcam(follow, lookAtHigh, WorldSpaceOffset, BindingMode.WorldSpace,
                DampedIdlePose, pitchedPose);
            var level = CreateFollowVcam(follow, lookAtLevel, WorldSpaceOffset, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);
            var expected = new Vector3(5f, 0f, 5f).normalized;

            var highForward = StableYawReference.ComputeStableForward(high.GetComponent<CinemachineVirtualCameraBase>());
            var levelForward = StableYawReference.ComputeStableForward(level.GetComponent<CinemachineVirtualCameraBase>());

            AssertApprox(highForward, expected,
                "LookAt vertical offset (Y=3) must not tilt the XZ stable yaw.");
            AssertApprox(highForward, levelForward,
                "A pitched vcam pose must not alter the XZ stable yaw.");
        }

        [Test]
        public void ComputeStableForward_StaticCameraNoFollowBody_UsesVcamPose()
        {
            // 03_Game cameraMain shape: static CinemachineCamera (no follow body).
            // The vcam position is never damped (nothing writes it), so the
            // undamped aim is (lookAt - vcam.position) XZ.
            var follow = CreateTarget("Player", Vector3.zero);
            var vcam = CreateStaticVcam(follow, new Vector3(10f, 5f, 0f), Quaternion.identity);

            var forward = StableYawReference.ComputeStableForward(vcam.GetComponent<CinemachineVirtualCameraBase>());

            AssertApprox(forward, new Vector3(-1f, 0f, 0f),
                "A static (no-follow-body) camera must aim from its fixed pose toward the look-at target.");
        }

        [Test]
        public void ComputeStableForward_NullVcam_ReturnsDegenerate()
        {
            var forward = StableYawReference.ComputeStableForward(null);

            Assert.AreEqual(StableYawReference.DegenerateForward, forward,
                "A null vcam must produce the degenerate forward so the helper yields zero.");
        }

        [Test]
        public void ComputeStableForward_NullFollow_ReturnsDegenerate()
        {
            var vcam = CreateFollowVcam(null, null, WorldSpaceOffset, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);

            var forward = StableYawReference.ComputeStableForward(vcam.GetComponent<CinemachineVirtualCameraBase>());

            Assert.AreEqual(StableYawReference.DegenerateForward, forward,
                "A vcam without a follow target must produce the degenerate forward (no direction, no drift).");
        }

        [Test]
        public void ComputeStableForward_ZeroOffsetOnFollow_ReturnsDegenerate()
        {
            // follow == lookAt with a zero offset -> desired camera position sits
            // on the look target -> forward XZ is zero -> degenerate.
            var follow = CreateTarget("Player", new Vector3(0f, 1f, 0f));
            var vcam = CreateFollowVcam(follow, follow, Vector3.zero, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);

            var forward = StableYawReference.ComputeStableForward(vcam.GetComponent<CinemachineVirtualCameraBase>());

            Assert.AreEqual(StableYawReference.DegenerateForward, forward,
                "Degenerate (zero-length) aim geometry must yield the degenerate forward.");
        }

        [Test]
        public void Pivot_Refresh_AppliesStableYaw_AndSharedHelperConsumesIt()
        {
            // End to end: the pivot transform exposes the stable yaw, and the
            // shared helper consumed by BOTH movement and rotation returns exactly
            // that direction for forward input. A degenerate pivot makes the
            // helper return zero (camera-null/degenerate contract preserved).
            var follow = CreateTarget("Player", new Vector3(0f, 1f, 0f));
            var vcam = CreateFollowVcam(follow, follow, WorldSpaceOffset, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);
            var pivot = StableYawReference.Create(vcam.GetComponent<CinemachineVirtualCameraBase>(), null);
            var player = CreateTarget("PlayerProxy", Vector3.zero).gameObject;

            AssertApprox(pivot.transform.forward, new Vector3(0f, 0f, 1f),
                "The pivot transform must expose the stable yaw as its forward.");
            var direction = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), pivot.gameObject, player);
            AssertApprox(direction, new Vector3(0f, 0f, 1f),
                "Forward input through the shared helper must equal the stable reference forward.");

            // Degenerate pivot: transform looks straight up -> helper zero.
            var degenerateVcam = CreateFollowVcam(follow, follow, Vector3.zero, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);
            var degeneratePivot = StableYawReference.Create(
                degenerateVcam.GetComponent<CinemachineVirtualCameraBase>(), null);
            AssertApprox(degeneratePivot.transform.forward, new Vector3(0f, 1f, 0f),
                "A degenerate stable reference must look straight up (forward XZ == 0).");
            var degenerateDirection = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), degeneratePivot.gameObject, player);
            Assert.AreEqual(Vector3.zero, degenerateDirection,
                "A degenerate stable reference must yield a zero canonical direction (facing unchanged).");
        }

        [Test]
        public void Configure_WiresStableRef_IntoMovementAndRotation_NotDampedVcam()
        {
            // Source-wiring contract: both gameplay systems must be configured
            // with the STABLE reference GameObject (never the damped vcam), so
            // movement, rotation and the shared helper all read the same basis.
            var follow = CreateTarget("Player", new Vector3(0f, 1f, 0f));
            var vcam = CreateFollowVcam(follow, follow, WorldSpaceOffset, BindingMode.WorldSpace,
                DampedIdlePose, Quaternion.identity);
            var pivot = StableYawReference.Create(vcam.GetComponent<CinemachineVirtualCameraBase>(), null);

            var playerGo = new GameObject("Player");
            var rigidbody = playerGo.AddComponent<Rigidbody>();
            var movement = playerGo.AddComponent<MovementRigidbodyV2>();
            var rotation = playerGo.AddComponent<RotationCharacterV2>();
            var stats = ScriptableObject.CreateInstance<StatisticsOfCharacter>();

            movement.Configure(rigidbody, 1f, 2f, pivot.gameObject, new MovementRigidBodyV2Stub(), stats);
            rotation.Configure(pivot.gameObject, playerGo, null, 0f);

            var movementCamera = (GameObject)ReadPrivateField(movement, typeof(MovementRigidbodyV2), "_camera");
            var rotationCamera = (GameObject)ReadPrivateField(rotation, typeof(RotationCharacterV2), "_camera");

            Assert.AreEqual(pivot.gameObject, movementCamera,
                "MovementRigidbodyV2 must be wired to the stable reference.");
            Assert.AreEqual(pivot.gameObject, rotationCamera,
                "RotationCharacterV2 must be wired to the stable reference.");
            Assert.AreNotEqual(vcam, movementCamera,
                "MovementRigidbodyV2 must NOT read the damped vcam transform.");
            Assert.AreNotEqual(vcam, rotationCamera,
                "RotationCharacterV2 must NOT read the damped vcam transform.");

            var q = new Vector2(0f, 1f);
            var movementDirection = InputMovementCustomV2.BuildCanonicalDirection(q, movementCamera, playerGo);
            var rotationDirection = InputMovementCustomV2.BuildCanonicalDirection(q, rotationCamera, playerGo);
            AssertApprox(movementDirection, rotationDirection,
                "Movement and rotation must derive the SAME canonical direction from the stable reference.");
            AssertApprox(movementDirection, new Vector3(0f, 0f, 1f),
                "The shared stable basis must equal the undamped steady-state yaw.");
        }

        /// <summary>No-op IMovementRigidBodyV2 stub for the wiring test.</summary>
        private sealed class MovementRigidBodyV2Stub : IMovementRigidBodyV2
        {
            public void UpdateAnimation() { }
            public void UpdateAnimation(bool isTouchingFloor, bool isTouchingWall) { }
            public void ChangeToNormalJump() { }
            public void ChangeRotation(Vector3 rotation) { }
            public void RestoreRotation() { }
            public void EndAttackMovement() { }
            public void PlayerFall() { }
            public void PlayerRecovery() { }
            public bool IsAttacking() => false;
            public void PlayerFallV2() { }
            public void PlayerRecoveryV2() { }
            public bool IsJumpingInWall() => false;
            public void OnStartRunning() { }
            public void OnStopRunning() { }
        }
    }
}