using System;
using Bellseboss.Pery.Scripts.Input;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

public class InteractiveManager : MonoBehaviour
{
    [SerializeField, InterfaceType(typeof(IColliderWithLayer))]
    private Object colliderWithLayer;
    [SerializeField] private Activable activable;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private string animationTrigger;
    [SerializeField] private Animator animatorInteractiveObject;
    [SerializeField] private GameObject refOfPlayer;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private bool canChangePosition;
    private IColliderWithLayer _collider => colliderWithLayer as IColliderWithLayer;
    private ICharacterV2 _characterV2;
    private double deltaTimeLocal;

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
    
    private void Update()
    {
        if(!canChangePosition) return;
        _characterV2.SetPositionAndRotation(refOfPlayer);
        deltaTimeLocal += Time.deltaTime;
        if (deltaTimeLocal >= playableDirector.duration)
        {
            canChangePosition = false;
            _characterV2.CanMove();
        }
    }

    private void OnAction()
    {
        //Debug.Log("InteractiveManager: OnAction");
        activable.Activate();
        playableDirector.Play();
        animatorInteractiveObject.SetTrigger("activate");
        _characterV2.DisableControls();
        _characterV2.ActivateAnimationTrigger(animationTrigger);
        canChangePosition = true;
        deltaTimeLocal = 0;
        cinemachineVirtualCamera.LookAt = refOfPlayer.transform;
    }
}
