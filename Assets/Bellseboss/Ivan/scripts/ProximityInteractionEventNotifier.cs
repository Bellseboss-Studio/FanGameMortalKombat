using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DoomlingsGame.SFX
{
    
    public class ProximityInteractionEventNotifier : MonoBehaviour
    {
        [SerializeField] private NotificationEvent notifyEnterProximityEvent;
        [SerializeField] private NotificationEvent notifyExitProximityEvent;
        [SerializeField] private string objectTag = "Player";
       
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(objectTag) && notifyEnterProximityEvent != null)
            {
                notifyEnterProximityEvent.Occurred();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(objectTag) && notifyExitProximityEvent != null)
            {
                notifyExitProximityEvent.Occurred();
            }
        }
    }
}