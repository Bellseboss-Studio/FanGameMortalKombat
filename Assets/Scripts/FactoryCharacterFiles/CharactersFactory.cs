using Builder;

namespace FactoryCharacterFiles
{
    public class CharactersFactory
    {
        private readonly CharactersConfiguration powerUpsConfiguration;

        public CharactersFactory(CharactersConfiguration powerUpsConfiguration)
        {
            this.powerUpsConfiguration = powerUpsConfiguration;
        }
        
        public CharacterBuilder Create(string id)
        {
            var prefab = powerUpsConfiguration.GetCharacterPrefabById(id);
            return new CharacterBuilder().WithCharacter(prefab);
        }
    }
}