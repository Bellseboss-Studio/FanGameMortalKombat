using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Bellseboss/Enemies/Configuration")]
public class EnemiesV2Configuration : ScriptableObject
{
    [SerializeField] private EnemyV2[] enemiesV2;
    private Dictionary<string, EnemyV2> idToEnemyV2;

    private void Awake()
    {
        idToEnemyV2 = new Dictionary<string, EnemyV2>(enemiesV2.Length);
        foreach (var enemyV2 in enemiesV2)
        {
            idToEnemyV2.Add(enemyV2.Id, enemyV2);
        }
    }

    public EnemyV2 GetEnemyV2PrefabById(string id)
    {
        if (!idToEnemyV2.TryGetValue(id, out var enemyV2))
        {
            throw new Exception($"EnemyV2 with id {id} does not exit");
        }
        return enemyV2;
    }
}