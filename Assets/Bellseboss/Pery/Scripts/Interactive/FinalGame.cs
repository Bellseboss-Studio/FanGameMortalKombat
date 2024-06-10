using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalGame : Activable
{    
    public override void Activate()
    {
        SceneManager.LoadScene(0);
    }
}