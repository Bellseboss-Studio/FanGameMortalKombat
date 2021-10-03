using System;
using MenuUI.SystemOfExtras;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerOfExtra : MonoBehaviour
{
    private IExtra _extra;
    [SerializeField] private Image imageToShow;
    [SerializeField] private Button buttonToAction;
    [SerializeField] private Animator animator;
    [SerializeField] private Image imageOfResource;
    [SerializeField] private TextMeshProUGUI text;
    
    private void Call()
    {
        animator.SetBool("show", true);
        imageOfResource.enabled = false;
        text.enabled = false;
        switch (_extra.GetTypeExtra())
        {
            case "text":
                text.text = _extra.GetSource();
                text.enabled = true;
                break;
            case "image":
                var spriteToPixel = Resources.Load<Sprite>(_extra.GetSource());
                imageOfResource.sprite = spriteToPixel;
                imageOfResource.enabled = true;
                break;
            case "video":
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
}