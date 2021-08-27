using UnityEngine;

namespace StatesOfEnemies
{
    public interface IBehavior
    {
        void SetNextState(int nextStateFromState);
        void WalkToPoint(Vector3 toPoint);
        bool IsPLayerInRedZone();
        bool IsEnemyArrived(Vector3 concurrentPoint);
        int GetNextState();
        int GetRandom(int start, int end);
    }
}