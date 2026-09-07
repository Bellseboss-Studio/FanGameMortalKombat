using NUnit.Framework;
using UnityEngine;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// EditMode unit tests for <see cref="StatisticsOfCharacter"/> configuration
    /// validation (spec character-configuration C3, design D3): a statistics
    /// object is valid only when life &gt; 0 and all movement speeds &gt;= 0.
    /// The isolated player prefab uses the same asset shape as
    /// <c>StatisticsOfCharacter_Player.asset</c> (life 3, speeds 0), which must
    /// validate as playable.
    /// </summary>
    public class StatisticsValidationTests
    {
        private static StatisticsOfCharacter CreateStats(int life, float angry, float normal, float scared)
        {
            var stats = ScriptableObject.CreateInstance<StatisticsOfCharacter>();
            stats.life = life;
            stats.speedToMoveAngry = angry;
            stats.speedToMoveNormal = normal;
            stats.speedToMoveScared = scared;
            return stats;
        }

        [Test]
        public void IsValid_PlayerPrefabShapedStats_ReturnsTrue()
        {
            // Mirrors StatisticsOfCharacter_Player.asset (life 3, speeds 0).
            var stats = CreateStats(3, 0f, 0f, 0f);

            Assert.IsTrue(stats.IsValid(),
                "life > 0 and all speeds >= 0 must be a valid configuration.");
        }

        [Test]
        public void IsValid_ZeroLife_ReturnsFalse()
        {
            var stats = CreateStats(0, 0f, 0f, 0f);

            Assert.IsFalse(stats.IsValid(),
                "life <= 0 must be rejected (C3 fail-fast).");
        }

        [Test]
        public void IsValid_NegativeLife_ReturnsFalse()
        {
            var stats = CreateStats(-5, 0f, 0f, 0f);

            Assert.IsFalse(stats.IsValid(),
                "negative life must be rejected (C3 fail-fast).");
        }

        [Test]
        public void IsValid_NegativeSpeed_ReturnsFalse()
        {
            var stats = CreateStats(3, 0f, -1f, 0f);

            Assert.IsFalse(stats.IsValid(),
                "negative movement speed must be rejected (C3 fail-fast).");
        }
    }
}