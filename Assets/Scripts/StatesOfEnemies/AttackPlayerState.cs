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
            if(behavior.GetIAmDeath()) behavior.SetNextState(EnemyStatesConfiguration.Death);
            while (behavior.IsPlayerInYellowZone() && behavior.IsPlayerInRangeOfAttack())
            {
                if (behavior.GetIAmDeath()) break;
                yield return new WaitForSeconds(behavior.GetEnemyVelocity());
                if (behavior.IsPlayerInYellowZone() && behavior.IsPlayerInRangeOfAttack())
                {
                    behavior.Attack();
                }
                yield return new WaitForSeconds(0.1f);
            }

            behavior.SetNextState(behavior.GetIAmDeath()
                ? EnemyStatesConfiguration.Death
                : EnemyStatesConfiguration.FollowTarget);
        }
    }
}