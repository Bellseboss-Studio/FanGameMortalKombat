using UnityEngine;

namespace InputSystemCustom
{
    public class MovementEnemies : InputCustom
    {
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
    }
}