using System;
using UnityEngine;

public interface IColliderWithLayer
{
    Action<GameObject, CameraCollider> ColliderEnter { get; set; }
    Action<GameObject, CameraCollider> ColliderExit { get; set; }
}