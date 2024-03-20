using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryMonoV2 : MonoBehaviour
{
    public Action AllEnemiesAreDead;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private string idToCreate;
    [SerializeField] private GameObject positionToCreate;
    private List<EnemyV2> _enemies = new List<EnemyV2>();
    private bool allEnemiesAreDead;
    
    public bool IsAllEnemiesAreDead => allEnemiesAreDead;
    
    public void Configure(EnemiesV2Factory factory)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            var e = factory.Create(idToCreate);
            e.transform.position = positionToCreate.transform.position;
            e.transform.rotation = positionToCreate.transform.rotation;
            e.OnDead += OnEnemyDead;
            _enemies.Add(e);
        }
    }

    private void OnEnemyDead(EnemyV2 enemy)
    {
        var allDead = _enemies.TrueForAll(enemy => enemy.IsDead);
        _enemies.Remove(enemy);
        if (allDead)
        {
            allEnemiesAreDead = true;
            AllEnemiesAreDead?.Invoke();
        }
    }
}
