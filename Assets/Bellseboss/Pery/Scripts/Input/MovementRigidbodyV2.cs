using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class MovementRigidbodyV2 : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private float _speed, _lowSpeed;
        private InputMovementCustomV2 _inputMovementCustom;
        private Vector2 _lastDirection;
        private bool _isConfigured;
        private GameObject _camera;
        private bool _isTarget;

        public void Configure(Rigidbody rigidbody, float speed, GameObject camera)
        {
            _rigidbody = rigidbody;
            _speed = speed;
            _inputMovementCustom = new InputMovementCustomV2();
            _isConfigured = true;
            _camera = camera;
        }

        private void Move(Vector2 vector2)
        {
            var result = Vector2.zero;
            if (_isTarget && _lastDirection.y > 0)
            {
                result.y = 0.49f;
            }else if (_lastDirection.y > 0 && _lastDirection.y < 0.5f)
            {
                result.y = 0.49f;
            }else if (_lastDirection.y >= 0.5f)
            {
                result.y = 1;
            }
            result.x = vector2.x;
            _rigidbody.velocity = _inputMovementCustom.CalculateMovement(result, _speed, _camera, _rigidbody.gameObject);
        }

        public void Direction(Vector2 vector2)
        {
            _lastDirection = vector2;
        }

        private void FixedUpdate()
        {
        }

        private void Update()
        {
            if (!_isConfigured) return;
            Move(_lastDirection);   
        }

        public void IsTarget(bool isTarget)
        {
            _isTarget = isTarget;
        }
    }
}