using System;
using System.Collections.Generic;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

public class EnemyFactoryMonoV2 : MonoBehaviour
{
    public Action AllEnemiesAreDead;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private string idToCreate;
    [SerializeField] private GameObject positionToCreate;
    [SerializeField] private GameObject[] pathToFollow;
    private List<EnemyV2> _enemies = new List<EnemyV2>();
    [SerializeField] private bool allEnemiesAreDead;
    
    public bool IsAllEnemiesAreDead => allEnemiesAreDead;
    
    public void Configure(EnemiesV2Factory factory, GameObject center)
    {
        if(numberOfEnemies == 0)
        {
            allEnemiesAreDead = true;
            AllEnemiesAreDead?.Invoke();
            return;
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            var e = factory.Create(idToCreate);
            e.transform.position = positionToCreate.transform.position;
            e.transform.rotation = positionToCreate.transform.rotation;
            e.Configure(pathToFollow, center);
            e.OnDead += OnEnemyDead;
            _enemies.Add(e);
        }
    }

    private void OnEnemyDead(EnemyV2 enemy)
    {
        var allDead = _enemies.TrueForAll(enemy => enemy.IsDead);
        Debug.Log($"AllEnemiesAreDead {allDead}");
        _enemies.Remove(enemy);
        if (allDead)
        {
            allEnemiesAreDead = true;
            AllEnemiesAreDead?.Invoke();
        }
    }

    public void SetPlayer(CharacterV2 characterV2)
    {
        foreach (var enemyV2 in _enemies)
        {
            enemyV2.SetPlayer(characterV2);
        }
    }

    public void IntoToFarZone(bool b)
    {
        foreach (var enemyV2 in _enemies)
        {
            enemyV2.IntoToFarZone(b);
        }
    }

    public void IntoToNearZone(bool b)
    {
        foreach (var enemyV2 in _enemies)
        {
            enemyV2.IntoToNearZone(b);
        }
    }
}
