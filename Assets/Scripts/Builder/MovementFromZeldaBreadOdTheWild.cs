using CharacterCustom;
using InputSystemCustom;
using UnityEngine;

namespace Builder
{
    public class MovementFromZeldaBreadOdTheWild : InputCustom
    {
        private readonly Character _character;
        public MovementFromZeldaBreadOdTheWild(Character characterInstantiate)
        {
            _character = characterInstantiate;
            characterInstantiate.OnInputChangedExtend += OnInputChangedExtend;
            characterInstantiate.OnCameraMovementExtend += OnCameraMovementExtend;
        }

        private void OnCameraMovementExtend(Vector2 input)
        {
            //nada, ya esta configurado
        }

        private void OnInputChangedExtend(Vector2 input)
        {
            Debug.Log($"{input}");
            _character.Move(input);
            //_character.Rotate()
        }

        public override Vector2 GetDirection()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsFireActionPressed()
        {
            throw new System.NotImplementedException();
        }

        public override Vector2 GetLasPosition()
        {
            throw new System.NotImplementedException();
        }
    }
}