using MenuUI.SystemOfExtras;
using UnityEngine;
using UnityEngine.UI;
using View.UI;

public class ContainerOfExtra : MonoBehaviour
{
    private IExtra _extra;
    [SerializeField] private Image imageToShow;
    [SerializeField] private Button buttonToAction;

    [SerializeField] private ExtraMediator mediator;
    [SerializeField] private ChangeInputMap _content;

    public Button ButtonToAction => buttonToAction;

    private void Call()
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
        _content.ChangeInputMapToNew();
    }

    public void Configure(IExtra extra, ExtraMediator mediator, Button backButtonToShowExtra)
    {
        this.mediator = mediator;
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

        buttonToAction.onClick.AddListener(Call);
        _content.ChangeInputMapToNew(backButtonToShowExtra.gameObject);
    }
}