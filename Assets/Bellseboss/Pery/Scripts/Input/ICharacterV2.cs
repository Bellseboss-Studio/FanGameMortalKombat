using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public interface ICharacterV2
    {
        Action OnAction { get; set; }
        GameObject Model3DInstance { get; }
        void DisableControls();
        void ActivateAnimationTrigger(string animationTrigger);
        void SetPositionAndRotation(GameObject refOfPlayer);
        void EnableControls();
        Transform GetGameObject();
    }
}