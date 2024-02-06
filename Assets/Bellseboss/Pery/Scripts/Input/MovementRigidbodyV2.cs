using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class MovementRigidbodyV2 : MonoBehaviour
    {
        [SerializeField] private float force;
        [SerializeField] private FloorController floorController;
        [SerializeField] private bool isFall, isUp;
        [SerializeField] private JumpSystem jumpSystem;
        private Rigidbody _rigidbody;
        private float _speed, _lowSpeed;
        private InputMovementCustomV2 _inputMovementCustom;
        private Vector2 _lastDirection;
        private bool _isConfigured;
        [SerializeField] private bool _canMove;
        private GameObject _camera;
        private bool _isTarget;
        private IMovementRigidBodyV2 _movementRigidBodyV2;
        private bool _jump;

        public void Configure(Rigidbody rigidBody, float speed, GameObject camera, IMovementRigidBodyV2 movementRigidBodyV2)
        {
            _rigidbody = rigidBody;
            _speed = speed;
            _inputMovementCustom = new InputMovementCustomV2(force);
            _isConfigured = true;
            _camera = camera;
            _movementRigidBodyV2 = movementRigidBodyV2;
            _canMove = true;
            floorController.Configure(this);
            jumpSystem.Configure(rigidBody, movementRigidBodyV2, floorController);
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
            if (_jump)
            {
                if (floorController.IsTouchingFloor())
                {
                    Debug.Log("MovementRigidbodyV2: Jump");
                    jumpSystem.Jump();
                    _jump = false;   
                }
            }
            else
            {
                _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (1.5f - 1) * Time.deltaTime);
            }
            var resultMovement = _inputMovementCustom.CalculateMovement(result, _speed, _camera, _rigidbody);
            _rigidbody.velocity = new Vector3(resultMovement.x, _rigidbody.velocity.y, resultMovement.z);
            if(_rigidbody.velocity.y > 0)
            {
                isUp = true;
                isFall = false;
            }
            else if(_rigidbody.velocity.y < 0 && !floorController.IsTouchingFloor())
            {
                isFall = true;
                isUp = false;
            }
            else
            {
                isFall = false;
                isUp = false;
            }
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

        public void Jump()
        {
            _jump = true;
        }
    }
}