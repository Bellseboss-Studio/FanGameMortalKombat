using CharacterCustom;
using UnityEngine.InputSystem;
using UnityEngine;

namespace View.Characters
{
    public class EventsOfFightPlayerInput
    {
        private readonly Character _character;
        private PlayerInput _playerInput;

        public EventsOfFightPlayerInput(Character character, PlayerInput playerInput)
        {
            _character = character;
            _playerInput = playerInput;
        }

    }
}