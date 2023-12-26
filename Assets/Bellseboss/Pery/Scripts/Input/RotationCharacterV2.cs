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

        public void Configure(GameObject camera, GameObject player)
        {
            _camera = camera;
            _player = player;
            _isConfigured = true;
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
            Rotate();   
        }

        public void Direction(Vector2 vector2)
        {
            _direction = vector2;
        }
    }
}