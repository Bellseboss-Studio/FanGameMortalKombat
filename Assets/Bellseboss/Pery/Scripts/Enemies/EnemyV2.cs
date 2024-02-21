using System;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

public class EnemyV2 : MonoBehaviour, IAnimationController
{
    [SerializeField] private StatisticsOfCharacter statisticsOfCharacter;
    [SerializeField] private MovementADSR movementADSR;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private GameObject model;
    private GameObject _model;
    private StatisticsOfCharacter _statisticsOfCharacter;

    private void Start()
    {
        _statisticsOfCharacter = Instantiate(statisticsOfCharacter);
        movementADSR.Configure(GetComponent<Rigidbody>(), _statisticsOfCharacter);
        _model = Instantiate(model, transform);
        animationController.Configure(_model.GetComponent<Animator>(), this);
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
    }

    public void SetAnimationToHit(bool isQuickAttack, int numberOfCombos)
    {
        Debug.Log($"EnemyV2: SetAnimationToHit isQuickAttack: {isQuickAttack} numberOfCombos: {numberOfCombos}");
    }
}
