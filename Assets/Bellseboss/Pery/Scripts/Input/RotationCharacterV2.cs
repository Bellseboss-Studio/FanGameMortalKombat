using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class RotationCharacterV2 : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _camera;
        private bool _isConfigured;
        private Vector2 _direction;
        private IRotationCharacterV2 _rotationCharacterV2;

        public void Configure(GameObject camera, GameObject player, IRotationCharacterV2 rotationCharacterV2)
        {
            _camera = camera;
            _player = player;
            _isConfigured = true;
            _rotationCharacterV2 = rotationCharacterV2;
        }

        private void Rotate()
        {
            var direction = _player.transform.position - _camera.transform.position;
            direction.y = 0;
            var rotation = Quaternion.LookRotation(direction);
            _player.transform.rotation = rotation;
        }

        private void Update()
        {
            if (!_isConfigured) return;
            //Rotate();   
        }

        public void Direction(Vector2 vector2)
        {
            _direction = vector2;
            RotatingCharacter();
        }
        
        protected void RotatingCharacter()
        {
            var targetDir = transform.position - _camera.transform.position;
            var forward = _camera.transform.forward;
            var angleBetween = Vector3.Angle(forward, targetDir);
            var anglr = Vector3.Cross(forward, targetDir);
            if (anglr.y < 0)
            {
                angleBetween *= -1;
            }
            Rotating(angleBetween);
        }
        
        protected void Rotating (float angle)
        {
            var targetRotation = Quaternion.Euler(0, angle, 0);;
            transform.rotation = targetRotation;
        }
    }
}