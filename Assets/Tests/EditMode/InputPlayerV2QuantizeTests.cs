using NUnit.Framework;
using UnityEngine;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// EditMode unit tests for <see cref="InputPlayerV2"/> direction quantization
    /// (spec player-character P3 input routing, design D7). The quantization math
    /// maps an analog Vector2 to a cardinal <see cref="INPUTS"/> direction.
    /// </summary>
    public class InputPlayerV2QuantizeTests
    {
        [Test]
        public void GetDirectionFromVector_RightDominant_ReturnsRight()
        {
            Assert.AreEqual(INPUTS.RIGHT, InputPlayerV2.GetDirectionFromVector(new Vector2(1f, 0.2f)));
        }

        [Test]
        public void GetDirectionFromVector_LeftDominant_ReturnsLeft()
        {
            Assert.AreEqual(INPUTS.LEFT, InputPlayerV2.GetDirectionFromVector(new Vector2(-1f, 0.2f)));
        }

        [Test]
        public void GetDirectionFromVector_UpDominant_ReturnsUp()
        {
            Assert.AreEqual(INPUTS.UP, InputPlayerV2.GetDirectionFromVector(new Vector2(0.2f, 1f)));
        }

        [Test]
        public void GetDirectionFromVector_DownDominant_ReturnsDown()
        {
            Assert.AreEqual(INPUTS.DOWN, InputPlayerV2.GetDirectionFromVector(new Vector2(0.2f, -1f)));
        }

        [Test]
        public void GetDirectionFromVector_DeadZone_ReturnsNone()
        {
            Assert.AreEqual(INPUTS.NONE, InputPlayerV2.GetDirectionFromVector(Vector2.zero));
        }

        [Test]
        public void GetDirectionFromVector_SubThresholdAxis_ReturnsNone()
        {
            // x = 0.005 is inside the 0.01 dead zone -> no direction.
            Assert.AreEqual(INPUTS.NONE, InputPlayerV2.GetDirectionFromVector(new Vector2(0.005f, 0f)));
        }

        [Test]
        public void GetDirectionFromVector_NegativeBelowThreshold_ReturnsNone()
        {
            Assert.AreEqual(INPUTS.NONE, InputPlayerV2.GetDirectionFromVector(new Vector2(-0.005f, 0f)));
        }
    }
}