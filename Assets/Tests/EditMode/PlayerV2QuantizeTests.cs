using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// EditMode unit tests for <see cref="MovementRigidbodyV2"/> single-sourced
    /// input quantization (spec player-v2-movement "single-sourced quantization
    /// config", design "quantization single-sourcing").
    ///
    /// Rotation must consume the SAME quantized input as movement — no duplicated
    /// thresholds. Modern style quantizes per-axis to a 0/minSpeed/maxSpeed step;
    /// ShaolinMonks style snaps to cardinal/diagonal directions. The dead zone is
    /// single-sourced at input magnitude &lt; 0.1 (Modern) and effective inputMax
    /// matches the prefab configuration (inputMax = 0.5).
    /// </summary>
    public class PlayerV2QuantizeTests
    {
        private const float InputMin = 0.1f;
        private const float InputMax = 0.5f; // prefab PlayerV2Update inputMax
        private const float MinSpeed = 0.25f;
        private const float MaxSpeed = 1f;

        private const float Epsilon = 1e-3f;

        private static void SetPrivateField(object instance, string fieldName, object value)
        {
            var field = typeof(MovementRigidbodyV2).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, $"Field {fieldName} must exist on MovementRigidbodyV2.");
            field.SetValue(instance, value);
        }

        [Test]
        public void QuantizeModern_DeadZone_ReturnsZero()
        {
            // Input magnitude < 0.1 (single-sourced dead zone) must yield zero.
            var result = MovementRigidbodyV2.QuantizeModern(
                new Vector2(0.05f, 0f), false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(Vector2.zero, result,
                "Sub-dead-zone input must quantize to zero on both axes.");
        }

        [Test]
        public void QuantizeModern_PartialAxis_ReturnsMinSpeed()
        {
            // 0.1 <= axis < 0.5 maps to the min speed step.
            var result = MovementRigidbodyV2.QuantizeModern(
                new Vector2(0.3f, 0f), false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(MinSpeed, result.x, Epsilon,
                "Partial input must map to minSpeed on the active axis.");
            Assert.AreEqual(0f, result.y, Epsilon,
                "The inactive axis must stay at zero.");
        }

        [Test]
        public void QuantizeModern_FullAxis_ReturnsMaxSpeed()
        {
            var result = MovementRigidbodyV2.QuantizeModern(
                new Vector2(1f, 0f), false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(MaxSpeed, result.x, Epsilon,
                "Full input must map to maxSpeed on the active axis.");
        }

        [Test]
        public void QuantizeModern_EffectiveInputMax_MatchesPrefab()
        {
            // Prefab inputMax = 0.5: an axis at exactly 0.5 is already "full"
            // (>= inputMax) and must map to maxSpeed, never stay at minSpeed.
            var result = MovementRigidbodyV2.QuantizeModern(
                new Vector2(0.5f, 0f), false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(MaxSpeed, result.x, Epsilon,
                "An axis at the effective inputMax (0.5, prefab value) must map to maxSpeed.");
        }

        [Test]
        public void QuantizeModern_NegativeAxis_ReturnsNegativeStep()
        {
            var result = MovementRigidbodyV2.QuantizeModern(
                new Vector2(0f, -1f), false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(0f, result.x, Epsilon, "The inactive axis must stay at zero.");
            Assert.AreEqual(-MaxSpeed, result.y, Epsilon,
                "Negative full input must map to negative maxSpeed.");
        }

        [Test]
        public void QuantizeModern_MixedAxes_QuantizesEachAxisIndependently()
        {
            var result = MovementRigidbodyV2.QuantizeModern(
                new Vector2(0.3f, 1f), false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(MinSpeed, result.x, Epsilon, "Partial x must map to minSpeed.");
            Assert.AreEqual(MaxSpeed, result.y, Epsilon, "Full y must map to maxSpeed.");
        }

        [Test]
        public void QuantizeCardinal_DeadZone_ReturnsZero()
        {
            var result = MovementRigidbodyV2.QuantizeCardinalStatic(
                new Vector2(0.1f, 0.1f), 0.25f, true);

            Assert.AreEqual(Vector2.zero, result,
                "Cardinal quantization below the monks dead zone must yield zero.");
        }

        [Test]
        public void QuantizeCardinal_DominantAxis_ReturnsCardinal()
        {
            var result = MovementRigidbodyV2.QuantizeCardinalStatic(
                new Vector2(1f, 0.2f), 0.25f, true);

            Assert.AreEqual(1f, result.x, Epsilon,
                "The dominant x axis must snap to a clean cardinal step.");
            Assert.AreEqual(0f, result.y, Epsilon,
                "The minor axis must be dropped (no diagonal snap).");
        }

        [Test]
        public void QuantizeCardinal_Diagonal_ReturnsNormalizedDiagonal()
        {
            var result = MovementRigidbodyV2.QuantizeCardinalStatic(
                new Vector2(1f, 1f), 0.25f, true);

            var expected = new Vector2(1f, 1f).normalized;
            Assert.AreEqual(expected.x, result.x, Epsilon,
                "Diagonal input must snap to the normalized diagonal.");
            Assert.AreEqual(expected.y, result.y, Epsilon,
                "Diagonal input must snap to the normalized diagonal.");

            // Same logic with normalizeDiagonal=false keeps the raw unit diagonal.
            var raw = MovementRigidbodyV2.QuantizeCardinalStatic(
                new Vector2(1f, 1f), 0.25f, false);
            Assert.AreEqual(1f, raw.x, Epsilon, "Without normalizeDiagonal the step stays (1,1).");
            Assert.AreEqual(1f, raw.y, Epsilon, "Without normalizeDiagonal the step stays (1,1).");
        }

        [Test]
        public void QuantizeInput_ModernStyle_DispatchesToStepQuantization()
        {
            var go = new GameObject("QuantizeModern");
            var mv = go.AddComponent<MovementRigidbodyV2>();
            SetPrivateField(mv, "inputMin", InputMin);
            SetPrivateField(mv, "inputMax", InputMax);
            SetPrivateField(mv, "minSpeed", MinSpeed);
            SetPrivateField(mv, "maxSpeed", MaxSpeed);
            SetPrivateField(mv, "movementStyle", 0); // MovementStyle.Modern
            SetPrivateField(mv, "_isTarget", false);

            var result = mv.QuantizeInput(new Vector2(1f, 0f));

            Assert.AreEqual(MaxSpeed, result.x, Epsilon,
                "Style-aware QuantizeInput must dispatch to Modern step quantization.");
            Assert.AreEqual(0f, result.y, Epsilon);
            Object.DestroyImmediate(go);
        }

        [Test]
        public void QuantizeInput_ShaolinMonksStyle_DispatchesToCardinalQuantization()
        {
            var go = new GameObject("QuantizeMonks");
            var mv = go.AddComponent<MovementRigidbodyV2>();
            SetPrivateField(mv, "movementStyle", 1); // MovementStyle.ShaolinMonks
            SetPrivateField(mv, "monksCardinalDeadZone", 0.25f);
            SetPrivateField(mv, "normalizeDiagonal", true);

            var result = mv.QuantizeInput(new Vector2(1f, 1f));

            var expected = new Vector2(1f, 1f).normalized;
            Assert.AreEqual(expected.x, result.x, Epsilon,
                "Style-aware QuantizeInput must dispatch to cardinal quantization.");
            Assert.AreEqual(expected.y, result.y, Epsilon);
            Object.DestroyImmediate(go);
        }
    }
}