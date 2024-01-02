using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public interface IMovementRigidBodyV2 : ICharacterV2
    {
    }
    
    public interface IAnimationController : ICharacterV2
    {
    }
    
    public interface IRotationCharacterV2 : ICharacterV2
    {
    }
    public interface ICombatSystem : ICharacterV2
    {
        void PowerAttack(float runningDistance, Vector3 runningDirection);
        void QuickAttack(float runningDistance, Vector3 runningDirection);
        void CanMove();
    }
}