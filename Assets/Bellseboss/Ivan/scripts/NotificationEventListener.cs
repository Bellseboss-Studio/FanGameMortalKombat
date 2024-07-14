using System;
using UnityEngine;
using UnityEngine.Events;

namespace DoomlingsGame.SFX
{
    [Serializable]
    public class NotificationEventListener : MonoBehaviour
    {
        [SerializeField] private NotificationEvent m_GameEvent;
        [SerializeField] private UnityEvent m_Response;

        private void OnEnable()
        {
            m_GameEvent.Register(this);
        }

        public void OnDisable()
        {
            m_GameEvent.Unregister(this);
        }
        public void OnEventOccurs()
        {
            m_Response.Invoke();
        }
    }
}