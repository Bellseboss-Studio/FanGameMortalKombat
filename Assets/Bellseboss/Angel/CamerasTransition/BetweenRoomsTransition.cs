using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Bellseboss.Angel
{
    [Serializable]
    public class BetweenRoomsTransition
    {
        public CameraCollider finishingRoom;
        public Transform[] cameraPoints;
        public float transitionTime;
        public Ease easeType;
    }
}