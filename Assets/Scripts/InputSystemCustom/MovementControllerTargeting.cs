using UnityEngine;
using View;
using View.Characters;

namespace InputSystemCustom
{
    public class MovementControllerTargeting : InputCustom
    {
        public MovementControllerTargeting(Character character, GameObject camera)
        {
            _playerCharacter = (PlayerCharacter)character;
            _character = character;
            character.OnInputChangedExtend += OnInputChangedExtend;
            character.OnCameraMovementExtend += OnCameraMovementExtend;
            character.OnLeftShitOn += OnLeftShitOn;
            character.OnLeftShitOff += OnLeftShitOff;
            mainCameraTransform = camera.transform;
        }
        
        public override void ChangeInputCustom()
        {
            Debug.Log("desuscribe");
            _playerCharacter.OnInputChangedExtend -= OnInputChangedExtend;
            _playerCharacter.OnCameraMovementExtend -= OnCameraMovementExtend;
            _playerCharacter.OnLeftShitOn -= OnLeftShitOn;
            _playerCharacter.OnLeftShitOff -= OnLeftShitOff;
        }

        public override void ConfigureInputWithCharacter()
        {
            base.ConfigureInputWithCharacter();
            _character.SetCameraForward(mainCameraTransform);
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

        public override void TransformDirectionalForForce(Vector2 input)
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
            //var transformForward = _character.GetDirectionWithObjective();
            //transformForward = new Vector3(input.x, 0, input.y);
            if (Mathf.Abs(transformForward.sqrMagnitude) > 0)
            {
                RotatingCharacter();
                RotatingCharacterObject3D(input);
            }
            return transformForward;
        }

        private void RotatingCharacterObject3D(Vector2 input)
        {
            if (!_playerCharacter.CanRotate()) return;
            var eulerAnglesY = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + _playerCharacter.GetTransform().eulerAngles.y;
            float reference = 0;
            var smoothDampAngle = Mathf.SmoothDampAngle(_playerCharacter.GetTransformProtagonist().eulerAngles.y, eulerAnglesY, ref reference,_character.GetSmoothTimeRotation());
            _playerCharacter.GetTransformProtagonist().rotation = Quaternion.Euler(0f, smoothDampAngle, 0);
        }

        protected override void RotatingCharacter()
        {
            
        }
        protected void RotatingCharacterNew()
        {
            var targetDir = _playerCharacter.GetPointToCamera().position - mainCameraTransform.position;
            var eulerAnglesY = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg + _character.GetTransform().eulerAngles.y;
            float reference = 0;
            var smoothDampAngle = Mathf.SmoothDampAngle(_character.GetTransform().eulerAngles.y, eulerAnglesY, ref reference,_character.GetSmoothTimeRotation());
            _character.GetTransform().rotation = Quaternion.Euler(0f, smoothDampAngle, 0);
            Debug.Log(smoothDampAngle);
        }
    }
}