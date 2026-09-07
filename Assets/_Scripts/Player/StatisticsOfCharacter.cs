using UnityEngine;

namespace _Scripts.Player
{
    [CreateAssetMenu(menuName = "Bellseboss/CharacterStatistics", fileName = "StatisticsOfCharacter", order = 0)]
    public class StatisticsOfCharacter : ScriptableObject
    {
        public int life;
        public int damage;
        public float speedToMoveAngry;
        public float speedToMoveNormal;
        public float speedToMoveScared;
        public float timeToAttack;
        public float timeBetweenAttacks;
        public string attackAnimationName;
        public AttackMovementSystem.TypeOfAttack attackAnimationType;
        public float timeToActivateCollider;
        public float timeToEnableCollider;

        [Tooltip("Energy to add in percentage each attack.")]
        public float energyToAdd;

        public float energy;

        /// <summary>
        /// Fail-fast configuration validation (spec character-configuration C3):
        /// the statistics are playable only when life &gt; 0 and every movement
        /// speed is &gt;= 0. Returns false for any invalid field so callers can
        /// log a clear error and skip half-initialization.
        /// </summary>
        public bool IsValid()
        {
            return life > 0
                   && speedToMoveAngry >= 0f
                   && speedToMoveNormal >= 0f
                   && speedToMoveScared >= 0f;
        }
    }
}