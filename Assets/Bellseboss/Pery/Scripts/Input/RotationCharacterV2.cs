﻿using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class RotationCharacterV2 : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _camera;
        private bool _isConfigured;
        private IRotationCharacterV2 _rotationCharacterV2;
        [SerializeField] private Vector2 _vector2;
        private float _forceRotation;
        [SerializeField] private bool _canRotate;
        private bool _canRotateWhileAttack = true;
        private Vector3 _lastDirection;
        private bool _canChangeDirection = true;

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
            if (!_canChangeDirection) return;
            if (result != Vector3.zero && _canRotateWhileAttack)
            {
                _lastDirection = result;
                _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, Quaternion.LookRotation(result), _forceRotation * Time.deltaTime);
            }
            else if (_lastDirection != Vector3.zero)
            {
                _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation,
                    Quaternion.LookRotation(_lastDirection), _forceRotation * Time.deltaTime);
            }

        }

        public void CanRotate(bool canRotate)
        {
            //Debug.Log("Can Rotate: " + canRotate);
            _vector2 = Vector2.zero;
            _canRotateWhileAttack = canRotate;
            _canRotate = canRotate;
        }

        public bool CanRotate()
        {
            return _canRotateWhileAttack;
        }

        public void RotateToLookTheTarget(Vector3 getTarget)
        {
            if(getTarget != Vector3.zero)
            {
                _lastDirection = getTarget - _player.transform.position;
                _lastDirection.y = 0;
            }
        }

        public void ChangeDirection(Vector3 rotation)
        {
            Debug.Log("Change Direction: " + rotation);
            _canChangeDirection = false;
            _vector2 = rotation;
        }

        public void RestoreRotation()
        {
            _canChangeDirection = true;
        }
    }
}