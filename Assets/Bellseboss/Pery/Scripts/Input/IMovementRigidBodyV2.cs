using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public interface IMovementRigidBodyV2
    {
        void UpdateAnimation();
        void ChangeToNormalJump();
        void ChangeRotation(Vector3 rotation);
        void RestoreRotation();
        void EndAttackMovement();
        void PlayerFall();
        void PlayerRecovery();
        bool IsAttacking();
        void PlayerFallV2();
        void PlayerRecoveryV2();
    }
    
    public interface IAnimationController
    {
    }
    
    public interface IRotationCharacterV2
    {
    }
    public interface ICombatSystem : ICharacterV2
    {
        //void PowerAttack(float runningDistance, Vector3 runningDirection);
        //void QuickAttack(float runningDistance, Vector3 runningDirection);
        //void CanMove();
        //Vector3 RotateToTarget(Vector3 originalDirection);
        //bool CanAttack();
        //AttackMovementSystem GetAttackSystem();
    }
    
    public interface IFocusTarget
    {
    }
}