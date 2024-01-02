using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class InputMovementCustomV2 : InputCustomV2
    {
        public Vector3 CalculateMovement(Vector2 vector2, float speed, GameObject camera, GameObject player)
        {
            var direction = player.transform.position - camera.transform.position;
            direction.y = 0;
            direction.Normalize();
            var right = new Vector3(direction.z, 0, -direction.x);
            var result = vector2.x * right + vector2.y * direction;
            result.Normalize();
            return result * speed;
        }
    }
}