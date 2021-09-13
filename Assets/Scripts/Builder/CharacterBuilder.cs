using System;
using CharacterCustom;
using InputSystemCustom;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Builder
{
    public class CharacterBuilder
    {
        private Character _character;
        private TypeOfInputs _input;
        private GameObject camera;
        private Vector3 position;

        public CharacterBuilder()
        {
            position = Vector3.zero;
        }

        public CharacterBuilder WithCharacter(Character character)
        {
            _character = character;
            return this;
        }

        public CharacterBuilder WithCamera(GameObject cameraExternal)
        {
            camera = cameraExternal;
            return this;
        }

        public CharacterBuilder WithInput(TypeOfInputs inputCustom)
        {
            _input = inputCustom;
            return this;
        }

        public CharacterBuilder InPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }
        
        public Character Build()
        {
            CheckPreConditions();
            var characterInstantiate = Object.Instantiate(_character);
            characterInstantiate.transform.position = position;
            InputCustom valueOfInput = null;
            switch (_input)
            {
                case TypeOfInputs.PlayerControl:
                    valueOfInput = new MovementController(characterInstantiate, camera);
                    break;
                case TypeOfInputs.EnemyIa:
                    valueOfInput = new MovementEnemies(characterInstantiate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            characterInstantiate.Configure(valueOfInput);
            valueOfInput.ConfigureInputWithCharacter();
            return characterInstantiate;
        }

        private void CheckPreConditions()
        {
            
        }
    }
}