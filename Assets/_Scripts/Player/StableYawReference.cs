using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEngine;

namespace _Scripts.Player
{
    /// <summary>
    /// Runtime-composed STABLE gameplay-yaw reference (PR2 of
    /// stabilize-player-movement-rotation-basis, tasks Phase 3.2-3.3).
    ///
    /// The rendered Cinemachine vcam transform is the DAMPED output
    /// (TrackerSettings.PositionDamping (1,1,1) + RotationComposer Damping
    /// (0.5,0.5)): while the player strafes, the vcam position lags the player and
    /// the composer aim swings, so reading vcam.transform.forward as the gameplay
    /// basis feeds that lag back into movement/rotation — the tremble. This
    /// component instead derives the UNDAMPED steady-state yaw from the vcam's raw
    /// follow geometry (follow target + follow offset + look-at target) and exposes
    /// it through its own transform.forward, so gameplay direction is decoupled
    /// from the damped output while PositionDamping stays untouched.
    ///
    /// Both MovementRigidbodyV2 and RotationCharacterV2 consume it through the
    /// SAME shared helper (InputMovementCustomV2.BuildCanonicalDirection, which
    /// only reads camera.transform.forward) — the helper stays byte-identical.
    ///
    /// Transform contract (what the helper sees):
    ///   - stable forward: yaw-only XZ of (lookAt - desiredCameraPos), normalized.
    ///     desiredCameraPos = follow.position + referenceRotation * FollowOffset
    ///     when the vcam has a CinemachineFollow body (reference rotation is
    ///     identity for WorldSpace binding, yaw-only follow-target reference for
    ///     the LockToTarget*/LazyFollow family); when there is NO follow body the
    ///     vcam pose is static, so the vcam transform position IS the steady state.
    ///   - degenerate/null source: transform looks straight UP (forward XZ == 0)
    ///     so the helper's degenerate-camera branch returns zero (facing unchanged,
    ///     no drift) — the camera-null safety contract is preserved.
    ///   - transform.position mirrors the vcam transform so the LEGACY airborne
    ///     jump path of InputMovementCustomV2.CalculateMovement keeps its original
    ///     3D profile (airborne is out of scope; the grounded canonical path never
    ///     reads position).
    ///
    /// Runtime wiring only: created by CharacterV2.ConfigCamera, no serialized
    /// scene/prefab reference, zero asset migration.
    /// </summary>
    public class StableYawReference : MonoBehaviour
    {
        private const float DegenerateSqrMagnitude = 0.000001f;

        private CinemachineVirtualCameraBase _vcam;

        /// <summary>
        /// Degenerate sentinel returned by <see cref="ComputeStableForward"/> when
        /// no stable yaw exists (null vcam, null follow, zero-length aim geometry).
        /// <see cref="Refresh"/> maps it to a straight-up pose so the shared helper
        /// produces a zero canonical direction.
        /// </summary>
        internal static readonly Vector3 DegenerateForward = Vector3.up;

        /// <summary>
        /// Creates a stable-yaw reference targeting <paramref name="vcam"/>,
        /// optionally parented (so it is destroyed with its owner). The initial
        /// pose is applied immediately.
        /// </summary>
        public static StableYawReference Create(CinemachineVirtualCameraBase vcam, Transform parent)
        {
            var gameObject = new GameObject("StableYawReference");
            if (parent != null)
            {
                gameObject.transform.SetParent(parent, false);
            }
            var reference = gameObject.AddComponent<StableYawReference>();
            reference.Configure(vcam);
            reference.Refresh();
            return reference;
        }

        /// <summary>
        /// (Re)targets the stable reference at a vcam. Runtime wiring only — called
        /// by <see cref="CharacterV2.ConfigCamera"/> on camera swaps.
        /// </summary>
        public void Configure(CinemachineVirtualCameraBase vcam)
        {
            _vcam = vcam;
        }

        /// <summary>
        /// Applies the stable yaw to this transform and mirrors the vcam position
        /// (legacy airborne profile). Called from LateUpdate; tests call it
        /// directly. Safe to call every frame.
        /// </summary>
        public void Refresh()
        {
            var forward = ComputeStableForward(_vcam);
            if (forward == DegenerateForward)
            {
                // Straight up (forward (0,1,0)) -> forward XZ == 0 -> the shared
                // helper returns zero (facing unchanged). Euler(-90) about X maps
                // (0,0,1) to (0,1,0); Euler(+90) would look DOWN.
                transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
            }

            if (_vcam != null)
            {
                // The grounded canonical path never reads position; only the legacy
                // airborne jump profile does, and it must see the original vcam pose.
                transform.position = _vcam.transform.position;
            }
        }

        private void LateUpdate()
        {
            Refresh();
        }

        /// <summary>
        /// UNDAMPED steady-state yaw of the camera as a world XZ direction
        /// (position-independent for a follow rig; smooth geometry for a static
        /// rig). Pure function — deterministic and EditMode-testable. Returns
        /// <see cref="DegenerateForward"/> when no stable yaw exists.
        /// </summary>
        internal static Vector3 ComputeStableForward(CinemachineVirtualCameraBase vcam)
        {
            if (vcam == null) return DegenerateForward;
            var follow = vcam.Follow;
            if (follow == null) return DegenerateForward;

            // CinemachineCamera falls back to the follow target when LookAt is
            // unset (03_Game cameraMain); be explicit anyway.
            var lookAt = vcam.LookAt != null ? vcam.LookAt : follow;

            var desiredCameraPos = ResolveDesiredCameraPosition(vcam, follow);
            var forward = lookAt.position - desiredCameraPos;
            forward.y = 0f; // yaw-only: pitch and LookAt vertical offset never participate
            if (forward.sqrMagnitude <= DegenerateSqrMagnitude) return DegenerateForward;
            return forward.normalized;
        }

        private static Vector3 ResolveDesiredCameraPosition(CinemachineVirtualCameraBase vcam, Transform follow)
        {
            var followComponent = vcam.GetComponent<CinemachineFollow>();
            if (followComponent == null)
            {
                // No follow body: static camera pose (03_Game cameraMain). Nothing
                // damps the position (no body writes it), so the vcam transform
                // position IS the steady state.
                return vcam.transform.position;
            }

            return follow.position + ResolveReferenceOrientation(followComponent, follow) * followComponent.FollowOffset;
        }

        private static Quaternion ResolveReferenceOrientation(CinemachineFollow followComponent, Transform follow)
        {
            if (followComponent.TrackerSettings.BindingMode == BindingMode.WorldSpace)
            {
                // World-space offset: never rotated (PlayerV2Test cameraMain).
                return Quaternion.identity;
            }

            // LockToTarget*/LazyFollow family: yaw-only reference from the follow
            // target (exact for LockToTargetWithWorldUp; documented approximation
            // for the others — no shipped cameraMain uses them).
            var fwd = follow.forward;
            fwd.y = 0f;
            if (fwd.sqrMagnitude <= DegenerateSqrMagnitude) return Quaternion.identity;
            return Quaternion.LookRotation(fwd.normalized, Vector3.up);
        }
    }
}