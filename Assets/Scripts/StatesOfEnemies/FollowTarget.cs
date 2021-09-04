using System.Collections;
using UnityEngine;

namespace StatesOfEnemies
{
    public class FollowTarget : IEnemyState
    {
        public IEnumerator DoAction(IBehavior behavior)
        {
            Debug.Log("Follow the player");
            yield return new WaitForSeconds(0.1f);
            while (behavior.IsPlayerInRedZone())
            {
                if (behavior.IsPlayerInRangeOfAttack())
                {
                    behavior.SetNextState(EnemyStatesConfiguration.AttackPlayer);
                    break;
                }
                if (!behavior.IsPlayerInYellowZone())
                {
                    Debug.Log("no esta dentro de la zona amarilla");
                    behavior.SetNextState(EnemyStatesConfiguration.LookPlayer);
                    break;
                }
                Debug.Log("esta en la red zone");
                behavior.WalkToPoint(behavior.GetTarget());
                yield return new WaitForSeconds(0.1f);
            }

            if (behavior.GetNextState() == 0)
            {
                behavior.SetNextState(EnemyStatesConfiguration.LookPlayer);
            }
        }
    }
}

