using System.Collections.Generic;
using FMODUnity;
using MortalKombat.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace MortalKombat.Audio
{
    public class UIButtonsSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private EventReference m_SfxToPlayOnClick;
        [SerializeField] private EventReference m_SfxToPlayOnHover;
        [SerializeField] private EventReference m_SfxToPlayOnPointEnter;
        private IFmodManager m_FmodManager;
       
        private bool m_MouseOver = false;
        
        private void Awake()
        { 
            m_FmodManager = new FmodManagerUI();
        }
        
        private void OnMouseHover()
        {
            if (!m_SfxToPlayOnHover.IsNull)
            {
                m_FmodManager.PlaySfx(m_SfxToPlayOnHover);
            }
        }

        public void OnMouseClick()
        {
            if (!m_SfxToPlayOnClick.IsNull)
            {
                m_FmodManager.PlaySfx(m_SfxToPlayOnClick);
            }
        }

        private void OnMouseEnter()
        {
            if (!m_SfxToPlayOnPointEnter.IsNull)
            {
                m_FmodManager.PlaySfx(m_SfxToPlayOnPointEnter);
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointEnter();
        }

        public void PointEnter()
        {
            OnMouseHover();
            if (!m_MouseOver)
            {
                m_MouseOver = true;
            }
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