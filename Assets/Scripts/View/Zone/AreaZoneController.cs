using System;
using UnityEngine;
using View.Characters;

namespace View.Zone
{
    public class AreaZoneController : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private ZoneController zoneController;
        public event EnemyDefaultCharacter.OnPlayerTrigger OnPlayerEnter;
        public event EnemyDefaultCharacter.OnPlayerTrigger OnPlayerExit;
        private GameObject target;
        private PlayerCharacter _player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter playerCharacter))
            {
                _player = playerCharacter;
                if(radius == 35) zoneController.AddEnemiesToPlayer(playerCharacter);
                target = other.gameObject;
                OnPlayerEnter?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter playerCharacter))
            {
                if(radius == 35) zoneController.RemoveEnemiesToPlayer(playerCharacter);
                target = null;
                OnPlayerExit?.Invoke(other.gameObject);
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

        public void RemoveEnemyToPlayerList(GameObject gameObjectt)
        {
            if (_player == null) return;
            _player.RemoveEnemy(gameObjectt);
        }

        public void AddEnemyToPlayerList(GameObject characterEnemy)
        {
            _player.AddEnemy(characterEnemy);
        }
    }
}
