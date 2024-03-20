using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class GetDataWentCollisionWithPlayer : MonoBehaviour
    {
        public Action<GameObject, Vector3> CollisionEnter;
        public Action<GameObject, Vector3> CollisionExit;
        public LayerMask layerMask;
        private Collider _collider;

        public void Configure()
        {
            _collider = GetComponent<Collider>();
            DisableCollider();
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                //Debug.Log($"TargetFocus: OnTriggerEnter: other: {other.gameObject.name}");
                CollisionEnter?.Invoke(other.gameObject, other.ClosestPoint(transform.position));
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            //compare layer
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                //Debug.Log($"TargetFocus: OnTriggerExit: other: {other.gameObject.name}");
                CollisionExit?.Invoke(other.gameObject, other.ClosestPoint(transform.position));
            }
        }

        public void EnableCollider()
        {
            _collider.enabled = true;
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
        }
    }
}