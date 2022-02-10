using UnityEngine;
using View;
using View.Characters;

namespace InputSystemCustom
{
    public class MovementEnemies : InputCustom
    {
        private readonly EnemyDefaultCharacter _enemyCharacter;

        public MovementEnemies(Character character)
        {
            _character = character;
            _enemyCharacter = (EnemyDefaultCharacter) character;
        }

        public override Vector2 GetDirection()
        {
            return Vector2.up;
        }

        public override bool IsFireActionPressed()
        {
            throw new System.NotImplementedException();
        }

        public override Vector2 GetLasPosition()
        {
            return Vector2.up;
        }
        
        public override Vector3 InputCalculateForTheMovement(Vector2 input)
        {
            var diff = _enemyCharacter.GetToPoint() - _character.GetTransform().position;
            var transformForward = _character.GetTransform().TransformDirection(new Vector3(input.x, 0, input.y));
            if (Mathf.Abs(transformForward.sqrMagnitude) > 0)
            {
                //RotatingCharacter();
            }
            return diff.normalized;
        }

        public override void ChangeInputCustom()
        {
        }

        protected override void RotatingCharacter()
        {
            var targetDir = Vector3.forward;
            var forward = _character.gameObject.transform.forward;
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