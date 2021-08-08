using System.Diagnostics;
using CharacterCustom;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace View.Characters
{
    public class PlayerCharacter : Character
    {
        [SerializeField] private PlayerInput playerInput;
        protected override void ConfigureExplicit()
        {
            _inputCustom.playerInput = playerInput;
        }
        private void OnMovementControllers(InputValue value)
        {
            OnInputChangedExtend(value.Get<Vector2>());
        }

        private void OnCameraMovement(InputValue value)
        {
            var vector2 = value.Get<Vector2>();
            OnCameraMovementExtend(vector2);
        }

        private bool _isOn = true;
        private void OnButtonsToAction(InputValue value)
        {
            if (_isOn)
            {
                IsOnPress();
            }
            else
            {
                IsOffPress();
            }
            Debug.Log($"is on {_isOn}");
        }

        private void IsOnPress()
        {
            OnLeftShitOn?.Invoke();
            Debug.Log($"Press On");
            _isOn = false;
        }

        private void IsOffPress()
        {
            OnLeftShitOff?.Invoke();
            Debug.Log($"Press Off");
            _isOn = true;
        }
    }
}