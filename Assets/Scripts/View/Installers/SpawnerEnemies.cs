using System;
using System.Collections;
using FactoryCharacterFiles;
using InputSystemCustom;
using UnityEngine;

namespace View.Installers
{
    public class SpawnerEnemies : MonoBehaviour
    {
        [SerializeField] private CharactersConfiguration charactersConfiguration;
        [SerializeField] private string idCharacter;
        private CharactersFactory _charactersFactory;

        private void Start()
        {
            _charactersFactory = new CharactersFactory(Instantiate(charactersConfiguration));
            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(5f);
            _charactersFactory.Create(idCharacter).WithInput(TypeOfInputs.EnemyIa).Build();
        }
    }
}
