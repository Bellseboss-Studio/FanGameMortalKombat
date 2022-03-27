using System;
using Audio.Managers;
using AudioStatePattern;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ServiceLocatorPath
{
    public class PauseMenu : MonoBehaviour, IPauseMainMenu
    {
        [SerializeField] private Animator animador;
        
        public delegate void OnPause(bool isPause);
        
        public OnPause onPause { get; set; }
        private bool isInPause;
        private bool isTransitioningToMainMenu;
        public void Configuracion()
        {
        
        }

        public void Pause()
        {
            isInPause = !isInPause;
            Debug.Log("Pause");
            animador.SetBool("pause", isInPause);
            onPause?.Invoke(isInPause);
            if (!isTransitioningToMainMenu)
            {
                ClientStateAudio.Instance.ChangeSceneSnapshot(isInPause ? GameStates.Paused : GameStates.MainScene);
                MxManager.Instance.ChangeSceneMx(isInPause? GameStates.Paused : GameStates.MainScene);
            }
            
        }
        
        

        public void Exit()
        {
            SceneManager.LoadScene(0);
            ClientStateAudio.Instance.ChangeSceneSnapshot(GameStates.MainMenu);
            MxManager.Instance.ChangeSceneMx(GameStates.MainMenu);
            SfxManager.Instance.ChangeSceneAmbient(GameStates.MainMenu);
            isTransitioningToMainMenu = true;
            Pause();
        }

    }
}
