using System;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class EnemyV2 : MonoBehaviour, IAnimationController, IEnemyV2
{
    public Action<EnemyV2> OnDead;
    [SerializeField] private string id;
    [SerializeField] private StatisticsOfCharacter statisticsOfCharacter;
    [SerializeField] private MovementADSR movementADSR;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private GameObject model;
    [SerializeField] private AiController aiController;
    private GameObject _model;
    private StatisticsOfCharacter _statisticsOfCharacter;
    
    public string Id => id;
    public bool IsDead { get; private set; }

    private void Start()
    {
        _statisticsOfCharacter = Instantiate(statisticsOfCharacter);
        movementADSR.Configure(GetComponent<Rigidbody>(), _statisticsOfCharacter);
        _model = Instantiate(model, transform);
        animationController.Configure(_model.GetComponent<Animator>(), this);
        aiController.Configure(this);
    }

    public void ReceiveDamage(int damage, Vector3 direction)
    {
        _statisticsOfCharacter.life -= damage;
        if (_statisticsOfCharacter.life <= 0)
        {
            Die();
        }
        Debug.Log($"Stats: {_statisticsOfCharacter.life} {_statisticsOfCharacter.damage} {_statisticsOfCharacter.speedToMove}");
        if (movementADSR.CanAttackAgain())
        {
            movementADSR.Attack(direction);
        }
    }

    private void Die()
    {
        Debug.Log("EnemyV2: Die");
        IsDead = true;
        OnDead?.Invoke(this);
    }

    public void SetAnimationToHit(bool isQuickAttack, int numberOfCombos)
    {
        Debug.Log($"EnemyV2: SetAnimationToHit isQuickAttack: {isQuickAttack} numberOfCombos: {numberOfCombos}");
    }
}

public interface IEnemyV2
{
}

public class EnemiesV2Factory
{
    private readonly EnemiesV2Configuration EnemiesConfiguration;

    public EnemiesV2Factory(EnemiesV2Configuration enemiesConfiguration)
    {
        EnemiesConfiguration = Object.Instantiate(enemiesConfiguration);
    }
        
    public EnemyV2 Create(string id)
    {
        var prefab = EnemiesConfiguration.GetEnemyV2PrefabById(id);

        return Object.Instantiate(prefab);
    }
}