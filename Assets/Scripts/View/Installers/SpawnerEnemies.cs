using System;
using System.Collections;
using System.Collections.Generic;
using FactoryCharacterFiles;
using InputSystemCustom;
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
        [SerializeField] private AreaZoneController yellowZone, greenZone;
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
            characterEnemy.SetBehavior(yellowZone, greenZone);
        }
    }
}
