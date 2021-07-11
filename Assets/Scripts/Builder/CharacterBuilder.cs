using System;
using CharacterCustom;
using InputSystemCustom;
using Object = UnityEngine.Object;

namespace Builder
{
    public class CharacterBuilder
    {
        private Character _character;
        private TypeOfInputs _input;

        public CharacterBuilder WithCharacter(Character character)
        {
            _character = character;
            return this;
        }

        public CharacterBuilder WithInput(TypeOfInputs inputCustom)
        {
            _input = inputCustom;
            return this;
        }
        
        public Character Build()
        {
            CheckPreConditions();
            var characterInstantiate = Object.Instantiate(_character);
            InputCustom valueOfInput = null;
            switch (_input)
            {
                case TypeOfInputs.PlayerControl:
                    valueOfInput = new MovementController(characterInstantiate);
                    break;
                case TypeOfInputs.EnemyIa:
                    valueOfInput = new MovementEnemies();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            characterInstantiate.Configure(valueOfInput);
            return characterInstantiate;
        }

        private void CheckPreConditions()
        {
            
        }
    }
}