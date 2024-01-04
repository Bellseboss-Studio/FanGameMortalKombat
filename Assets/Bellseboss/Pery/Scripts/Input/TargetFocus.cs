using System;
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

        public void Configure(IFocusTarget focusTarget)
        {
            _focusTarget = focusTarget;
        }
        public Vector3 RotateToTarget(Vector3 originalDirection)
        {
            var directionToTarget = originalDirection;
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;
            //convert to global space to locate the center of the sphere
            var directionToTargetLocall = transform.TransformDirection(directionToTargetLocal);
            RaycastHit[] hits = Physics.SphereCastAll(origin + directionToTargetLocall, coneRadius, direction, maxDistance, layerMask);
            foreach (RaycastHit hit in hits)
            {
                directionToTarget = hit.collider.gameObject.transform.position - transform.position;
                directionToTarget.y = 0;
                directionToTarget.Normalize();
                directionToTarget = transform.InverseTransformDirection(directionToTarget);
                break;
            }
            return directionToTarget;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var directionToTargetLocall = transform.TransformDirection(directionToTargetLocal);
            Gizmos.DrawWireSphere(transform.position + directionToTargetLocall, coneRadius);
        }
    }
}