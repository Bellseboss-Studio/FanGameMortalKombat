using UnityEngine;
using UnityEngine.SceneManagement;

namespace View.UI
{
    public class LoadScene : MonoBehaviour
    {
        public void LoadSceneMethod()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
