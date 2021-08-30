using System.Collections;
using UnityEngine;
using View.Characters;

namespace StatesOfEnemies
{
    public class WaitState : IEnemyState
    {
        public IEnumerator DoAction(IBehavior behavior)
        {
            Debug.Log("waitState");
            while (!behavior.IsPlayerInYellowZone() && behavior.IsPlayerInGreenZone())
            {
                behavior.LookPlayer(behavior.GetTarget());
                yield return new WaitForSeconds(0.1f);
            }

            if (!behavior.IsPlayerInGreenZone())
            {
                behavior.SetNextState(EnemyStatesConfiguration.PatrolState);
            }else if (behavior.IsPlayerInYellowZone())
            {
                behavior.SetNextState(EnemyStatesConfiguration.FollowTarget);
            }
        }
    }
}