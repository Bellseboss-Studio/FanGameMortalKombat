using System;
using UnityEngine;
using View.Characters;

namespace View.Zone
{
    public class AreaZoneController : MonoBehaviour
    {
        [SerializeField] private float radius;
        public event EnemyDefaultCharacter.OnPlayerTrigger OnPlayerEnter;
        public event EnemyDefaultCharacter.OnPlayerTrigger OnPlayerExit;
        private GameObject target;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                target = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                target = null;
            }
        }

        public bool IsPlayerInThisZone()
        {
            return target != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(90,90,90,0.5f);
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}
