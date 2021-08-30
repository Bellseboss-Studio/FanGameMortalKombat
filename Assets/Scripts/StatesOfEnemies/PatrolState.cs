using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatesOfEnemies
{
    public class PatrolState : IEnemyState
    {
        private readonly List<Vector3> _points;
        private Vector3 concurrentPoint;

        public PatrolState(List<Vector3> points)
        {
            _points = points;
        }

        public IEnumerator DoAction(IBehavior behavior)
        {
            Debug.Log("PatrolState");
            concurrentPoint = _points[behavior.GetRandom(0, _points.Count)];
            while (true)
            {
                while (!behavior.IsEnemyArrived(concurrentPoint))
                {
                    behavior.WalkToPoint(concurrentPoint);
                    if (behavior.IsPlayerInRedZone())
                    {
                        behavior.SetNextState(EnemyStatesConfiguration.FollowTarget);
                        break;
                    }
                    yield return new WaitForSeconds(0.1f);
                }

                if (behavior.GetNextState() != 0)
                {
                    break;
                }
                yield return new WaitForSeconds(0.1f);
                concurrentPoint = _points[behavior.GetRandom(0, _points.Count)];;
            }
        }
    }
}