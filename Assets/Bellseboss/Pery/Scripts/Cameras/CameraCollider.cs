using System;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    public Action ColliderEnter;
    public Action ColliderExit;
    
    private void OnTriggerEnter(Collider other)
    {
        var o = other.gameObject;
        Debug.Log($"CameraCollider:OnTriggerEnter: {o.name} - {o.layer} - {layer}");
        //validate if the layer is the same
        if ((layer.value & 1 << o.layer) > 0)
        {
            ColliderEnter?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var o = other.gameObject;
        Debug.Log($"CameraCollider:OnTriggerExit: {o.name} - {o.layer} - {layer}");
        //validate if the layer is the same
        if ((layer.value & 1 << o.layer) > 0)
        {
            ColliderExit?.Invoke();
        }
    }
}
