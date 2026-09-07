using NUnit.Framework;
using UnityEngine;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// EditMode unit tests for <see cref="MovementRigidbodyV2"/> speed math
    /// (spec player-character P4, design D7). The dead-zone input axis is mapped
    /// to a normalized speed step (0, minSpeed, or maxSpeed) depending on
    /// magnitude thresholds and target-lock mode.
    /// </summary>
    public class MovementRigidbodyV2SpeedMathTests
    {
        private const float InputMin = 0.1f;
        private const float InputMax = 0.8f;
        private const float MinSpeed = 0.2f;
        private const float MaxSpeed = 1f;

        [Test]
        public void CalculateDirection_BelowDeadZone_ReturnsZero()
        {
            var result = MovementRigidbodyV2.CalculateDirection(0.05f, false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(0f, result, "Input below the dead zone must map to zero.");
        }

        [Test]
        public void CalculateDirection_PartialAxis_ReturnsMinSpeed()
        {
            var result = MovementRigidbodyV2.CalculateDirection(0.5f, false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(MinSpeed, result, "Input between dead zone and max threshold maps to min speed.");
        }

        [Test]
        public void CalculateDirection_FullAxis_ReturnsMaxSpeed()
        {
            var result = MovementRigidbodyV2.CalculateDirection(1f, false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(MaxSpeed, result, "Input at or above max threshold maps to max speed.");
        }

        [Test]
        public void CalculateDirection_NegativeFullAxis_ReturnsNegativeMaxSpeed()
        {
            var result = MovementRigidbodyV2.CalculateDirection(-1f, false, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(-MaxSpeed, result, "Negative full input maps to negative max speed.");
        }

        [Test]
        public void CalculateDirection_TargetMode_ReturnsMinSpeedForAnyActiveAxis()
        {
            // In target-lock mode any non-dead input maps to min speed (design P8).
            var result = MovementRigidbodyV2.CalculateDirection(0.9f, true, InputMin, InputMax, MinSpeed, MaxSpeed);

            Assert.AreEqual(MinSpeed, result,
                "Target-lock mode must use min speed for any active axis (no full run).");
        }
    }
}