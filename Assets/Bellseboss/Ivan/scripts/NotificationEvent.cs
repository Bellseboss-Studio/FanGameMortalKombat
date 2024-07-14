using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomlingsGame.SFX
{
    [CreateAssetMenu(fileName = "New Notification Event", menuName = "ScriptableObjects/Sfx Notification Event")]
    public class NotificationEvent : ScriptableObject
    {
        private List<NotificationEventListener> m_EListeners = new List<NotificationEventListener>();

        public void Register(NotificationEventListener listener)
        {
            m_EListeners.Add(listener);
        }
        
        public void Unregister(NotificationEventListener listener)
        {
            m_EListeners.Remove(listener);
        }

        public void Occurred()
        {
            for (int i = 0; i < m_EListeners.Count; i++)
            {
                m_EListeners[i].OnEventOccurs();
            }
        }
    }
}
