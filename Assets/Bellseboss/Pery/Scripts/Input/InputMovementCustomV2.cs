using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class InputMovementCustomV2 : InputCustomV2
    {
        private readonly float _force;

        public InputMovementCustomV2(float force)
        {
            _force = force;
        }

        public Vector3 CalculateMovement(Vector2 vector2, float speed, GameObject camera, Rigidbody player)
        {
            var direction = player.gameObject.transform.position - camera.transform.position;
            direction.y = 0;
            direction.Normalize();
            var right = new Vector3(direction.z, 0, -direction.x);
            var result = vector2.x * right + vector2.y * direction;
            //result.y = player.velocity.y;
            result.Normalize();
            return result * speed;
        }
    }
}