using System;
using System.Collections.Generic;

namespace View.Characters
{
    public class CombosSystem : ICombosSystem
    {
        private readonly string _punch1, _punch2, _kick;
        private readonly string _kick1;
        private List<string> _combo1;
        private int _currentComboPosition;

        public CombosSystem(string punch1, string punch2, string kick, string kick1)
        {
            _punch1 = punch1;
            _punch2 = punch2;
            _kick = kick;
            _kick1 = kick1;
            _combo1 = new List<string>(){"punch", "punch", "kick"};
        }
        public void ExecuteKick(PlayerCharacter playerCharacter)
        {
            if (_currentComboPosition >= _combo1.Count) return;
            if (_currentComboPosition == 0)
            {
                _currentComboPosition++;
                playerCharacter.ExecuteKickCombo(_kick);
            }
            else
            {
                if (_combo1[_currentComboPosition] == "kick")
                {
                    playerCharacter.ExecuteKickCombo(_kick1);
                }
            }
        }

        public void ExecutePunch(PlayerCharacter playerCharacter)
        {
            if (_currentComboPosition == 0)
            {
                _currentComboPosition++;
                playerCharacter.ExecutePunchCombo(_punch1);
            }
            else
            {
                if (_combo1[_currentComboPosition] == "punch")
                {
                    _currentComboPosition++;
                    playerCharacter.ExecutePunchCombo(_punch2);
                }
            }
        }

        public void ResetCombo()
        {
            _currentComboPosition = 0;
        }
    }
}