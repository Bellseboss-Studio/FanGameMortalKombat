using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class FloorController: MonoBehaviour
    {
        public Action OnFall, OnRecovery;
        public Action<bool> OnTouchingFloorChanged;
        [SerializeField] private float minDistanceToFloor;
        [SerializeField] private bool _isTouchingFloor;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private List<Transform> raycastsOrigins;
        private GameObject _player;
        private bool _isConfigured;
        
        private void FixedUpdate()
        {
            if (!_isConfigured) return;
            var touching = false;
            foreach (var origin in raycastsOrigins)
            {
                if (Physics.Raycast(origin.position + Vector3.up, Vector3.down, out var hit, 10, layerMask))
                {
                    var isTouchingFloor = hit.distance <= minDistanceToFloor;
                    if (isTouchingFloor)
                    {
                        OnRecovery?.Invoke();
                        if (!_isTouchingFloor)
                        {
                            OnTouchingFloorChanged?.Invoke(true);
                            _isTouchingFloor = true;
                        }
                        return;
                    }
                }
            }
            
            if (_isTouchingFloor) OnTouchingFloorChanged?.Invoke(false);
            _isTouchingFloor = false;
        }

        public bool IsTouchingFloor()
        {
            return _isTouchingFloor;
        }

        public void Configure(GameObject movementRigidBodyV2)
        {
            _player = movementRigidBodyV2.gameObject;
            _isConfigured = true;
        }

        private void OnDrawGizmos()
        {
            if(!_isConfigured) return;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_player.transform.position + Vector3.up, Vector3.down * 10);
            
            Gizmos.color = _isTouchingFloor ? Color.green : Color.red;
            Gizmos.DrawSphere(_player.transform.position + Vector3.up, 0.5f);
            Gizmos.DrawLine(_player.transform.position, _player.transform.position + Vector3.up * 2);
        }
    }
}