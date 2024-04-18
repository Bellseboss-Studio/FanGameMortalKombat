using System;
using System.Collections.Generic;
using TypeOfAttack = Bellseboss.Angel.CombatSystem.CombatSystemAngel.TypeOfAttack;

namespace Bellseboss.Angel.CombatSystem
{
    [Serializable]
    public class CombatMovement
    {
        public List<TypeOfAttack> comboSequence;
        public string transitionParameterName;
        public float timeToAttack, timeToDecreasing, timeToSustain, timeToRelease;
        public float maxDistance;
        public float distanceToDecresing;
        public float forceToAttack, forceToDecreasing;
        public float stunTime;
    }
}