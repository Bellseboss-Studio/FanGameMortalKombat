using NUnit.Framework;
using UnityEngine;
using Bellseboss.Pery.Scripts.Input;

namespace MortalKombat.Tests
{
    /// <summary>
    /// EditMode unit tests for the canonical camera-relative direction helper
    /// <see cref="InputMovementCustomV2.BuildCanonicalDirection"/> (spec
    /// player-v2-movement-orientation-alignment, design "canonical basis").
    ///
    /// The helper derives the world XZ movement/facing direction from the
    /// YAW-ONLY camera-rotation basis (camera forward projected to XZ), so the
    /// basis is CONSTANT while the player translates laterally (position never
    /// participates). It must be invariant to camera pitch and LookAt vertical
    /// offset, return zero for dead-zone/null/degenerate inputs (no exception,
    /// no drift), and normalize diagonals.
    ///
    /// The baseline camera is rotated yaw 180° so its forward-XZ is (0,0,-1),
    /// preserving the pre-change expected values (the old positional basis
    /// coincidentally produced (0,0,-1) for a camera at +Z relative to the
    /// player; the basis now derives from camera ROTATION, not position).
    /// </summary>
    public class PlayerV2DirectionMathTests
    {
        private const float Epsilon = 1e-4f;

        // Baseline camera at (0,5,10) rotated yaw 180° -> forward (0,0,-1).
        // The old positional (player - camera) XZ basis produced the same
        // (0,0,-1) for a player at the origin, so these expected values are
        // deliberately unchanged.
        private static readonly Vector3 BaselinePosition = new Vector3(0f, 5f, 10f);
        private static readonly Vector3 BaselineEuler = new Vector3(0f, 180f, 0f);

        private GameObject CreateCamera(Vector3 eulerAngles, Vector3 position)
        {
            var cam = new GameObject("Cam");
            cam.transform.position = position;
            cam.transform.rotation = Quaternion.Euler(eulerAngles);
            return cam;
        }

        private static GameObject CreatePlayer(Vector3 position)
        {
            var player = new GameObject("Player");
            player.transform.position = position;
            return player;
        }

        private static void AssertApprox(Vector3 actual, Vector3 expected, string message)
        {
            Assert.AreEqual(expected.x, actual.x, Epsilon, message);
            Assert.AreEqual(expected.y, actual.y, Epsilon, message);
            Assert.AreEqual(expected.z, actual.z, Epsilon, message);
        }

        [Test]
        public void BuildCanonicalDirection_ForwardInput_FollowsCameraForwardXZ()
        {
            // Baseline camera forward (yaw 180°) is (0,0,-1); forward input must
            // aim along that yaw-only camera-forward direction.
            var cam = CreateCamera(BaselineEuler, BaselinePosition);
            var player = CreatePlayer(Vector3.zero);

            var result = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), cam, player);

