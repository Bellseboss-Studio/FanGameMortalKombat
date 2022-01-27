using InputSystemCustom;
using UnityEngine;
using View;

namespace Builder
{
    public class MovementFromZeldaBreadOdTheWild
    {
        private readonly Character _character;
        public MovementFromZeldaBreadOdTheWild(Character characterInstantiate)
        {
            _character = characterInstantiate;
            characterInstantiate.OnInputChangedExtend += OnInputChangedExtend;
            characterInstantiate.OnCameraMovementExtend += OnCameraMovementExtend;
            characterInstantiate.OnLeftShitOn += OnLeftShitOn;
            characterInstantiate.OnLeftShitOff += OnLeftShitOff;
        }

        private void OnLeftShitOff()
        {
            Debug.Log("Off");
            //_character.NormalMotionInCamera();
        }

        private void OnLeftShitOn()
        {
            Debug.Log("On");
            //_character.SlowMotionInCamera();
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
    }
}