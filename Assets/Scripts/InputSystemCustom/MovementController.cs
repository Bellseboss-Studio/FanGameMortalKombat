using CharacterCustom;
using UnityEngine;

namespace InputSystemCustom
{
    public class MovementController : InputCustom
    {
        private readonly Character _character;
        private Vector2 inputToMovement;
        private Vector2 lastPosition;

        public MovementController(Character character)
        {
            _character = character;
            character.OnInputChangedExtend += OnInputChangedExtend;
            character.OnCameraMovementExtend += OnCameraMovementExtend;
        }

        private void OnCameraMovementExtend(Vector2 input)
        {
            _character.CameraMovement(input);
        }

        private void OnInputChangedExtend(Vector2 input)
        {
            inputToMovement = input;
            TransformDirectionalForForce(input);
            if (inputToMovement.sqrMagnitude > 0.1f)
            {
                lastPosition = inputToMovement;
            }
        }

        private void TransformDirectionalForForce(Vector2 input)
        {
            _character.Move(input.y,input.x);
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