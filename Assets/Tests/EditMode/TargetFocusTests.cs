using NUnit.Framework;
using UnityEngine;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// EditMode unit tests for <see cref="TargetFocus"/> closest-enemy selection
    /// (spec player-character P8, design D7). Enemies are registered through the
    /// internal <see cref="TargetFocus.AddEnemy"/> seam — the same method the
    /// trigger path uses after layer/tag filtering — so the selection math is
    /// exercised without a physics scene.
    /// </summary>
    public class TargetFocusTests
    {
        private GameObject _focusGo;
        private TargetFocus _targetFocus;
        private GameObject _nearEnemy;
        private GameObject _farEnemy;

        [SetUp]
        public void SetUp()
        {
            _focusGo = new GameObject("Focus");
            _focusGo.transform.position = Vector3.zero;
            _targetFocus = _focusGo.AddComponent<TargetFocus>();

            _nearEnemy = new GameObject("NearEnemy");
            _nearEnemy.transform.position = new Vector3(1f, 0f, 0f);
            _farEnemy = new GameObject("FarEnemy");
            _farEnemy.transform.position = new Vector3(5f, 0f, 0f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_focusGo);
            Object.DestroyImmediate(_nearEnemy);
            Object.DestroyImmediate(_farEnemy);
        }

        [Test]
        public void GetClosestEnemy_WithMultipleEnemies_ReturnsNearest()
        {
            _targetFocus.AddEnemy(_farEnemy);
            _targetFocus.AddEnemy(_nearEnemy);

            Assert.AreSame(_nearEnemy, _targetFocus.GetClosestEnemy(),
                "The enemy closest to the focus transform must be selected.");
        }

        [Test]
        public void GetClosestEnemy_EmptyList_ReturnsNull()
        {
            // No enemies registered -> nothing to select.
            Assert.IsNull(_targetFocus.GetClosestEnemy(),
                "With an empty enemy list the closest enemy must be null.");
        }

        [Test]
        public void GetTarget_WithEnemies_ReturnsClosestEnemyPosition()
        {
            _targetFocus.AddEnemy(_farEnemy);
            _targetFocus.AddEnemy(_nearEnemy);

            Assert.AreEqual(_nearEnemy.transform.position, _targetFocus.GetTarget(),
                "GetTarget must return the closest enemy's position.");
        }

        [Test]
        public void RotateToTarget_WithEnemies_ReturnsDirectionTowardClosest()
        {
            _targetFocus.AddEnemy(_nearEnemy);
            _targetFocus.AddEnemy(_farEnemy);

            var direction = _targetFocus.RotateToTarget(Vector3.forward);

            Assert.AreEqual(0f, direction.y, "The rotation direction must stay on the XZ plane.");
            Assert.Greater(direction.x, 0.9f,
                "The direction must point toward the enemy at (1,0,0).");
            Assert.LessOrEqual(direction.magnitude, 1.0001f,
                "The rotation direction must be normalized.");
        }
    }
}