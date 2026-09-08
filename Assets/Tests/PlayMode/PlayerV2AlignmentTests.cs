using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// PlayMode alignment tests (spec player-v2-movement-orientation-alignment,
    /// design "where alignment tests go"). After the rotation settles, the angle
    /// between velocity direction (XZ) and transform.forward MUST be &lt;= 5° for
    /// every cardinal/diagonal input under any camera yaw, pitch, or LookAt
    /// offset. The canonical basis is yaw-only camera ROTATION (camera forward
    /// projected to XZ), shared by movement and rotation — the player translates
    /// freely (never pinned) and the basis stays constant, so these tests prove
    /// the position-independent contract end to end.
    ///
    /// Settle gate (spec "timing stabilization"): facing unchanged within 1° for
    /// 3 consecutive fixed frames; assertions only run after that.
    /// </summary>
    public class PlayerV2AlignmentTests
    {
        private const string TestScenePath = "Assets/_Scenes/Tests/PlayerV2Test.unity";
        private const float MaxAngleDegrees = 5f;
        private const int MaxSettleFrames = 300;

        // Camera placed at (10,5,0) with identity rotation: camera.forward-XZ =
        // (0,0,1). The canonical basis is now yaw-only camera ROTATION (camera
        // forward projected to XZ), so the expected directions derive from camera
        // yaw and are position-independent — the player translates freely during
        // the tests (never pinned) and the basis stays constant.
        private static readonly Vector3 CameraPosition = new Vector3(10f, 5f, 0f);
        private static readonly Vector3 IdentityRotation = Vector3.zero;

        private GameObject _player;
        private GameObject _testCamera;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            ServiceLocator.Instance.Reset();
            yield return LoadSceneAndYieldFrames(TestScenePath);
            _player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(_player, "The test scene must contain PlayerV2Update.");

            // Cinemachine re-drives a vcam's transform every LateUpdate, so pinning
            // the scene's CM Camera pose is unreliable in play mode. Instead the
            // tests hand the movement/rotation systems a plain GameObject whose
            // transform is fully deterministic. The components only read
            // camera.transform.forward (XZ) for the yaw-only canonical basis.
            _testCamera = new GameObject("TestCamera");
            PlaceCamera(CameraPosition, IdentityRotation);

            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            Assert.IsNotNull(movement, "PlayerV2Update must have MovementRigidbodyV2.");
            Assert.IsNotNull(rotation, "PlayerV2Update must have RotationCharacterV2.");
            SetPrivateField(movement, typeof(MovementRigidbodyV2), "_camera", _testCamera);
            SetPrivateField(rotation, typeof(RotationCharacterV2), "_camera", _testCamera);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            ServiceLocator.Instance.Reset();
            yield return null;
        }

        private static IEnumerator LoadSceneAndYieldFrames(string scenePath)
        {
            EditorSceneManager.LoadSceneInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
            yield return null;
            yield return null;
        }

        /// <summary>
        /// Pins the deterministic test camera pose used as the canonical basis.
        /// </summary>
        private void PlaceCamera(Vector3 position, Vector3 eulerAngles)
        {
            Assert.IsNotNull(_testCamera, "The test camera must exist.");
            _testCamera.transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles));
        }

        private static void SetPrivateField(object instance, System.Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, $"Field {fieldName} must exist on {type.Name}.");
            field.SetValue(instance, value);
        }

        /// <summary>
        /// Spec settle gate: facing unchanged within 1° for 3 consecutive fixed
        /// frames, capped at MaxSettleFrames.
        ///
        /// Test harness (spec "timing stabilization", design "baseRotationSpeed
        /// 900°/s -> fast"): ONE test-only manipulation makes the gate measure
        /// GENUINE convergence instead of the production rotation-velocity decay:
        ///
        /// 1. Re-zero rotation._lastDirection every frame. Update() sees
        ///    _lastDirection == zero, re-initiates the direction change
        ///    (shouldUpdateDirection) and restores _rotationVelocity to 1, so
        ///    the turn runs at full baseRotationSpeed instead of decaying to the
        ///    min-velocity crawl (~25°/s, which a 180° turn would never finish
        ///    inside the frame cap and could false-settle at high frame rates).
        ///
        /// The player XZ is deliberately NOT pinned: the canonical basis is now
        /// position-independent (yaw-only camera rotation), so the player
        /// translates freely during settle — pinning would mask a regression to
        /// the old positional basis.
        ///
        /// A slow rotation creep (e.g. 0.4° per frame while the rotation curve
        /// ramps) must NOT count as settled — the gate uses the TOTAL angular
        /// change across the last 3 fixed-frame intervals, so the facing must
        /// have genuinely stopped near its target. Returns by yield-break when
        /// settled; fails the test if the cap is hit.
        /// </summary>
        private static IEnumerator WaitForSettle(Transform player, RotationCharacterV2 rotation)
        {
            var lastDirField = typeof(RotationCharacterV2).GetField(
                "_lastDirection", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(lastDirField, "RotationCharacterV2._lastDirection must exist.");

            var recent = new System.Collections.Generic.Queue<Quaternion>();
            for (var i = 0; i < MaxSettleFrames; i++)
            {
                lastDirField.SetValue(rotation, Vector3.zero); // force full rotation speed

                yield return new WaitForFixedUpdate();
                recent.Enqueue(player.rotation);
                if (recent.Count > 4) recent.Dequeue();

                if (recent.Count == 4)
                {
                    var samples = recent.ToArray();
                    var total = 0f;
                    for (var j = 0; j < samples.Length - 1; j++)
                    {
                        total += Quaternion.Angle(samples[j], samples[j + 1]);
                    }

                    if (total <= 1f) yield break;
                }
            }

            Assert.Fail($"Rotation did not settle within {MaxSettleFrames} fixed frames.");
        }

        /// <summary>
        /// Drives both systems with the same raw input (bypassing real input
        /// devices), waits for the settle gate, then asserts the alignment
        /// contract: velocity XZ and facing both converge to the canonical
        /// direction within MaxAngleDegrees.
        /// </summary>
        private IEnumerator AssertAlignment(Vector2 input, Vector3 expectedWorldDir)
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            Assert.IsNotNull(movement, "PlayerV2Update must have MovementRigidbodyV2.");
            Assert.IsNotNull(rotation, "PlayerV2Update must have RotationCharacterV2.");

            movement.Direction(input);
            rotation.Direction(input);

            // Wait for the FIRST movement sync (FixedUpdate) so the canonical sync
            // direction is accepted, then reset the rotation's _lastDirection to
            // zero BEFORE this frame's Update runs. This mirrors production: input
            // arrives in Update before any movement sync, so the rotation system
            // sees _lastDirection == zero and spins up its rotation velocity to
            // full speed instead of decaying into a slow crawl.
            yield return new WaitForFixedUpdate();
            SetPrivateField(rotation, typeof(RotationCharacterV2), "_lastDirection", Vector3.zero);

            yield return WaitForSettle(_player.transform, rotation);

            var rb = _player.GetComponent<Rigidbody>();
            var velocityXZ = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            var facing = _player.transform.forward;

            Assert.Greater(velocityXZ.magnitude, 0.1f,
                "Movement must actually produce velocity (not a ghost assertion).");
            Assert.LessOrEqual(Vector3.Angle(velocityXZ, facing), MaxAngleDegrees,
                $"Velocity direction must align with facing within {MaxAngleDegrees}° " +
                $"(input {input}, velocity {velocityXZ}, facing {facing}).");
            Assert.LessOrEqual(Vector3.Angle(facing, expectedWorldDir), MaxAngleDegrees,
                $"Facing must converge to the canonical world direction within {MaxAngleDegrees}° " +
                $"(input {input}, facing {facing}, expected {expectedWorldDir}).");
        }

        [UnityTest]
        public IEnumerator ForwardInput_AlignsVelocityAndFacing()
        {
            // Canonical: camera forward-XZ (identity yaw) = (0,0,1).
            yield return AssertAlignment(new Vector2(0f, 1f), new Vector3(0f, 0f, 1f));
        }

        [UnityTest]
        public IEnumerator BackwardInput_AlignsVelocityAndFacing()
        {
            yield return AssertAlignment(new Vector2(0f, -1f), new Vector3(0f, 0f, -1f));
        }

        [UnityTest]
        public IEnumerator StrafeRight_AlignsVelocityAndFacing()
        {
            // right = (z, 0, -x) of the canonical basis (0,0,1) = (1,0,0).
            yield return AssertAlignment(new Vector2(1f, 0f), new Vector3(1f, 0f, 0f));
        }

        [UnityTest]
        public IEnumerator StrafeLeft_AlignsVelocityAndFacing()
        {
            yield return AssertAlignment(new Vector2(-1f, 0f), new Vector3(-1f, 0f, 0f));
        }

        [UnityTest]
        public IEnumerator DiagonalInput_AlignsVelocityAndFacing()
        {
            // forward (0,0,1) + right (1,0,0) -> normalized diagonal, no cardinal snap.
            yield return AssertAlignment(new Vector2(1f, 1f), new Vector3(1f, 0f, 1f).normalized);
        }

        [UnityTest]
        public IEnumerator TiltedCamera30_KeepsAlignment()
        {
            // Camera pitch 30° must NOT alter the yaw-only canonical direction.
            PlaceCamera(CameraPosition, new Vector3(30f, 0f, 0f));
            yield return AssertAlignment(new Vector2(0f, 1f), new Vector3(0f, 0f, 1f));
        }

        [UnityTest]
        public IEnumerator LookAtOffsetCamera_KeepsAlignment()
        {
            // Camera raised above the player (LookAt vertical offset): same yaw basis.
            PlaceCamera(new Vector3(10f, 8f, 0f), IdentityRotation);
            yield return AssertAlignment(new Vector2(0f, 1f), new Vector3(0f, 0f, 1f));
        }

        [UnityTest]
        public IEnumerator DeadZoneInput_NoVelocity_NoFacingChange()
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            var initialFacing = _player.transform.rotation;

            movement.Direction(new Vector2(0.05f, 0f));
            rotation.Direction(new Vector2(0.05f, 0f));

            for (var i = 0; i < 10; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            var rb = _player.GetComponent<Rigidbody>();
            var velocityXZ = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            Assert.Less(velocityXZ.magnitude, 0.01f,
                "Dead-zone input must produce zero velocity (no drift).");
            Assert.Less(Quaternion.Angle(initialFacing, _player.transform.rotation), 1f,
                "Dead-zone input must leave facing unchanged.");
        }

        [UnityTest]
        public IEnumerator NullCamera_NoThrow_NoFacingChange()
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            var initialFacing = _player.transform.rotation;

            // Null the internal camera references (post-Configure) — the shared
            // helper must degrade to zero instead of throwing or rotating to a
            // spurious world direction (spec "camera null safety").
            SetPrivateField(movement, typeof(MovementRigidbodyV2), "_camera", null);
            SetPrivateField(rotation, typeof(RotationCharacterV2), "_camera", null);

            movement.Direction(new Vector2(0f, 1f));
            rotation.Direction(new Vector2(0f, 1f));

            for (var i = 0; i < 10; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            var rb = _player.GetComponent<Rigidbody>();
            var velocityXZ = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            Assert.Less(velocityXZ.magnitude, 0.01f,
                "A null camera must produce zero velocity, not a spurious move.");
            Assert.Less(Quaternion.Angle(initialFacing, _player.transform.rotation), 1f,
                "A null camera must leave facing unchanged.");
        }

        [UnityTest]
        public IEnumerator DiagonalInput_NoCardinalSnapping()
        {
            // Spec: diagonals must NOT snap to a cardinal axis.
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            movement.Direction(new Vector2(1f, 1f));
            rotation.Direction(new Vector2(1f, 1f));

            yield return new WaitForFixedUpdate();
            SetPrivateField(rotation, typeof(RotationCharacterV2), "_lastDirection", Vector3.zero);

            yield return WaitForSettle(_player.transform, rotation);

            var facing = _player.transform.forward;
            var expected = new Vector3(1f, 0f, 1f).normalized;
            var cardinalAngles = new[]
            {
                Vector3.Angle(facing, Vector3.right),
                Vector3.Angle(facing, -Vector3.right),
                Vector3.Angle(facing, Vector3.forward),
                Vector3.Angle(facing, -Vector3.forward)
            };
            foreach (var angle in cardinalAngles)
            {
                Assert.Greater(angle, 10f,
                    "Diagonal facing must not snap to a cardinal axis.");
            }

            Assert.LessOrEqual(Vector3.Angle(facing, expected), MaxAngleDegrees,
                "Diagonal facing must still converge to the canonical diagonal.");
        }
    }
}