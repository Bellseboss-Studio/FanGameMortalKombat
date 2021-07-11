using CharacterCustom;
using UnityEngine;
using UnityEngine.InputSystem;

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
    }
}