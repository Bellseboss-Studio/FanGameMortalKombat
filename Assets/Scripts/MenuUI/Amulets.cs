using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Amulets : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;
    public Image amuleto;

    void Awake() 
    {
        amuleto.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse enter");
        isOver = true;
        if (isOver == true){
            amuleto.enabled = true;
        }     
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse exit");
        isOver = false;
        if (isOver == false)
        {
            amuleto.enabled = false;
        }
    }

}
