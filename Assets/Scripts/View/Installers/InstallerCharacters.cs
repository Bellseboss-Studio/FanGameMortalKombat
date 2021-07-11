using FactoryCharacterFiles;
using InputSystemCustom;
using UnityEngine;

namespace View.Installers
{
    public class InstallerCharacters : MonoBehaviour
    {
        [SerializeField] private CharactersConfiguration charactersConfiguration;
        [SerializeField] private string idCharacter;

        private void Start()
        {
            var charactersFactory = new CharactersFactory(Instantiate(charactersConfiguration));
            var character = charactersFactory.Create(idCharacter).WithInput(TypeOfInputs.PlayerControl).Build();
        }
    }
}
