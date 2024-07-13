using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MortalKombat.Audio
{
    [CreateAssetMenu(fileName = "New Location Change Event", menuName = "ScriptableObjects/Sfx Location Event")]
    public class LocationEvent : ScriptableObject
    {
        private List<LocationEventListener> m_EListeners = new List<LocationEventListener>();

        public void Register(LocationEventListener listener)
        {
            if (!m_EListeners.Contains(listener))
            {
                m_EListeners.Add(listener);
            }
        }
        
        public void Unregister(LocationEventListener listener)
        {
            m_EListeners.Remove(listener);
        }

        public void Occurred(Vector3 position)
        {
            for (int i = 0; i < m_EListeners.Count; i++)
            {
                m_EListeners[i].OnEventOccurs(position);
            }
        }
    }
}
