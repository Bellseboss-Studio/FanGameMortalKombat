using MenuUI.SystemOfExtras;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class ContainerOfExtra : MonoBehaviour
{
    private IExtra _extra;
    [SerializeField] private Image imageToShow;
    [SerializeField] private Button buttonToAction;

    [SerializeField] private ExtraMediator mediator;
    
    private async void Call()
    {
        if (_extra == null) return;
        switch (_extra.GetTypeExtra())
        {
            case "text":
                mediator.SetExtraText(_extra);
                break;
            case "image":
                mediator.SetExtraImage(_extra);
                break;
            case "video":
                mediator.SetExtraVideo(_extra);
                break;
            case "audio":
                break;
        }
    }

    public void Configure(IExtra extra)
    {
        _extra = extra;
        if (_extra == null)
        {
            var spriteToPixel = Resources.Load<Sprite>("SinDatos");
            imageToShow.sprite = spriteToPixel;
        }
        else
        {
            var spriteToPixel = Resources.Load<Sprite>(_extra.GetIcon());
            imageToShow.sprite = spriteToPixel;
        }
        Debug.Log("Configure");
        ConfigureActionsTuButton();
    }

    private void ConfigureActionsTuButton()
    {
        buttonToAction.onClick.AddListener(Call);
        buttonToAction.navigation = new Navigation {mode = Navigation.Mode.Explicit};
        /*var navigation = buttonToAction.navigation;
        navigation.selectOnDown = buttonToAction;
        buttonToAction.navigation = navigation;*/
        
    }

    public void Clean()
    {
        _extra = null;
    }
    
}