            AssertApprox(result, new Vector3(0f, 0f, -1f),
                "Forward input must aim along the yaw-only camera-forward XZ direction.");
        }

        [Test]
        public void BuildCanonicalDirection_BackwardInput_PointsOppositeCameraForward()
        {
            var cam = CreateCamera(BaselineEuler, BaselinePosition);
            var player = CreatePlayer(Vector3.zero);

            var result = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, -1f), cam, player);

            AssertApprox(result, new Vector3(0f, 0f, 1f),
                "Backward input must point opposite the camera-forward XZ direction.");
        }

        [Test]
        public void BuildCanonicalDirection_RightInput_PointsCameraRight()
        {
            var cam = CreateCamera(BaselineEuler, BaselinePosition);
            var player = CreatePlayer(Vector3.zero);
            // Basis direction (0,0,-1) -> right = (z, 0, -x) = (-1, 0, 0).
            var expectedRight = new Vector3(-1f, 0f, 0f);

            var result = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(1f, 0f), cam, player);

            AssertApprox(result, expectedRight,
                "Right input must map to the camera-relative right vector.");
        }

        [Test]
        public void BuildCanonicalDirection_LeftInput_PointsCameraLeft()
        {
            var cam = CreateCamera(BaselineEuler, BaselinePosition);
            var player = CreatePlayer(Vector3.zero);

            var result = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(-1f, 0f), cam, player);

            AssertApprox(result, new Vector3(1f, 0f, 0f),
                "Left input must point opposite the camera-relative right.");
        }

        [Test]
        public void BuildCanonicalDirection_DiagonalInput_IsNormalized()
        {
            var cam = CreateCamera(BaselineEuler, BaselinePosition);
            var player = CreatePlayer(Vector3.zero);
            // forward (0,0,-1) + right (-1,0,0) = (-1,0,-1), normalized.
            var expected = new Vector3(-1f, 0f, -1f).normalized;

            var result = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(1f, 1f), cam, player);

            AssertApprox(result, expected,
                "Diagonal input must produce the normalized diagonal world direction.");
            Assert.AreEqual(1f, result.magnitude, Epsilon,
                "The canonical direction must always be normalized.");
        }

        [Test]
        public void BuildCanonicalDirection_CameraPitch30_IgnoresPitch()
        {
            // Camera pitched 30 degrees (same yaw) must yield the SAME XZ direction.
            var level = CreateCamera(BaselineEuler, BaselinePosition);
            var tilted = CreateCamera(new Vector3(30f, 180f, 0f), BaselinePosition);
            var player = CreatePlayer(Vector3.zero);

            var levelResult = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), level, player);
            var tiltedResult = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), tilted, player);

            AssertApprox(tiltedResult, levelResult,
                "Camera pitch must not alter the yaw-only canonical direction.");
        }

        [Test]
        public void BuildCanonicalDirection_TranslationInvariance_SameCameraDifferentPlayerXZ()
        {
            // CORE FIX PROOF: the basis is yaw-only camera ROTATION, so the player's
            // XZ translation must NEVER change the canonical world direction. The old
            // positional (player - camera) basis rotated as the player translated
            // (the root cause of the strafe tremble) and fails this test.
            var cam = CreateCamera(BaselineEuler, BaselinePosition);

            var atOrigin = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), cam, CreatePlayer(Vector3.zero));
            var atOffset = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), cam, CreatePlayer(new Vector3(5f, 0f, 3f)));

            AssertApprox(atOffset, atOrigin,
                "Player XZ translation must not rotate the canonical direction.");
            AssertApprox(atOffset, new Vector3(0f, 0f, -1f),
                "The translated result must still be the yaw-only camera-forward direction.");
        }

        [Test]
        public void BuildCanonicalDirection_CameraStraightUp_Degenerate_ReturnsZero()
        {
            // Camera looking straight up (pitch -90°): forward XZ is zero, so the
            // yaw-only basis is degenerate -> zero direction, no drift.
            var cam = CreateCamera(new Vector3(-90f, 0f, 0f), BaselinePosition);
            var player = CreatePlayer(Vector3.zero);

            var result = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), cam, player);

            Assert.AreEqual(Vector3.zero, result,
                "A degenerate (forward XZ ~= 0) camera basis must yield zero.");
        }

        [Test]
        public void BuildCanonicalDirection_Yaw45WithPitch30_MatchesYawOnlyDirection()
        {
            // Yaw 45° + pitch 30° must equal the pure yaw-45° XZ direction
            // (sin45, 0, cos45): pitch is ignored, only yaw participates.
            var yaw45Pitch30 = CreateCamera(new Vector3(30f, 45f, 0f), BaselinePosition);
            var yaw45Only = CreateCamera(new Vector3(0f, 45f, 0f), BaselinePosition);
            var player = CreatePlayer(Vector3.zero);
            var expected = new Vector3(Mathf.Sin(45f * Mathf.Deg2Rad), 0f, Mathf.Cos(45f * Mathf.Deg2Rad));

            var pitchedResult = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), yaw45Pitch30, player);
            var yawOnlyResult = InputMovementCustomV2.BuildCanonicalDirection(
                new Vector2(0f, 1f), yaw45Only, player);

            AssertApprox(pitchedResult, expected,
                "Yaw 45° forward input must follow the yaw-only XZ direction.");
            AssertApprox(pitchedResult, yawOnlyResult,
                "Pitch 30° must not alter the yaw-45° canonical direction.");
        }

        [Test]
        public void BuildCanonicalDirection_DeadZoneInput_ReturnsZero()
        {
            // A sub-dead-zone quantized input (0,0 after quantization) must yield zero
            // so facing never drifts. Here we pass a zero quantized vector directly.
            var cam = CreateCamera(BaselineEuler, BaselinePosition);
            var player = CreatePlayer(Vector3.zero);

            var result = InputMovementCustomV2.BuildCanonicalDirection(
                Vector2.zero, cam, player);

            Assert.AreEqual(Vector3.zero, result,
                "A zero quantized input must produce a zero canonical direction (no drift).");
        }

        [Test]
        public void BuildCanonicalDirection_NullCamera_ReturnsZero_NoThrow()
        {
            var player = CreatePlayer(Vector3.zero);

            Assert.DoesNotThrow(() =>
            {
                var result = InputMovementCustomV2.BuildCanonicalDirection(
                    new Vector2(0f, 1f), null, player);
                Assert.AreEqual(Vector3.zero, result,
                    "A null camera must degrade to a neutral (zero) direction without throwing.");
            });
        }

        [Test]
        public void BuildCanonicalDirection_NullPlayer_ReturnsZero_NoThrow()
        {
            var cam = CreateCamera(BaselineEuler, BaselinePosition);

            Assert.DoesNotThrow(() =>
            {
                var result = InputMovementCustomV2.BuildCanonicalDirection(
                    new Vector2(0f, 1f), cam, null);
                Assert.AreEqual(Vector3.zero, result,
                    "A null player must degrade to a neutral (zero) direction without throwing.");
            });
        }
    }
}