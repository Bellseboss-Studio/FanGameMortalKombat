using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Audio
{
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
            
        }

        public void OnMouseClick()
        {
            
        }

        private void OnMouseEnter()
        {
        
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointEnter();
        }

        public void PointEnter()
        {
            OnMouseHover();
            m_MouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointExit();
        }

        public void PointExit()
        {
            m_MouseOver = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnMouseClick();
        }
    }
}