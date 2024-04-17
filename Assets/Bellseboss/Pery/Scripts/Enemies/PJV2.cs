using UnityEngine;

public abstract class PJV2 : MonoBehaviour{
    public abstract void ReceiveDamage(int damage, Vector3 transformForward);
    public abstract void SetAnimationToHit(bool isQuickAttack, int numberOfCombosQuick);
    public abstract void Stun(bool isStun);
}