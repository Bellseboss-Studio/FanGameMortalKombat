using System.Collections.Generic;
using UnityEngine;

namespace View.Characters
{
    public interface IEnemyCharacter
    {
        void MoveToPoint(Vector3 toPoint);
        List<Vector3> GetPoints();
        bool IsEnemyArrived(Vector3 concurrentPoint);
        void SubscribeOnPlayerEnterTrigger(EnemyDefaultCharacter.OnPlayerTrigger action);
        void UnsubscribeOnPlayerEnterTrigger(EnemyDefaultCharacter.OnPlayerTrigger action);
        void CleanOnPlayerEnterTrigger();
        void SubscribeOnPlayerExitTrigger(EnemyDefaultCharacter.OnPlayerTrigger action);
        void UnsubscribeOnPlayerExitTrigger(EnemyDefaultCharacter.OnPlayerTrigger action);
        void CleanOnPlayerExitTrigger();
    }
}