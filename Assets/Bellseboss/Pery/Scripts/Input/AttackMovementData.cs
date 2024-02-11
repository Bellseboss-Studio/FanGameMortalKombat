using UnityEngine;

[CreateAssetMenu(menuName = "Bellseboss/AttackMovementData", fileName = "AttackMovementData", order = 0)]
public class AttackMovementData : ScriptableObject
{
    public float timeToAttack, timeToDecreasing, timeToSustain, timeToRelease;
    public float maxDistance;
    public float distanceToDecresing;
    public float forceToAttack, forceToDecreasing;
}