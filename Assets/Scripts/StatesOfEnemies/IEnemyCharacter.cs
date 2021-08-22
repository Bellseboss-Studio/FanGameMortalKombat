using UnityEngine;

namespace View.Characters
{
    public interface IEnemyCharacter
    {
        void MoveToPoint(Vector3 toPoint);
        Vector3 GetPointA();
        Vector3 GetPointB();
    }
}