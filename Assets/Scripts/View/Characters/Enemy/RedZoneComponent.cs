using UnityEngine;

namespace View.Characters.Enemy
{
    public class RedZoneComponent : MonoBehaviour
    {
        public event EnemyDefaultCharacter.OnPlayerTrigger OnPlayerEnter;
        public event EnemyDefaultCharacter.OnPlayerTrigger OnPlayerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnPlayerEnter?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnPlayerExit?.Invoke(other.gameObject);
            }
        }
    }
}
