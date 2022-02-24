using System;
using System.Collections.Generic;
using ServiceLocatorPath;
using UnityEngine;
using View.Characters;

namespace View.Zone
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField] private string nameOfZone;
        [SerializeField] private AreaZoneController yellowZone, greenZone;
        private List<GameObject> _enemies;
        public string NameZone => nameOfZone;

        private void Start()
        {
            _enemies = new List<GameObject>();
            ServiceLocator.Instance.GetService<IGodObserver>().Observe(nameOfZone, Zones.GREEN, greenZone);
            ServiceLocator.Instance.GetService<IGodObserver>().Observe(nameOfZone, Zones.YELLOW, yellowZone);
        }

        private void OnDisable()
        {
            ServiceLocator.Instance.GetService<IGodObserver>().UnObserve();
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.GetService<IGodObserver>().UnObserve();
        }

        public void AddEnemyToList(GameObject characterEnemy)
        {
            if (greenZone.IsPlayerInThisZone())
            {
                greenZone.AddEnemyToPlayerList(characterEnemy);
            }
            _enemies.Add(characterEnemy);
        }

        public void AddEnemiesToList(List<GameObject> characterEnemy)
        {
            foreach (var enemy in characterEnemy)
            {
                _enemies.Add(enemy);
            }
        }
        
        public void AddEnemiesToPlayer(PlayerCharacter playerCharacter)
        {
            playerCharacter.AddEnemies(_enemies);
        }
        
        public void RemoveEnemiesToPlayer(PlayerCharacter playerCharacter)
        {
            playerCharacter.RemoveEnemies(_enemies);
        }

        public void RemoveEnemyToPlayerList(GameObject gameObjectt)
        {
            if (_enemies.Contains(gameObjectt)) _enemies.Remove(gameObjectt);
            greenZone.RemoveEnemyToPlayerList(gameObjectt);
        }
    }
}