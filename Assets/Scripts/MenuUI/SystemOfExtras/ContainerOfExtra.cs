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

    public void Add(IExtra extra)
    {
        _extra = extra;
    }

    public void Configure()
    {
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

        ConfigureActionsTuButton();
    }

    private void ConfigureActionsTuButton()
    {
        buttonToAction.onClick.AddListener(Call);
    }

    public void Clean()
    {
        _extra = null;
    }
    
}