using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bellseboss.Pery.Scripts.Input
{
    public class InputPlayerV2 : MonoBehaviour
    {
        public Action<Vector2> onMoveEvent;
        public Action<bool> onTargetEvent;
        public Action onKickEvent;
        public Action onPunchEvent;
        public Action onCameraMovementEvet;
        public Vector3 CurrentVector => _currentInputVector;
        private Vector3 _currentInputVector;
        public void OnMove(InputAction.CallbackContext context)
        {
            _currentInputVector = context.ReadValue<Vector2>();
            onMoveEvent?.Invoke(_currentInputVector);
        }
        
        public void OnCameraMovement(InputAction.CallbackContext context)
        {
            onCameraMovementEvet?.Invoke();
        }
        
        public void OnTarget(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onTargetEvent?.Invoke(true);
            }else if (context.canceled)
            {
                onTargetEvent?.Invoke(false);
            }
        }
        
        public void OnKick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                //Debug.Log("Kick");
                onKickEvent?.Invoke();
            }
        }
        
        public void OnPunch(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                //Debug.Log("Punch");
                onPunchEvent?.Invoke();
            }
        }
    }
}
