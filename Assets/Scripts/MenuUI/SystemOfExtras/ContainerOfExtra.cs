using System;
using MenuUI.SystemOfExtras;
using UnityEngine;
using UnityEngine.UI;

public class ContainerOfExtra : MonoBehaviour
{
    private IExtra _extra;
    [SerializeField] private Image imageToShow;
    [SerializeField] private Button buttonToAction;
    [SerializeField] private Animator animator;
    [SerializeField] private Image imageOfResource;
    
    private void Call()
    {
        animator.SetBool("show", true);
        var spriteToPixel = Resources.Load<Sprite>(_extra.GetSource());
        imageOfResource.sprite = spriteToPixel;
    }

    public void Add(IExtra extra)
    {
        _extra = extra;
        Debug.Log("Agregado un extra");
    }

    public void Configure()
    {
        if (_extra == null)
        {
            //no tiene contenido
            Debug.Log("sin configuracion");
            var spriteToPixel = Resources.Load<Sprite>("SinDatos");
            imageToShow.sprite = spriteToPixel;
        }
        else
        {
            Debug.Log("con configuracion");
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