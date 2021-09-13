using System.Collections;
using UnityEngine;

namespace StatesOfEnemies
{
    public class AttackPlayerState : IEnemyState
    {
        //dano, velocidad, alcance
        //dano es el numero a restar a la vida del player
        //velocidad, es el tiempo entre ataques, cuando este al alcance comienza a contar
        //alcance, 
        public IEnumerator DoAction(IBehavior behavior)
        {
            Debug.Log("Range of attack");
            while (behavior.IsPlayerInRangeOfAttack())
            {
                behavior.StopMovementForAttack();
                yield return new WaitForSeconds(behavior.GetEnemyVelocity());
                if (behavior.IsPlayerInRangeOfAttack())
                {
                    behavior.Attack();
                }
                yield return new WaitForSeconds(0.1f);
            }
            behavior.SetNextState(EnemyStatesConfiguration.FollowTarget);
        }
    }
}