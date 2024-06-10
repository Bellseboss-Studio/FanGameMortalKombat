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

        public Vector3 CalculateMovement(Vector2 input, float speed, GameObject camera, GameObject player)
        {
            //Debug.Log($"InputMovementCustomV2: Input: {input}");
            var direction = player.transform.position - camera.transform.position;
            direction.Normalize();
            var right = new Vector3(direction.z, 0, -direction.x);
            var result = input.x * right + input.y * direction;
            result.Normalize();
            return result * speed;
        }
    }
}