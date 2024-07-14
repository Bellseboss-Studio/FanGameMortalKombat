using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

namespace MortalKombat.Audio
{
    public class PositionNotifier : MonoBehaviour
    {
        [SerializeField] LocationEvent positionChanged;
       // private IPlayerController playerController;
    
        private void Awake()
        {
          //  playerController = ServiceLocator.GetService<IPlayerController>();
        }
        
        void Update()
        {
            positionChanged.Occurred(transform.position);
        }
    }
}
