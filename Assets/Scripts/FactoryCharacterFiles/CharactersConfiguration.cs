using System;
using System.Collections.Generic;
using CharacterCustom;
using UnityEngine;

namespace FactoryCharacterFiles
{
    [CreateAssetMenu(menuName = "Bellseboss/CharacterConfiguration")]
    public class CharactersConfiguration : ScriptableObject
    {
        [SerializeField] private Character[] characters;
        private Dictionary<string, Character> idToCharacter;

        private void Awake()
        {
            idToCharacter = new Dictionary<string, Character>(characters.Length);
            foreach (var character in characters)
            {
                idToCharacter.Add(character.Id, character);
            }
        }

        public Character GetCharacterPrefabById(string id)
        {
            if (!idToCharacter.TryGetValue(id, out var characterPrefab))
            {
                throw new Exception($"Character with id {id} does not exit");
            }
            return characterPrefab;
        }
    }
}