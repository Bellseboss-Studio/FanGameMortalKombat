using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class InputMovementCustomV2 : InputCustomV2
    {
        private readonly float _force;

        public InputMovementCustomV2(float force)
        {
            _force = force;
        }

        public Vector3 CalculateMovement(Vector2 input, float speed, GameObject camera, GameObject player, bool isJumping = false)
        {
            if (!isJumping)
            {
                // Canonical grounded path: XZ, yaw-only, single source shared with rotation.
                return BuildCanonicalDirection(input, camera, player) * speed;
            }

            // Legacy jumping path: keeps the vertical component of (player - camera) so
            // airborne motion retains its original 3D profile. The rotation basis stays XZ.
            var direction = player.transform.position - camera.transform.position;
            direction.Normalize();
            var right = new Vector3(direction.z, 0, -direction.x);
            var result = input.x * right + input.y * direction;
            result.Normalize();
            return result * speed;
        }

        /// <summary>
        /// Canonical camera-relative XZ world direction shared by movement and rotation
        /// (spec player-v2-movement-orientation-alignment). Uses the YAW-ONLY camera
        /// ROTATION basis (camera forward projected to XZ), so the basis is CONSTANT
        /// while the player translates laterally — strafing no longer rotates the
        /// direction (the old positional (player - camera) basis caused the tremble).
        /// Returns a zero vector (no exception, no drift) when the camera/player is
        /// null or the basis is degenerate (camera looking straight up/down).
        /// </summary>
        internal static Vector3 BuildCanonicalDirection(Vector2 quantizedInput, GameObject camera, GameObject player)
        {
            if (camera == null || player == null) return Vector3.zero;

            var direction = camera.transform.forward; // yaw-only: position never participates
            direction.y = 0f; // camera pitch and LookAt vertical offset must not participate
            if (direction.sqrMagnitude <= 0.000001f) return Vector3.zero; // degenerate basis (up/down)

            direction.Normalize();
            var right = new Vector3(direction.z, 0f, -direction.x);
            var result = quantizedInput.x * right + quantizedInput.y * direction;

            if (result.sqrMagnitude <= 0.0001f) return Vector3.zero; // dead-zone input -> no drift
            result.Normalize();
            return result;
        }
    }
}