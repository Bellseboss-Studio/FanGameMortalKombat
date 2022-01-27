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
        void SubscribeOnPlayerExitTrigger(EnemyDefaultCharacter.OnPlayerTrigger action);
        bool IsPlayerInYellowZone();
        bool IsPlayerInGreenZone();
        void LookTarget(Vector3 target);
        bool IsPlayerInRangeOfAttack();
        void StopMovement();
        float GetVelocity();
        void Attack(Character characterToAttack, float damage);
    }
}