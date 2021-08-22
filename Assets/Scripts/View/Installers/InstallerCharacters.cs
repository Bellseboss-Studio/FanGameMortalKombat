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
        [SerializeField] private CinemachineFreeLook cameraMainFreeLook;
        [SerializeField] private CinemachineTargetGroup group;
        private GameObject player, pointFar;

        private void Start()
        {
            var charactersFactory = new CharactersFactory(Instantiate(charactersConfiguration));
            var character = (PlayerCharacter) charactersFactory.Create(idCharacter).WithInput(TypeOfInputs.PlayerControl).WithCamera(cameraMainFreeLook.gameObject).Build();

            cameraMainFreeLook.Follow = character.GetPointToCamera();
            cameraMainFreeLook.LookAt = character.GetPointToCamera();

            /*
            cameraMain.Follow = character.GetPointToCamera();
            var _player = new CinemachineTargetGroup.Target();
            _player.target = character.GetPointToCamera();
            _player.weight = 1;
            var _pointFar = new CinemachineTargetGroup.Target();
            _pointFar.target = character.GetPointToGroupCamera();
            _pointFar.weight = 1;
            group.m_Targets = new[]
            {
                _player,
                _pointFar
            };
            cameraMain.m_Lens.FieldOfView = 20;
            var cinemachineTransposer = cameraMain.GetCinemachineComponent<CinemachineTransposer>();
            var followOffset = cinemachineTransposer.m_FollowOffset;
            cinemachineTransposer.m_FollowOffset = new Vector3(followOffset.x, 2, followOffset.z);
            */
        }
    }
}
