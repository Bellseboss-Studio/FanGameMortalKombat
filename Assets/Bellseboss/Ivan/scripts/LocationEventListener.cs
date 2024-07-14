using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace MortalKombat.Audio
{
    [Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }
    
    [Serializable]
    public class LocationEventListener : MonoBehaviour
    {
        [SerializeField] private LocationEvent m_GameLocationEvent;
        [SerializeField] private Vector3Event m_Response;

        private void OnEnable()
        {
            if (m_GameLocationEvent != null)
            {
                m_GameLocationEvent.Register(this);
            }
            else
            {
                Debug.LogWarning("GameEvent is not assigned on " + gameObject.name);
            }
        }
        
        private void OnDisable()
        {
            if (m_GameLocationEvent != null)
            {
                m_GameLocationEvent.Unregister(this);
            }
        }

        public void OnEventOccurs(Vector3 position)
        {
            if (m_Response != null)
            {
                m_Response.Invoke(position);
            }
            else
            {
                Debug.LogWarning("Response is not set on " + gameObject.name);
            }
        }
        
        private void OnValidate()
        {
            if (m_GameLocationEvent == null)
            {
                Debug.LogError("GameEvent is not assigned on " + gameObject.name, this);
            }
        }

    }
}