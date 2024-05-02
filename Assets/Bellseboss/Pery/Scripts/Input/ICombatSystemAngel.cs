using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public interface ICombatSystemAngel
    {
        /*void PowerAttack(float runningDistance, Vector3 runningDirection);
        void QuickAttack(float runningDistance, Vector3 runningDirection);
        void CanMove();*/
        /*Vector3 RotateToTargetAngel(Vector3 originalDirection);*/
        /*bool CanAttack();
        AttackMovementSystem GetAttackSystem();*/
        public Action<string> GetActionToAnimate();
        void PlayerTouchEnemy();
    }
}