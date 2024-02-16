using UnityEngine;

public class EnemyV2 : MonoBehaviour
{
    [SerializeField] private StatisticsOfCharacter statisticsOfCharacter;
    [SerializeField] private MovementADSR movementADSR;
    private StatisticsOfCharacter _statisticsOfCharacter;

    private void Start()
    {
        _statisticsOfCharacter = Instantiate(statisticsOfCharacter);
        movementADSR.Configure(GetComponent<Rigidbody>(), _statisticsOfCharacter);
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
