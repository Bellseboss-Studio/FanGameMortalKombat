using System;
using System.Collections;
using System.Collections.Generic;
using FactoryCharacterFiles;
using InputSystemCustom;
using ServiceLocatorPath;
using StatesOfEnemies;
using UnityEngine;
using View.Characters;
using View.Zone;

namespace View.Installers
{
    public class SpawnerEnemies : MonoBehaviour
    {
        [SerializeField] private CharactersConfiguration charactersConfiguration;
        [SerializeField] private string idCharacter;
        [SerializeField] private List<GameObject> points;
        [SerializeField] private ZoneController zoneOur;
        private CharactersFactory _charactersFactory;

        private void Start()
        {
            _charactersFactory = new CharactersFactory(Instantiate(charactersConfiguration));
            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(5f);
            var characterEnemy = (EnemyDefaultCharacter) _charactersFactory.Create(idCharacter).WithInput(TypeOfInputs.EnemyIa).InPosition(transform.position).Build();
            characterEnemy.SetPoints(points);
            var yellowZone = ServiceLocator.Instance.GetService<IGodObserver>().GetZone(zoneOur.NameZone, Zones.YELLOW);
            var greenZone = ServiceLocator.Instance.GetService<IGodObserver>().GetZone(zoneOur.NameZone, Zones.GREEN);
            characterEnemy.SetBehavior(yellowZone, greenZone);
        }
    }
}
