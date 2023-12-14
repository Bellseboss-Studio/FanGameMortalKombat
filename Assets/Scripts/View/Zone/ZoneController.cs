using System;
using System.Collections.Generic;
using ServiceLocatorPath;
using Unity.Mathematics;
using UnityEngine;
using View.Characters;

namespace View.Zone
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField] private string nameOfZone;
        [SerializeField] private AreaZoneController yellowZone, greenZone;
        [Range(0,5)] [SerializeField] private float approachDistance = .1f;
        private Dictionary<GameObject, float> _enemiesInCombat;
        private List<GameObject> _enemies;
        public string NameZone => nameOfZone;

        private void Start()
        {
            _enemiesInCombat = new Dictionary<GameObject, float>();
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

        public Vector3 GetPointAccordingPlayer(GameObject target, GameObject x3)
        {
            var x1 = GetAnglesBetweenPlayerPForwardAndEnemy(target, x3, out var angle);
            var xf = GetEnemyAssignedCoords(angle, x1, out var zf);
            return new Vector3((float) xf, 0, (float) zf);
        }

        private double GetEnemyAssignedCoords(double angle, Vector3 x1, out double zf)
        {
            var xf = 0 - Math.Sin(angle * math.PI / 180) * approachDistance + x1.x;
            zf = 0 + Math.Cos(angle * math.PI / 180) * approachDistance + x1.z;
            return xf;
        }

        private Vector3 GetAnglesBetweenPlayerPForwardAndEnemy(GameObject target, GameObject enemy, out double angle)
        {
            
            if (!_enemiesInCombat.ContainsKey(enemy)) _enemiesInCombat.Add(enemy, _enemiesInCombat.Count);
            
            var x1 = target.transform.position;
            var x3 = enemy.transform.position;
            var enemyPos = _enemiesInCombat[enemy];
            
            if (_enemiesInCombat.Count <=1)
            {
                angle = (1.570796326795 - Math.Atan2(x3.z - x1.z, x3.x - x1.x)) * 180 / math.PI;
            }
            else
            {
                if (_enemiesInCombat.Count == 2)
                {
                    angle = _enemiesInCombat[enemy] == 0 ? 0 : 180;
                }
                else
                {
                    angle = (360 / _enemiesInCombat.Count) * enemyPos;
                    if (angle < 0) angle += 360;
                }
            }
            return x1;
        }
    }
}