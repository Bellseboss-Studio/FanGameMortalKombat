using UnityEngine;
using UnityEngine.InputSystem;
using View;

namespace InputSystemCustom
{
    public abstract class InputCustom
    {
        public PlayerInput playerInput;
        protected Character _character;
        protected Transform cameraTransform;
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

        protected abstract void RotatingCharacter();
    }
}