using System.Collections.Generic;
using UnityEngine;

namespace View.Characters
{
    public interface IEnemyCharacter
    {
        void MoveToPoint(Vector3 toPoint);
        List<Vector3> GetPoints();
        bool IsEnemyArrived(Vector3 concurrentPoint);
        void SubscribeOnPlayerEnterTrigger(EnemyDefaultCharacter.OnPlayerExitTrigger action);
        void UnsubscribeOnPlayerEnterTrigger(EnemyDefaultCharacter.OnPlayerExitTrigger action);
        void CleanOnPlayerEnterTrigger();
        void SubscribeOnPlayerExitTrigger(EnemyDefaultCharacter.OnPlayerExitTrigger action);
        void UnsubscribeOnPlayerExitTrigger(EnemyDefaultCharacter.OnPlayerExitTrigger action);
        void CleanOnPlayerExitTrigger();
    }
}