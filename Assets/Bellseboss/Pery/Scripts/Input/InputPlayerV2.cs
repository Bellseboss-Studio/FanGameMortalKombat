using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Bellseboss.Pery.Scripts.Input
{
    public class InputPlayerV2 : MonoBehaviour
    {
        public Action<Vector2> onMoveEvent;
        public Action<bool> onTargetEvent;
        public void OnMove(InputAction.CallbackContext context)
        {
            var _inputVector = context.ReadValue<Vector2>();
            //Debug.Log($"OnMove {_inputVector}");
            onMoveEvent?.Invoke(_inputVector);
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
    }
}
