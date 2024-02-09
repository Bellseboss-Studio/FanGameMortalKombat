using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class RotationCharacterV2 : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _camera;
        private bool _isConfigured;
        private IRotationCharacterV2 _rotationCharacterV2;
        private Vector2 _vector2;
        private float _forceRotation;
        [SerializeField] private bool _canRotate;
        private Vector3 _lastDirection;

        public void Configure(GameObject camera, GameObject player, IRotationCharacterV2 rotationCharacterV2,
            float forceRotation)
        {
            _camera = camera;
            _player = player;
            _isConfigured = true;
            _rotationCharacterV2 = rotationCharacterV2;
            _forceRotation = forceRotation;
            _canRotate = true;
        }

        public void Direction(Vector2 vector2)
        {
            _vector2 = vector2;
        }

        private void Update()
        {
            if (!_isConfigured || !_canRotate) return;
            var direction = _player.transform.position - _camera.transform.position;
            direction.y = 0;
            direction.Normalize();
            var right = new Vector3(direction.z, 0, -direction.x);
            var result = _vector2.x * right + _vector2.y * direction;
            result.Normalize();
            if (result != Vector3.zero)
            {
                _lastDirection = result;
                _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, Quaternion.LookRotation(result), _forceRotation * Time.deltaTime);
            }
            else
            {
                _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, Quaternion.LookRotation(_lastDirection), _forceRotation * Time.deltaTime);
            }
        }

        public void CanRotate(bool canRotate)
        {
            _canRotate = canRotate;
        }

        public bool CanRotate()
        {
            return _canRotate;
        }

        public void RotateToDirectionToMove(Vector2 runningDirection)
        {
            _vector2 = runningDirection;
        }
    }
}