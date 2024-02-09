using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class TargetFocus : MonoBehaviour
    {
        public float coneRadius = 1.0f;
        public float maxDistance = 10.0f;
        public LayerMask layerMask;
        public Vector3 directionToTargetLocal;
        private IFocusTarget _focusTarget;
        private List<GameObject> _enemies = new List<GameObject>();

        public void Configure(IFocusTarget focusTarget)
        {
            _focusTarget = focusTarget;
        }
        public Vector3 RotateToTarget(Vector3 originalDirection)
        {
            var result = originalDirection;
            if (_enemies.Count > 0)
            {
                var closestEnemy = GetClosestEnemy();
                var directionToTarget = closestEnemy.transform.position - transform.position;
                directionToTargetLocal = transform.InverseTransformDirection(directionToTarget);
                directionToTargetLocal.y = 0;
                directionToTargetLocal.Normalize();
                result = directionToTargetLocal;
                Debug.Log($"TargetFocus: RotateToTarget: directionToTargetLocal: {directionToTargetLocal}");
            }
            return result;
        }

        private GameObject GetClosestEnemy()
        {
            GameObject closestEnemy = null;
            var minDistance = float.MaxValue;
            foreach (var enemy in _enemies)
            {
                var distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                Debug.Log($"TargetFocus: OnTriggerEnter: other: {other.gameObject.name}");
                _enemies.Add(other.gameObject);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            //compare layer
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                Debug.Log($"TargetFocus: OnTriggerExit: other: {other.gameObject.name}");
                _enemies.Remove(other.gameObject);
            }
        }

        public Vector3 GetTarget()
        {
            if(_enemies.Count > 0)
            {
                var closestEnemy = GetClosestEnemy();
                return closestEnemy.transform.position;
            }
            return Vector3.zero;
        }
    }
}