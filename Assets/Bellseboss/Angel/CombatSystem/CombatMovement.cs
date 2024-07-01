using System;
using System.Collections.Generic;

namespace Bellseboss.Angel.CombatSystem
{
    [Serializable]
    public class CombatMovement
    {
        public List<TypeOfAttack> comboSequence;
        public string transitionParameterName;
        public StunInfo stuntInfo;
        public float timeToAttack, timeToDecreasing, timeToSustain, timeToRelease;
        public float maxDistance;
        public float distanceToDecresing;
        public float forceToAttack, forceToDecreasing;
        public float stunTime;
    }
}