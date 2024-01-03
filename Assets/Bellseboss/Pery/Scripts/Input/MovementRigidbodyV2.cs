﻿using System;
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
        [SerializeField] private bool _canMove;
        private GameObject _camera;
        private bool _isTarget;
        private IMovementRigidBodyV2 _movementRigidBodyV2;

        public void Configure(Rigidbody rigidbody, float speed, GameObject camera, IMovementRigidBodyV2 movementRigidBodyV2)
        {
            _rigidbody = rigidbody;
            _speed = speed;
            _inputMovementCustom = new InputMovementCustomV2();
            _isConfigured = true;
            _camera = camera;
            _movementRigidBodyV2 = movementRigidBodyV2;
            _canMove = true;
        }

        private void Move()
        {
            var result = Vector2.zero;
            if (_isTarget && Mathf.Abs(_lastDirection.y) > 0)
            {
                result.y = _lastDirection.y >= 0 ? 0.49f : -0.49f;
            }else if (Mathf.Abs(_lastDirection.y) > 0 && Mathf.Abs(_lastDirection.y) < 0.5f)
            {
                result.y = _lastDirection.y >= 0 ? 0.49f : -0.49f;
            }else if (Mathf.Abs(_lastDirection.y) >= 0.5f)
            {
                result.y = _lastDirection.y >= 0 ? 1f : -1f;
            }
            result.x = _lastDirection.x;
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
            if (!_isConfigured || !_canMove) return;
            Move();   
        }

        public void IsTarget(bool isTarget)
        {
            _isTarget = isTarget;
        }

        public float GetVelocity()
        {
            return _rigidbody.velocity.magnitude;
        }

        public void AddForce(Vector3 runningDirection, float runningDistance)
        {
            _rigidbody.velocity = Vector3.zero;
            Vector3 globalDirection = transform.TransformDirection(runningDirection.normalized);
            _rigidbody.AddForce(globalDirection * runningDistance, ForceMode.Impulse);
            _canMove = false;
        }

        public void CanMove()
        {
            _canMove = true;
        }

        public Vector3 GetVelocityV3()
        {
            return _rigidbody.velocity;
        }
    }
}