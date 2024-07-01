using UnityEngine;

namespace Bellseboss.Angel.CombatSystem
{
    [CreateAssetMenu(fileName = "NewStunInfo", menuName = "Angel/StunInfo")]
    public class StunInfo : ScriptableObject
    {
        public string parameterName;
        public float timeToAttack, timeToDecreasing, timeToSustain, timeToRelease;
        public float maxDistance;
        public float distanceToDecreasing;
        public float forceToAttack, forceToDecreasing;
        public float stunTime;
    }
}