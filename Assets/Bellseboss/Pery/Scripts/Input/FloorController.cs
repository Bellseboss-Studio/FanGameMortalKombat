using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class FloorController: MonoBehaviour
    {
        [SerializeField] private float minDistanceToFloor;
        [SerializeField] private bool _isTouchingFloor;
        [SerializeField] private LayerMask layerMask;
        private GameObject _player;
        private bool _isConfigured;
        private void FixedUpdate()
        {
            if (!_isConfigured) return;
            if (Physics.Raycast(_player.transform.position + Vector3.up, Vector3.down, out var hit, 10, layerMask))
            {
                _isTouchingFloor = hit.distance <= minDistanceToFloor;
            }
            else
            {
                _isTouchingFloor = false;
            }
        }

        public bool IsTouchingFloor()
        {
            return _isTouchingFloor;
        }

        public void Configure(MovementRigidbodyV2 movementRigidBodyV2)
        {
            _player = movementRigidBodyV2.gameObject;
            _isConfigured = true;
        }

        private void OnDrawGizmos()
        {
            if(!_isConfigured) return;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_player.transform.position + Vector3.up, Vector3.down * 10);
        }
    }
}