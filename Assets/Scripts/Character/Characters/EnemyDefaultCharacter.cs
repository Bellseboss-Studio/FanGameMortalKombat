using CharacterCustom;
using StatesOfEnemies;
using UnityEngine;

namespace View.Characters
{
    public class EnemyDefaultCharacter : Character, IEnemyCharacter
    {
        protected EnemyBehavior behaviorEnemy;
        protected override void ConfigureExplicit()
        {
            
        }

        public void SetBehavior()
        {
            behaviorEnemy = gameObject.AddComponent<EnemyBehavior>();
            var enemyState = behaviorEnemy.Configuration(this);
            StartCoroutine(behaviorEnemy.StartState(enemyState));
        }

        public void MoveToPoint(Vector3 toPoint)
        {
            movementPlayer = (toPoint - transform.position).normalized;
        }

        public Vector3 GetPointA()
        {
            return Vector3.forward;
        }

        public Vector3 GetPointB()
        {
            return Vector3.back;
        }
    }
}