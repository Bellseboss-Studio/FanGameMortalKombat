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
        [SerializeField] private UISoundList[] m_SoundToPlayOnClick;
        [SerializeField] private UISoundList[] m_SoundToPlayOnHover;
        [SerializeField] private UISoundList[] m_SoundToPlayOnPointEnter;
        private IFmodManager m_FmodManager;
       
        private bool m_MouseOver = false;
        
        private void Awake()
        { 
            m_FmodManager = new FmodManagerUI();
        }
        
        private void OnMouseHover()
        {
            if (m_SoundToPlayOnHover == null)
            {
                return;
            }
            foreach (UISoundList sfx in m_SoundToPlayOnHover)
            {
                m_FmodManager.PlaySfx(sfx);
            }
        }

        public void OnMouseClick()
        {
            if (m_SoundToPlayOnClick == null)
            {
                return;
            }
            foreach (UISoundList sfx in m_SoundToPlayOnClick)
            {
                m_FmodManager.PlaySfx(sfx);
            }
        }

        private void OnMouseEnter()
        {
            if (m_SoundToPlayOnPointEnter == null)
            {
                return;
            }
            foreach (UISoundList sfx in m_SoundToPlayOnPointEnter)
            {
                m_FmodManager.PlaySfx(sfx);
            }
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