using UnityEngine;
using UnityEngine.SceneManagement;

namespace View.UI
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private int sceneIndex;
        public void LoadSceneMethod()
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}