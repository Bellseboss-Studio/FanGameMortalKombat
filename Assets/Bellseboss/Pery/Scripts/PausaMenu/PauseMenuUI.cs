using ServiceLocatorPath;
using UnityEngine;
using UnityEngine.SceneManagement;
using View.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private ChangeInputMap changeInputMap;
    [SerializeField] private GameObject newInputSelected;
    private void Start()
    {
        ServiceLocator.Instance.GetService<IPauseMainMenu>().onPause += OnPause;
    }

    private void OnPause(bool ispause)
    {
        pauseMenu.SetActive(ispause);
        changeInputMap.ChangeInputMapToNew(ispause ? newInputSelected : null);
        changeInputMap.ChangeInputMapToNew();
    }
    
    public void Resume()
    {
        ServiceLocator.Instance.GetService<IPauseMainMenu>().onPause.Invoke(false);
    }
    
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}