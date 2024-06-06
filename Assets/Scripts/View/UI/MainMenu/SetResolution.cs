using TMPro;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown_Resolution;
    
    private void Start()
    {
        dropdown_Resolution.onValueChanged.AddListener(OnResolutionChanged);
    }
    
    private void OnResolutionChanged(int arg0)
    {
        switch (arg0)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1600, 900, true);
                break;
            case 2:
                Screen.SetResolution(1280, 720, true);
                break;
            case 3:
                Screen.SetResolution(1024, 576, true);
                break;
            case 4:
                Screen.SetResolution(800, 600, true);
                break;
            case 5:
                Screen.SetResolution(640, 480, true);
                break;
        }
        Debug.Log("Resolution changed to: " + Screen.width + "x" + Screen.height);
    }
}