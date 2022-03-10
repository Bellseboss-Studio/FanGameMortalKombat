using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIButtonsSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{


    [SerializeField] List <UIAudioObjects> OnHoverTargetAudioObject = new List<UIAudioObjects>();
    [SerializeField] List <UIAudioObjects> OnClickTargetAudioObject = new List <UIAudioObjects>();
   
    private bool m_MouseOver = false;

    
    private void Awake()
    {
       
    }

    private void Update()
    {
        
    }

    private void OnMouseHover()
    {
        foreach(var goName in OnHoverTargetAudioObject)
        {
            SfxManager.Instance.PlaySound(goName.ToString());
        }
    }

    private void OnMouseClick()
    {
        foreach (var goName in OnClickTargetAudioObject)
        {
            SfxManager.Instance.PlaySound(goName.ToString());
        }
    }

    private void OnMouseEnter()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseHover();
        m_MouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_MouseOver = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnMouseClick();
        }

    }

}

