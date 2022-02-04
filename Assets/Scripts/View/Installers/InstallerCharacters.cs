using System;
using Cinemachine;
using FactoryCharacterFiles;
using InputSystemCustom;
using UnityEngine;
using View.Characters;

namespace View.Installers
{
    public class InstallerCharacters : MonoBehaviour
    {
        [SerializeField] private CharactersConfiguration charactersConfiguration;
        [SerializeField] private string idCharacter;
        [SerializeField] private CinemachineVirtualCamera cameraMain;
        [SerializeField] private CinemachineFreeLook cameraMainFreeLook, secondCamera;
        [SerializeField] private CinemachineTargetGroup group;
        private GameObject player, pointFar;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            var charactersFactory = new CharactersFactory(Instantiate(charactersConfiguration));
            var character = (PlayerCharacter) charactersFactory.Create(idCharacter)
                .WithInput(TypeOfInputs.PlayerControl).InPosition(transform.position)
                .WithMainCamera(cameraMainFreeLook.gameObject)
                .Build();
            character.ConfigureCameras(cameraMainFreeLook, secondCamera, group);
            cameraMainFreeLook.Follow = character.GetPointToCamera();
            cameraMainFreeLook.LookAt = character.GetPointToCamera();
            ServiceLocator.Instance.GetService<IObserverUI>().Observer(character);
        }
    }
}
