using UnityEngine;
using UnityEngine.InputSystem;
using View;
using View.Characters;

namespace InputSystemCustom
{
    public abstract class InputCustom
    {
        public PlayerInput playerInput;
        protected Character _character;
        protected Transform mainCameraTransform;
        protected Transform grupalCameraTransform;
        protected Vector2 inputToMovement;
        protected Vector2 lastPosition;
        protected PlayerCharacter _playerCharacter;
        public abstract Vector2 GetDirection();
        public abstract bool IsFireActionPressed();
        public abstract Vector2 GetLasPosition();

        public virtual void ConfigureInputWithCharacter()
        {
            
        }

        public abstract Vector3 InputCalculateForTheMovement(Vector2 input);
        
        protected void Rotating (float angle)
        {
            var targetRotation = Quaternion.Euler(0, angle, 0);;
            _character.GetTransform().rotation = targetRotation;
        }
        protected void RotatingObject3D (float angle)
        {
            //var targetRotation = Quaternion.Euler(0, angle, 0);
            _character.GetTransformProtagonist().localEulerAngles =  new Vector3(0, angle, 0);
            //_character.GetTransformProtagonist().eulerAngles = targetRotation;
        }

        public abstract void ChangeInputCustom();
        
        protected abstract void RotatingCharacter();
        
        protected void OnInputChangedExtend(Vector2 input)
        {
            lastPosition = Vector2.zero;
            if (_character.IsInPause) return;
            if (_playerCharacter.CanMove)
            {
                //here take skicks from control
                inputToMovement = input;//adelante (0,0,1)
                lastPosition = inputToMovement;
                TransformDirectionalForForce(input);
            }
            
            else
            {
                lastPosition = Vector2.zero;
            }
        }

        public abstract void TransformDirectionalForForce(Vector2 input);
    }
}