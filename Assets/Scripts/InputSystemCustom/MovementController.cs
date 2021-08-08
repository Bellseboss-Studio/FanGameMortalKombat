using CharacterCustom;
using UnityEngine;

namespace InputSystemCustom
{
    public class MovementController : InputCustom
    {
        private readonly Character _character;
        private Vector2 inputToMovement;
        private Vector2 lastPosition;
        private Transform cameraTransform;

        public MovementController(Character character, GameObject camera)
        {
            _character = character;
            character.OnInputChangedExtend += OnInputChangedExtend;
            character.OnCameraMovementExtend += OnCameraMovementExtend;
            character.OnLeftShitOn += OnLeftShitOn;
            character.OnLeftShitOff += OnLeftShitOff;
            cameraTransform = camera.transform;
        }

        public override void ConfigureInputWithCharacter()
        {
            base.ConfigureInputWithCharacter();
            _character.SetCameraForward(cameraTransform);
        }

        private void OnLeftShitOff()
        {
            Debug.Log("Off");
            _character.NormalMotionInCamera();
        }

        private void OnLeftShitOn()
        {
            Debug.Log("On");
            _character.SlowMotionInCamera();
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
            
            Vector3 targetDir = _character.GetPointToCamera().position - cameraTransform.position;
            var forward = cameraTransform.forward;
            var angleBetween = Vector3.Angle(forward, targetDir);
            var anglr = Vector3.Cross(forward, targetDir);
            if (anglr.y < 0)
            {
                angleBetween *= -1;
            }
            Rotating(angleBetween);

            var transformForward = _character.GetTransform().TransformDirection(new Vector3(input.x,0,input.y));

            _character.Move(transformForward);
            _character.SetCameraForward(cameraTransform);
        }

        private void Rotating (float angle)
        {
            var targetRotation = Quaternion.Euler(0, angle, 0);;
            _character.GetTransform().rotation = targetRotation;
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