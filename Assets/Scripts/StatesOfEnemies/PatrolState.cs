using System.Collections;
using UnityEngine;

namespace StatesOfEnemies
{
    public class PatrolState : IEnemyState
    {
        private readonly Vector3 _pointA;
        private readonly Vector3 _pointB;
        private Vector3 concurrentPoint;

        public PatrolState(Vector3 pointA, Vector3 pointB)
        {
            _pointA = pointA;
            _pointB = pointB;
            concurrentPoint = pointB;
        }

        public IEnumerator DoAction(IBehavior behavior)
        {
            Debug.Log("PatrolState");
            behavior.WalkToPoint(concurrentPoint);
            while (!behavior.IsPLayerInRedZone())
            {
                yield return new WaitForSeconds(0.1f);
            }
            behavior.SetNextState(EnemyStatesConfiguration.comebackState);
        }
    }
}