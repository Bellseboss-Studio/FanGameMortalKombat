using System;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using Object = UnityEngine.Object;

public class InteractiveManager : MonoBehaviour
{
    [SerializeField, InterfaceType(typeof(IColliderWithLayer))]
    private Object colliderWithLayer;
    [SerializeField] private Activable activable;
    private IColliderWithLayer _collider => colliderWithLayer as IColliderWithLayer;
    private ICharacterV2 _characterV2;

    private void Start()
    {
        _collider.ColliderEnter += OnColliderEnter;
        _collider.ColliderExit += OnColliderExit;
    }

    private void OnColliderExit(GameObject o)
    {
        if(o.TryGetComponent(out ICharacterV2 character))
        {
            _characterV2.OnAction -= OnAction;
            _characterV2 = null;
        }
    }

    private void OnColliderEnter(GameObject o)
    {
        if(o.TryGetComponent(out ICharacterV2 character))
        {
            _characterV2 = character;
            _characterV2.OnAction += OnAction;
        }
    }

    private void OnAction()
    {
        //Debug.Log("InteractiveManager: OnAction");
        activable.Activate();
    }
}
