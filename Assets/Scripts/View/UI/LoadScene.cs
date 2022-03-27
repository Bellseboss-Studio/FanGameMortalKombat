using Audio.Managers;
using AudioStatePattern;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace View.UI
{
    public class LoadScene : MonoBehaviour
    {
        public void LoadSceneMethod()
        {
            SceneManager.LoadScene(1);
            MxManager.Instance.ChangeSceneMx(GameStates.MainScene);
            SfxManager.Instance.ChangeSceneAmbient(GameStates.MainScene);
            ClientStateAudio.Instance.ChangeSceneSnapshot(GameStates.MainScene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}