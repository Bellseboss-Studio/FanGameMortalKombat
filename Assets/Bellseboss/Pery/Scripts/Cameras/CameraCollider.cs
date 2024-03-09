using System;
using UnityEngine;

public class CameraCollider : MonoBehaviour, IColliderWithLayer
{
    [SerializeField] private LayerMask layer;
    public Action<GameObject> ColliderEnter { get; set; }
    public Action<GameObject> ColliderExit { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        var o = other.gameObject;
        //Debug.Log($"CameraCollider:OnTriggerEnter: {o.name} - {o.layer} - {layer}");
        //validate if the layer is the same
        if ((layer.value & 1 << o.layer) > 0)
        {
            ColliderEnter?.Invoke(o);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var o = other.gameObject;
        //Debug.Log($"CameraCollider:OnTriggerExit: {o.name} - {o.layer} - {layer}");
        //validate if the layer is the same
        if ((layer.value & 1 << o.layer) > 0)
        {
            ColliderExit?.Invoke(o);
        }
    }
}