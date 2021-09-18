using CharacterCustom;
using UnityEngine;
using View.Characters;

namespace InputSystemCustom
{
    public class MovementController : InputCustom
    {
        private Vector2 inputToMovement;
        private Vector2 lastPosition;
        private PlayerCharacter _playerCharacter;

        public MovementController(Character character, GameObject camera)
        {
            _playerCharacter = (PlayerCharacter)character;
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
            inputToMovement = input;//adelante (0,0,1)
            if (_playerCharacter.CanMove)
            {
                TransformDirectionalForForce(input);
                lastPosition = inputToMovement;
            }
            RotatingCharacter();
            RotatingCharacterObject3D(input);
        }

        public void TransformDirectionalForForce(Vector2 input)
        {
            var transformForward = InputCalculateForTheMovement(input);

            _character.Move(transformForward);
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
        public override Vector3 InputCalculateForTheMovement(Vector2 input)
        {
            var transformForward = _character.GetTransform().TransformDirection(new Vector3(input.x, 0, input.y));
            if (Mathf.Abs(transformForward.sqrMagnitude) > 0)
            {
                RotatingCharacter();
            }
            return transformForward;
        }

        private void RotatingCharacterObject3D(Vector2 input)
        {
            /*var targetDir = new Vector3(input.x, 0, input.y);
            var forward = _playerCharacter.GetTransform().forward;
            //var forward = _playerCharacter.GetTransformProtagonist().forward;
            var angleBetween = Vector3.Angle(forward, targetDir);
            var anglr = Vector3.Cross(forward, targetDir);
            if (anglr.y < 0)
            {
                //angleBetween *= -1;
            }
            */
            var eulerAnglesY = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + _playerCharacter.GetTransform().eulerAngles.y;
            float reference = 0;
            var smoothDampAngle = Mathf.SmoothDampAngle(_playerCharacter.GetTransformProtagonist().eulerAngles.y, eulerAnglesY, ref reference,_character.GetSmoothTimeRotation());
            _playerCharacter.GetTransformProtagonist().rotation = Quaternion.Euler(0f, smoothDampAngle, 0);
            //Debug.Log(angleBetween);
            //RotatingObject3D(angleBetween);
        }

        protected override void RotatingCharacter()
        {
            var targetDir = _playerCharacter.GetPointToCamera().position - cameraTransform.position;
            var forward = cameraTransform.forward;
            //var forward = _playerCharacter.GetTransformProtagonist().forward;
            var angleBetween = Vector3.Angle(forward, targetDir);
            var anglr = Vector3.Cross(forward, targetDir);
            if (anglr.y < 0)
            {
                angleBetween *= -1;
            }

            Rotating(angleBetween);
        }
    }
}