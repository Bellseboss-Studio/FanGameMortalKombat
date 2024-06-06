using System;
using UnityEngine;

public abstract class PJV2 : MonoBehaviour
{
    public Action<float> OnReceiveDamage;
    public abstract void ReceiveDamage(int damage, Vector3 transformForward, float currentAttackStunTime);
    public abstract void SetAnimationToHit(string animationParameterName);
    public abstract void Stun(bool isStun);
}