using Audio;
using UnityEngine;
using Singleton;

namespace AudioStatePattern
{
    public class ClientStateAudio : Singleton <ClientStateAudio>, ICheckDependencies
    {
        
        [SerializeField] private MixerController m_MixerController;

        private void Start()
        {
            CheckDependencies();
            m_MixerController.MenuAudio();
        }
        public void ChangeSceneSnapshot(GameStates gameState)
        {
            switch (gameState)
            {
               case GameStates.MainMenu:
                   m_MixerController.MenuAudio();
                   break;
               case GameStates.MainScene:
                   m_MixerController.GameplayAudio();
                   break;
               case GameStates.CombatMode:
                   m_MixerController.CombatAudio();
                   break;
               case GameStates.Paused:
                   m_MixerController.PausedAudio();
                   break;
            }
        }
        public void CheckDependencies()
        {
            if (m_MixerController != null)
            {
                m_MixerController = (MixerController)FindObjectOfType(typeof(MixerController));
            }
        }
    }
}
    

