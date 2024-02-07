using System;
using UnityEngine;

public interface IColliderWithLayer
{
    Action<GameObject> ColliderEnter { get; set; }
    Action<GameObject> ColliderExit { get; set; }
}