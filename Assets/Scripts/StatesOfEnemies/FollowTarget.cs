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
            while (behavior.IsPLayerInRedZone())
            {
                behavior.WalkToPoint(behavior.GetTarget());
                yield return new WaitForSeconds(0.1f);
            }
            behavior.SetNextState(EnemyStatesConfiguration.PatrolState);
        }
    }
}