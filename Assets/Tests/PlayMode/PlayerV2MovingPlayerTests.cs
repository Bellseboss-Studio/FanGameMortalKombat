using System.Collections;
using System.Reflection;
using NUnit.Framework;
using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// PR3 moving-player PlayMode suite (spec player-v2-movement-orientation-alignment
    /// "moving-player coverage", design "PlayerV2MovingPlayerTests", tasks Phase 4.1):
    /// proves the canonical yaw-only camera basis stays STABLE while the player
    /// actually TRANSLATES — the exact condition the old positional
    /// (player - camera) basis failed (strafe tremble + W/S transient diagonal walk).
    ///
    /// Harness rules (deliberate, non-vacuous):
    ///   - The player XZ is NEVER pinned: position is set ONCE per test for setup
    ///     (floor placement / perpendicular facing), then the player translates
    ///     freely under real rigidbody physics. Every sampling window asserts a
    ///     position-delta guard (&gt;= 0.5 m) proving real translation.
    ///   - Cinemachine PositionDamping stays ACTIVE and non-zero (1,1,1) — verified
    ///     on the live scene vcam in SetUp. The suite runs under the exact damping
    ///     that used to feed the tremble back into gameplay; PR2's StableYawReference
    ///     decouples the gameplay basis from it, and these tests prove that end to
    ///     end while the player moves.
    ///   - Movement/rotation are driven with the same raw input via Direction()
    ///     (bypassing real input devices), using a deterministic test camera whose
    ///     transform the shared helper reads (same harness as PlayerV2AlignmentTests).
    ///   - Raw input magnitude 0.3 sits inside the [inputMin, inputMax) band, so it
    ///     quantizes to the min-speed (WALK) step: ~3 m/s Modern, ~10 m/s
    ///     ShaolinMonks. Fast enough to exercise the dynamics, slow enough to keep
    ///     the player on the 10x10 test floor for the whole window.
    ///   - The transient is measured deterministically: per-fixed-frame sampling
    ///     with per-frame angle bounds — not only final alignment.
    /// </summary>
    public class PlayerV2MovingPlayerTests
    {
        private const string TestScenePath = "Assets/_Scenes/Tests/PlayerV2Test.unity";
        private const float MaxAngleDegrees = 5f;
        private const int MaxSettleFrames = 300;
        private const int StableSampleFrames = 30;
        private const int TransientWindowFrames = 60;
        private const float MinTranslationMeters = 0.5f;
        private const float MinSustainedSpeed = 0.5f;
        private const float WalkInput = 0.3f; // in [inputMin, inputMax) -> min-speed (walk) step

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

            // Requirement: Cinemachine PositionDamping stays ACTIVE/nonzero while
            // these tests run. Read the live scene vcam body and fail loudly if the
            // damping was ever zeroed (the rejected PositionDamping=0 workaround).
            var cmCamera = GameObject.Find("CM Camera");
            Assert.IsNotNull(cmCamera, "The test scene must contain the CM Camera vcam.");
            var follow = cmCamera.GetComponent<CinemachineFollow>();
            Assert.IsNotNull(follow, "CM Camera must have a CinemachineFollow body (damping host).");
            var damping = follow.TrackerSettings.PositionDamping;
            Assert.IsTrue(damping.sqrMagnitude > 0f,
                "Cinemachine PositionDamping must stay ACTIVE (nonzero) during the moving-player suite.");
            Assert.AreEqual(new Vector3(1f, 1f, 1f), damping,
                "Cinemachine PositionDamping must retain its current value (1,1,1) — no damping workaround.");

            // Cinemachine re-drives a vcam's transform every LateUpdate, so pinning
            // the scene's CM Camera pose is unreliable in play mode. Instead the
            // tests hand the systems a plain GameObject whose transform is fully
            // deterministic. The components only read camera.transform.forward (XZ)
            // for the yaw-only canonical basis (see PlayerV2AlignmentTests).
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
        /// Places the player ONCE (setup only — XZ is NEVER pinned afterwards) and
        /// lets the body settle on the floor before the test drives input.
        /// </summary>
        private IEnumerator StartPlayerAt(Vector3 position, Quaternion rotation)
        {
            _player.transform.SetPositionAndRotation(position, rotation);
            var rb = _player.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            yield return null;
            yield return null;
        }

        /// <summary>
        /// Independently derives the canonical world direction for the given raw
        /// input from the test camera transform (yaw-only forward XZ projection,
        /// right = (fwd.z, 0, -fwd.x)) — the SAME contract the shared helper must
        /// satisfy, derived WITHOUT calling the helper so the assertion cannot
        /// mask a helper regression.
        /// </summary>
        private Vector3 ComputeExpectedDirection(Vector2 quantizedInput)
        {
            var fwd = _testCamera.transform.forward;
            fwd.y = 0f;
            fwd.Normalize();
            var right = new Vector3(fwd.z, 0f, -fwd.x);
            var result = quantizedInput.x * right + quantizedInput.y * fwd;
            if (result.sqrMagnitude <= 0.0001f) return Vector3.zero;
            return result.normalized;
        }

        /// <summary>
        /// Spec settle gate: facing unchanged within 1° for 3 consecutive fixed
        /// frames, capped at MaxSettleFrames. Re-zeros rotation._lastDirection
        /// every frame so the turn runs at full baseRotationSpeed (900°/s in the
        /// test scene) instead of decaying to the min-velocity crawl — identical
        /// harness to PlayerV2AlignmentTests.WaitForSettle. The player XZ is NOT
        /// pinned here either.
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
        /// Warm-up + stable-window sampling for the strafe tests: drives the input,
        /// settles the facing on the canonical direction, then samples N fixed
        /// frames asserting facing AND velocityXZ stay within 5° of the canonical
        /// direction EVERY frame (a positional-basis regression swings the basis as
        /// the player translates and fails hard here) with a position-delta guard
        /// and a sustained-speed guard (assertions are never vacuous).
        /// </summary>
        private IEnumerator AssertStableMovingWindow(
            Vector2 input, Vector3 expectedDir, int frames, string label)
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            var rb = _player.GetComponent<Rigidbody>();
            Assert.IsNotNull(movement, "PlayerV2Update must have MovementRigidbodyV2.");
            Assert.IsNotNull(rotation, "PlayerV2Update must have RotationCharacterV2.");

            movement.Direction(input);
            rotation.Direction(input);

            // Wait for the first movement sync, then re-zero so the settle gate
            // measures GENUINE convergence (same harness as AlignmentTests).
            yield return new WaitForFixedUpdate();
            SetPrivateField(rotation, typeof(RotationCharacterV2), "_lastDirection", Vector3.zero);
            yield return WaitForSettle(_player.transform, rotation);

            var startPos = _player.transform.position;
            var maxFacingAngle = 0f;
            var maxVelocityAngle = 0f;
            var minSpeed = float.MaxValue;
            for (var i = 0; i < frames; i++)
            {
                movement.Direction(input); // input held for the whole window
                rotation.Direction(input);
                yield return new WaitForFixedUpdate();

                var facing = _player.transform.forward;
                var velXZ = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                var facingAngle = Vector3.Angle(facing, expectedDir);
                maxFacingAngle = Mathf.Max(maxFacingAngle, facingAngle);
                Assert.LessOrEqual(facingAngle, MaxAngleDegrees,
                    $"{label}: facing drifted {facingAngle:F2}° from canonical at fixed frame {i} " +
                    $"(tremble — positional-basis regression would swing the target).");

                if (velXZ.magnitude > 0.1f)
                {
                    var velocityAngle = Vector3.Angle(velXZ, expectedDir);
                    maxVelocityAngle = Mathf.Max(maxVelocityAngle, velocityAngle);
                    Assert.LessOrEqual(velocityAngle, MaxAngleDegrees,
                        $"{label}: velocityXZ {velocityAngle:F2}° off canonical at fixed frame {i} " +
                        $"(movement chases a moving basis).");
                    minSpeed = Mathf.Min(minSpeed, velXZ.magnitude);
                }
            }

            var travelled = Vector3.Distance(startPos, _player.transform.position);
            Assert.Greater(travelled, MinTranslationMeters,
                $"{label}: player must translate >= {MinTranslationMeters}m during sampling " +
                $"(was {travelled:F2}m) — test would be vacuous if XZ were pinned.");
            Assert.Greater(minSpeed, MinSustainedSpeed,
                $"{label}: sustained velocity must exceed {MinSustainedSpeed} m/s every sampled frame " +
                $"(was {minSpeed:F2}) — not a ghost assertion.");
            Assert.LessOrEqual(maxFacingAngle, MaxAngleDegrees,
                $"{label}: worst-case facing angle must stay within {MaxAngleDegrees}°.");
            Assert.LessOrEqual(maxVelocityAngle, MaxAngleDegrees,
                $"{label}: worst-case velocity angle must stay within {MaxAngleDegrees}°.");
        }

        /// <summary>
        /// W/S transient measurement from a stationary perpendicular facing at a
        /// lateral offset where the OLD positional basis was 90° wrong. Samples
        /// EVERY fixed frame for the transient window:
        ///   - velocityXZ must equal the canonical direction within 5° EVERY frame
        ///     (no transient diagonal walk — the old basis walked 90° off here),
        ///   - facing must converge within 5° of canonical inside the bounded
        ///     window (immediate alignment),
        ///   - velocity and facing must agree within 5° once settled,
        ///   - the player must have translated >= MinTranslationMeters.
        /// </summary>
        private IEnumerator AssertNoTransientDiagonal(
            Vector2 input, Vector3 expectedDir, string label)
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            var rb = _player.GetComponent<Rigidbody>();
            Assert.IsNotNull(movement, "PlayerV2Update must have MovementRigidbodyV2.");
            Assert.IsNotNull(rotation, "PlayerV2Update must have RotationCharacterV2.");

            var startPos = _player.transform.position;
            movement.Direction(input);
            rotation.Direction(input);

            var facingConvergedFrame = -1;
            var maxVelocityAngle = 0f;
            var lastVelocity = Vector3.zero;
            for (var i = 0; i < TransientWindowFrames; i++)
            {
                movement.Direction(input); // input held for the whole window
                rotation.Direction(input);
                yield return new WaitForFixedUpdate();

                var facing = _player.transform.forward;
                var velXZ = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                if (velXZ.magnitude > 0.1f)
                {
                    var velocityAngle = Vector3.Angle(velXZ, expectedDir);
                    maxVelocityAngle = Mathf.Max(maxVelocityAngle, velocityAngle);
                    Assert.LessOrEqual(velocityAngle, MaxAngleDegrees,
                        $"{label}: velocityXZ {velocityAngle:F2}° off canonical at fixed frame {i} " +
                        $"(transient diagonal walk — velocity must be canonical every frame).");
                    lastVelocity = velXZ;
                }

                if (facingConvergedFrame < 0 &&
                    Vector3.Angle(facing, expectedDir) <= MaxAngleDegrees)
                {
                    facingConvergedFrame = i;
                }
            }

            Assert.Greater(facingConvergedFrame, 0,
                $"{label}: facing must converge within {MaxAngleDegrees}° of canonical inside the " +
                $"{TransientWindowFrames}-frame window (was still unaligned — no immediate alignment).");
            Assert.LessOrEqual(maxVelocityAngle, MaxAngleDegrees,
                $"{label}: worst-case velocity angle must stay within {MaxAngleDegrees}° (no diagonal walk).");

            // Settled agreement contract: once both systems converge, velocity and
            // facing must agree within 5° (spec "velocity and facing agree within 5°").
            var settledFacing = _player.transform.forward;
            Assert.Greater(lastVelocity.magnitude, 0.1f,
                $"{label}: the player must still be moving at the end of the window.");
            Assert.LessOrEqual(Vector3.Angle(lastVelocity, settledFacing), MaxAngleDegrees,
                $"{label}: settled velocity and facing must agree within {MaxAngleDegrees}° " +
                $"(velocity {lastVelocity}, facing {settledFacing}).");

            var travelled = Vector3.Distance(startPos, _player.transform.position);
            Assert.Greater(travelled, MinTranslationMeters,
                $"{label}: player must translate >= {MinTranslationMeters}m during the window " +
                $"(was {travelled:F2}m) — test would be vacuous if XZ were pinned.");
        }

        private static void SetMovementStyle(MovementRigidbodyV2 movement, int style)
        {
            var styleField = typeof(MovementRigidbodyV2).GetField(
                "movementStyle", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(styleField, "MovementRigidbodyV2.movementStyle must exist.");
            var enumType = typeof(MovementRigidbodyV2).GetNestedType(
                "MovementStyle", BindingFlags.NonPublic);
            Assert.IsNotNull(enumType, "MovementRigidbodyV2.MovementStyle must exist.");
            styleField.SetValue(movement, System.Enum.ToObject(enumType, style));
        }

        // ------------------------------------------------------------------
        // 1-2) Strafe L/R while translating: facing AND velocityXZ stay within
        //      5° of the canonical strafe direction EVERY sampled frame.
        //      Player starts off the camera's z-axis (z=3) so a positional-basis
        //      regression swings the canonical direction as the player translates.
        // ------------------------------------------------------------------

        [UnityTest]
        public IEnumerator StrafeRight_WhileMoving_NoTremble()
        {
            yield return StartPlayerAt(new Vector3(0f, 1f, 3f), Quaternion.LookRotation(Vector3.forward));
            yield return AssertStableMovingWindow(
                new Vector2(WalkInput, 0f), new Vector3(1f, 0f, 0f), StableSampleFrames, "StrafeRight");
        }

        [UnityTest]
        public IEnumerator StrafeLeft_WhileMoving_NoTremble()
        {
            yield return StartPlayerAt(new Vector3(0f, 1f, 3f), Quaternion.LookRotation(Vector3.forward));
            yield return AssertStableMovingWindow(
                new Vector2(-WalkInput, 0f), new Vector3(-1f, 0f, 0f), StableSampleFrames, "StrafeLeft");
        }

        // ------------------------------------------------------------------
        // 3-4) W/S from a stationary perpendicular facing at a lateral offset
        //      where the OLD positional basis was 90° wrong: velocityXZ must be
        //      canonical EVERY frame (no transient diagonal), facing converges
        //      inside a bounded window, settled agreement <= 5°.
        // ------------------------------------------------------------------

        [UnityTest]
        public IEnumerator W_FromPerpendicularFacing_NoTransientDiagonal()
        {
            // Player at x=4 (safely on the 10x10 floor; x=5 is the edge) facing +X —
            // perpendicular to the canonical forward (0,0,1). The old positional
            // basis at this offset pointed -X (90° wrong) and never converged.
            yield return StartPlayerAt(new Vector3(4f, 1f, 0f), Quaternion.LookRotation(Vector3.right));
            yield return AssertNoTransientDiagonal(
                new Vector2(0f, WalkInput), new Vector3(0f, 0f, 1f), "W");
        }

        [UnityTest]
        public IEnumerator S_FromPerpendicularFacing_NoTransientDiagonal()
        {
            yield return StartPlayerAt(new Vector3(4f, 1f, 0f), Quaternion.LookRotation(Vector3.right));
            yield return AssertNoTransientDiagonal(
                new Vector2(0f, -WalkInput), new Vector3(0f, 0f, -1f), "S");
        }

        // ------------------------------------------------------------------
        // 5) Camera pitch 30° + strafe while translating: pitch must NOT alter
        //    the yaw-only canonical basis (invariance under translation).
        // ------------------------------------------------------------------

        [UnityTest]
        public IEnumerator TiltedCamera30_StrafeWhileMoving_Stable()
        {
            PlaceCamera(CameraPosition, new Vector3(30f, 0f, 0f));
            yield return StartPlayerAt(new Vector3(0f, 1f, 3f), Quaternion.LookRotation(Vector3.forward));
            yield return AssertStableMovingWindow(
                new Vector2(WalkInput, 0f), new Vector3(1f, 0f, 0f), StableSampleFrames, "Tilted30StrafeRight");
        }

        // ------------------------------------------------------------------
        // 6) LookAt vertical offset camera + W/S while translating: the raised
        //    camera's vertical offset must not tilt the yaw-only basis.
        // ------------------------------------------------------------------

        [UnityTest]
        public IEnumerator LookAtOffsetCamera_WS_WhileMoving_Stable()
        {
            PlaceCamera(new Vector3(10f, 8f, 0f), IdentityRotation); // vertical LookAt offset
            yield return StartPlayerAt(new Vector3(4f, 1f, 0f), Quaternion.LookRotation(Vector3.right));
            yield return AssertNoTransientDiagonal(
                new Vector2(0f, WalkInput), new Vector3(0f, 0f, 1f), "LookAtOffsetW");
        }

        // ------------------------------------------------------------------
        // 7-8) ShaolinMonks (movementStyle=1): movement derives its world
        //      direction from the SAME canonical helper as rotation. Start the
        //      player against the wall so the fast monks speed (10 m/s) keeps it
        //      on the 10x10 floor for the whole window.
        // ------------------------------------------------------------------

        [UnityTest]
        public IEnumerator ShaolinMonks_W_SharedBasis_NoDescuadre()
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            SetMovementStyle(movement, 1); // ShaolinMonks
            yield return StartPlayerAt(new Vector3(0f, 1f, -4f), Quaternion.LookRotation(Vector3.right));
            yield return AssertShaolinMonksSharedBasis(new Vector2(0f, WalkInput), "ShaolinMonksW");
        }

        [UnityTest]
        public IEnumerator ShaolinMonks_D_SharedBasis_NoDescuadre()
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            SetMovementStyle(movement, 1); // ShaolinMonks
            yield return StartPlayerAt(new Vector3(-4f, 1f, 0f), Quaternion.LookRotation(Vector3.forward));
            yield return AssertShaolinMonksSharedBasis(new Vector2(WalkInput, 0f), "ShaolinMonksD");
        }

        /// <summary>
        /// ShaolinMonks contract: the quantized input flows through the shared
        /// helper, so velocityXZ and facing must both equal the helper output
        /// within 5° (movement and rotation NEVER disagree — no descuadre).
        /// Samples per-fixed-frame after settle with translation + speed guards.
        /// </summary>
        private IEnumerator AssertShaolinMonksSharedBasis(Vector2 input, string label)
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            var rb = _player.GetComponent<Rigidbody>();
            Assert.IsNotNull(movement, "PlayerV2Update must have MovementRigidbodyV2.");
            Assert.IsNotNull(rotation, "PlayerV2Update must have RotationCharacterV2.");

            // Single-source quantization + single-source basis (the helper). The
            // expected direction is derived independently in the test.
            var quantized = movement.QuantizeInput(input);
            var expected = ComputeExpectedDirection(quantized);
            Assert.Greater(expected.sqrMagnitude, 0.0001f, $"{label}: quantized input must be active.");

            movement.Direction(input);
            rotation.Direction(input);

            yield return new WaitForFixedUpdate();
            SetPrivateField(rotation, typeof(RotationCharacterV2), "_lastDirection", Vector3.zero);
            yield return WaitForSettle(_player.transform, rotation);

            var startPos = _player.transform.position;
            const int monksSampleFrames = 10; // 10 m/s * 10 fixed frames ~= 2 m
            var minSpeed = float.MaxValue;
            for (var i = 0; i < monksSampleFrames; i++)
            {
                movement.Direction(input);
                rotation.Direction(input);
                yield return new WaitForFixedUpdate();

                var facing = _player.transform.forward;
                var velXZ = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                Assert.LessOrEqual(Vector3.Angle(facing, expected), MaxAngleDegrees,
                    $"{label}: facing {Vector3.Angle(facing, expected):F2}° off the shared helper output " +
                    $"at frame {i} (descuadre).");
                if (velXZ.magnitude > 0.1f)
                {
                    Assert.LessOrEqual(Vector3.Angle(velXZ, expected), MaxAngleDegrees,
                        $"{label}: velocityXZ {Vector3.Angle(velXZ, expected):F2}° off the shared helper " +
                        $"output at frame {i} (movement/rotation bases disagree).");
                    minSpeed = Mathf.Min(minSpeed, velXZ.magnitude);
                }
            }

            Assert.Greater(Vector3.Distance(startPos, _player.transform.position), MinTranslationMeters,
                $"{label}: player must translate >= {MinTranslationMeters}m during sampling " +
                $"(monks path — not vacuous).");
            Assert.Greater(minSpeed, MinSustainedSpeed,
                $"{label}: sustained velocity must exceed {MinSustainedSpeed} m/s every sampled frame.");
        }

        // ------------------------------------------------------------------
        // 9) Camera yaw 45° + strafe while translating: the canonical basis is
        //    the yaw-rotated camera right; it must stay constant while moving.
        // ------------------------------------------------------------------

        [UnityTest]
        public IEnumerator Yaw45_StrafeWhileMoving_Stable()
        {
            PlaceCamera(CameraPosition, new Vector3(0f, 45f, 0f));
            yield return StartPlayerAt(new Vector3(0f, 1f, 3f), Quaternion.LookRotation(Vector3.forward));
            // Camera yaw 45 -> forward XZ (0.7071, 0, 0.7071); camera right = (0.7071, 0, -0.7071).
            yield return AssertStableMovingWindow(
                new Vector2(WalkInput, 0f),
                new Vector3(0.7071f, 0f, -0.7071f),
                StableSampleFrames, "Yaw45StrafeRight");
        }

        // ------------------------------------------------------------------
        // 10) Dead-zone while moving: after a real strafe the input drops into
        //     the dead zone -> zero velocity (no drift) and facing unchanged.
        // ------------------------------------------------------------------

        [UnityTest]
        public IEnumerator DeadZone_WhileMoving_NoVelocity_FacingUnchanged()
        {
            var movement = _player.GetComponent<MovementRigidbodyV2>();
            var rotation = _player.GetComponent<RotationCharacterV2>();
            var rb = _player.GetComponent<Rigidbody>();
            Assert.IsNotNull(movement, "PlayerV2Update must have MovementRigidbodyV2.");
            Assert.IsNotNull(rotation, "PlayerV2Update must have RotationCharacterV2.");

            // Phase 1: real strafe (warm-up) — proves the "while moving" precondition.
            // Settle alone only moves ~0.3 m (the turn completes before the walk
            // ramps up), so hold the input for a short sustained-move window until
            // the player is genuinely translating at steady speed.
            yield return StartPlayerAt(new Vector3(0f, 1f, 3f), Quaternion.LookRotation(Vector3.forward));
            var warmUpStart = _player.transform.position;
            movement.Direction(new Vector2(WalkInput, 0f));
            rotation.Direction(new Vector2(WalkInput, 0f));
            yield return new WaitForFixedUpdate();
            SetPrivateField(rotation, typeof(RotationCharacterV2), "_lastDirection", Vector3.zero);
            yield return WaitForSettle(_player.transform, rotation);
            for (var i = 0; i < 15; i++)
            {
                movement.Direction(new Vector2(WalkInput, 0f));
                rotation.Direction(new Vector2(WalkInput, 0f));
                yield return new WaitForFixedUpdate();
            }
            Assert.Greater(Vector3.Distance(warmUpStart, _player.transform.position), MinTranslationMeters,
                "DeadZone: the warm-up strafe must translate the player (while-moving precondition).");

            // Phase 2: dead-zone input (0.05 < inputMin 0.1) while the player is moving.
            var facingBefore = _player.transform.rotation;
            movement.Direction(new Vector2(0.05f, 0f));
            rotation.Direction(new Vector2(0.05f, 0f));

            for (var i = 0; i < 40; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            var velXZ = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            Assert.Less(velXZ.magnitude, 0.01f,
                $"DeadZone: dead-zone input must bring the moving player to zero velocity (was {velXZ.magnitude:F3}).");
            Assert.Less(Quaternion.Angle(facingBefore, _player.transform.rotation), 1f,
                "DeadZone: dead-zone input must leave facing unchanged (no drift, no rotation).");
        }
    }
}