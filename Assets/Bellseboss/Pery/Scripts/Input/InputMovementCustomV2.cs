using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class InputMovementCustomV2 : InputCustomV2
    {
        public Vector3 CalculateMovement(Vector2 vector2, float speed, GameObject camera, GameObject player)
        {
            return player.transform.TransformDirection(new Vector3(vector2.x, 0, vector2.y)) * speed;
        }
    }
}