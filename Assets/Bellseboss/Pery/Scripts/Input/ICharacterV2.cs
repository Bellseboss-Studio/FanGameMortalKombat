using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public interface ICharacterV2
    {
        Action OnAction { get; set; }
        void DisableControls();
        void ActivateAnimationTrigger(string animationTrigger);
        void SetPositionAndRotation(GameObject refOfPlayer);
        void CanMove();
        Transform GetGameObject();
    }
}