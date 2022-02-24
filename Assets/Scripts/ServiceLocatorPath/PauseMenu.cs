using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceLocatorPath
{
    public class PauseMenu : MonoBehaviour, IPauseMainMenu
    {
        [SerializeField] private Animator animador;

        public delegate void OnPause(bool isPause);
        public OnPause onPause { get; set; }
        private bool isInPause;
    
        public void Configuracion()
        {
        
        }

        public void Pause()
        {
            isInPause = !isInPause;
            Debug.Log("Pause");
            animador.SetBool("pause", isInPause);
            onPause?.Invoke(isInPause);
        }

        public void Exit()
        {
            SceneManager.LoadScene(0);
            MxManager.Instance.PlayMusicState();
            Pause();
        }

    }
}
