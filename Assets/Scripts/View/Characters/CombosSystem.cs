using System;
using System.Collections.Generic;

namespace View.Characters
{
    public class CombosSystem : ICombosSystem
    {
        private readonly string _punch1, _punch2, _kick;
        private readonly string _kick1;
        private readonly List<AttackType> _attackTypes;
        private int _currentComboPosition;

        public CombosSystem(string punch1, string punch2, string kick, string kick1, List<AttackType> attackTypes)
        {
            _punch1 = punch1;
            _punch2 = punch2;
            _kick = kick;
            _kick1 = kick1;
            _attackTypes = attackTypes;
        }
        public void ExecuteKick(PlayerCharacter playerCharacter)
        {
            
        }

        public void ExecutePunch(PlayerCharacter playerCharacter)
        {
            
        }

        public void ResetCombo()
        {
            _currentComboPosition = 0;
        }
    }
}