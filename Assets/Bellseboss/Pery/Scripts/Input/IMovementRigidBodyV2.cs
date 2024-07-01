using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public interface IMovementRigidBodyV2
    {
        void UpdateAnimation();
        void UpdateAnimation(bool isTouchingFloor, bool isTouchingWall);
        void ChangeToNormalJump();
        void ChangeRotation(Vector3 rotation);
        void RestoreRotation();
        void EndAttackMovement();
        void PlayerFall();
        void PlayerRecovery();
        bool IsAttacking();
        void PlayerFallV2();
        void PlayerRecoveryV2();
        bool IsJumpingInWall();
    }

    public interface IAnimationController
    {
    }

    public interface IRotationCharacterV2
    {
    }

    public interface ICombatSystem : ICharacterV2
    {
    }

    public interface IFocusTarget
    {
    }
}