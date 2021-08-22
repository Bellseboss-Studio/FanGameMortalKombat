using UnityEngine;

namespace StatesOfEnemies
{
    public interface IBehavior
    {
        void SetNextState(int nextStateFromState);
        void WalkToPoint(Vector3 toPoint);
        bool IsPLayerInRedZone();
    }
}