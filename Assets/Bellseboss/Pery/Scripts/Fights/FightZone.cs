using System;
using System.Collections.Generic;
using System.Linq;
using Bellseboss.Pery.Scripts.Input;
using Unity.Collections;
using UnityEngine;

public class FightZone : MonoBehaviour
{
    [SerializeField] private GetDataWentCollisionWithPlayer far, near;
    [SerializeField] private EnemyFactoryMonoV2[] enemiesMonoV2FactoryMono;
    [SerializeField] private EnemiesV2Configuration enemiesConfiguration;
    [SerializeField] private Activable activable;
    [ReadOnly] private bool allEnemiesAreDead;
    private EnemiesV2Factory _factory;
    
    public bool AllEnemiesAreDead => allEnemiesAreDead;

    private void Start()
    {
        Configure();
    }

    public void Configure()
    {
        far.Configure();
        near.Configure();
        far.EnableCollider();
        near.EnableCollider();
        far.CollisionEnter += FarOnCollisionEnter;
        near.CollisionEnter += NearOnCollisionEnter;
        far.CollisionExit += FarOnCollisionExit;
        near.CollisionExit += NearOnCollisionExit;
        
        _factory = new EnemiesV2Factory(enemiesConfiguration);
        
        foreach (var enemyFactoryMonoV2 in enemiesMonoV2FactoryMono)
        {
            
            enemyFactoryMonoV2.AllEnemiesAreDead += EnemyFactoryMonoV2OnAllEnemiesAreDead;
            enemyFactoryMonoV2.Configure(_factory, near.gameObject);
        }
    }

    private void EnemyFactoryMonoV2OnAllEnemiesAreDead()
    {
        var allDead = enemiesMonoV2FactoryMono.All(enemyFactoryMonoV2 => enemyFactoryMonoV2.IsAllEnemiesAreDead);
        Debug.Log($"AllEnemiesAreDead: {allDead}");
        if (allDead)
        {
            activable.Activate();
        }
    }

    private void OnDestroy()
    {
        far.CollisionEnter -= FarOnCollisionEnter;
        near.CollisionEnter -= NearOnCollisionEnter;
        far.CollisionExit -= FarOnCollisionExit;
        near.CollisionExit -= NearOnCollisionExit;
    }

    private void NearOnCollisionExit(GameObject arg1, Vector3 arg2)
    {
        foreach (var factoryMonoV2 in enemiesMonoV2FactoryMono)
        {
            factoryMonoV2.IntoToNearZone(false);
        }
    }

    private void NearOnCollisionEnter(GameObject arg1, Vector3 arg2)
    {
        foreach (var factoryMonoV2 in enemiesMonoV2FactoryMono)
        {
            factoryMonoV2.IntoToNearZone(true);
        }
    }

    private void FarOnCollisionExit(GameObject arg1, Vector3 arg2)
    {
        foreach (var factoryMonoV2 in enemiesMonoV2FactoryMono)
        {
            factoryMonoV2.SetPlayer(null);
            factoryMonoV2.IntoToFarZone(false);
        }
        arg1.GetComponent<CharacterV2>().GetOutOfEnemyZone();
    }

    private void FarOnCollisionEnter(GameObject arg1, Vector3 arg2)
    {
        var characterV2 = arg1.GetComponent<CharacterV2>();
        var enemies = new List<GameObject>();
        foreach (var factoryMonoV2 in enemiesMonoV2FactoryMono)
        {
            factoryMonoV2.SetPlayer(characterV2);
            factoryMonoV2.IntoToFarZone(true);
            enemies.AddRange(factoryMonoV2.Enemies.Select(enemy => enemy.gameObject));
        }
        characterV2.GetIntoEnemyZone(enemies);
    }
}
