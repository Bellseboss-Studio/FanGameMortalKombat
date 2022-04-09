using Builder;

namespace FactoryCharacterFiles
{
    public class CharactersFactory
    {
        private readonly CharactersConfiguration charactersConfiguration;

        public CharactersFactory(CharactersConfiguration charactersConfiguration)
        {
            this.charactersConfiguration = charactersConfiguration;
        }
        
        public CharacterBuilder Create(string id)
        {
            var prefab = charactersConfiguration.GetCharacterPrefabById(id);
            return new CharacterBuilder().WithCharacter(prefab);
        }
    }
}