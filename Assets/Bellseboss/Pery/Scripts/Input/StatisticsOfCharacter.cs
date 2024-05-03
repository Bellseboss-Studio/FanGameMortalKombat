using UnityEngine;

[CreateAssetMenu(menuName = "Bellseboss/CharacterStatistics", fileName = "StatisticsOfCharacter", order = 0)]
public class StatisticsOfCharacter : ScriptableObject
{
    public int life;
    public int damage;
    public float speedToMoveAngry;
    public float speedToMoveNormal;
    public float speedToMoveScared;
    public float timeToAttack;
    public float timeBetweenAttacks;
    public string attackAnimationName;
    public AttackMovementSystem.TypeOfAttack attackAnimationType;
    public float timeToActivateCollider;
    public float timeToEnableCollider;
}