using CharacterCustom;
using UnityEngine;

namespace InputSystemCustom
{
    public class MovementController : InputCustom
    {
        private Vector2 inputToMovement;
        private Vector2 lastPosition;

        public MovementController(Character character)
        {
            character.OnInputChangedExtend += OnInputChangedExtend;
        }

        private void OnInputChangedExtend(Vector2 input)
        {
            inputToMovement = input;
            if (inputToMovement.sqrMagnitude > 0.1f)
            {
                lastPosition = inputToMovement;
            }
        }

        public override Vector2 GetDirection()
        {
            return inputToMovement;
        }

        public override bool IsFireActionPressed()
        {
            throw new System.NotImplementedException();
        }

        public override Vector2 GetLasPosition()
        {
            return lastPosition;
        }
    }
}