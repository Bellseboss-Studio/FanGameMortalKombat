using System.Collections;
using UnityEngine;

namespace StatesOfEnemies
{
    public class FollowTarget : IEnemyState
    {
        public IEnumerator DoAction(IBehavior behavior)
        {
            /*Debug.Log("Follow the player");*/
            /*yield return new WaitForSeconds(0.1f);*/
            while (behavior.IsPlayerInRedZone())
            {
                Debug.Log("isFollowing");
                if (behavior.IsPlayerInYellowZone() && behavior.IsPlayerInRangeOfAttack())
                {
                    behavior.StopMovementForAttack();
                    behavior.LookPlayer(behavior.GetTarget());
                    behavior.SetNextState(EnemyStatesConfiguration.AttackPlayer);
                    break;
                }
                if (!behavior.IsPlayerInYellowZone())
                {
                    behavior.StopMovementForAttack();
                    behavior.SetNextState(EnemyStatesConfiguration.LookPlayer);
                    break;
                }

                Debug.Log("walkToPlayer");
                behavior.WalkToPlayer();
                yield return null;
            }

            if (behavior.GetNextState() == 0)
            {
                behavior.SetNextState(EnemyStatesConfiguration.LookPlayer);
            }
        }
    }
}

