﻿using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

public class InteractiveObjectWithButton : InteractiveManager
{
    [SerializeField] protected string animationTrigger;
    [SerializeField] protected Animator animatorInteractiveObject;
    [SerializeField] protected GameObject refOfPlayer;

    protected override void OnColliderExit(GameObject o, CameraCollider room)
    {
        if (o.TryGetComponent(out ICharacterV2 character))
        {
            _characterV2.OnAction -= OnAction;
            _characterV2 = null;
        }
    }

    protected override void OnColliderEnter(GameObject o, CameraCollider room)
    {
        if (o.TryGetComponent(out ICharacterV2 character))
        {
            _characterV2 = character;
            _characterV2.OnAction += OnAction;
        }
    }

    protected override void OnAction()
    {
        //Debug.Log("InteractiveManager: OnAction");
        Debug.Log($"InteractiveManager: OnAction {playableDirector.duration}");
        playableDirector.Play();
        animatorInteractiveObject.SetTrigger("activate");
        _characterV2.DisableControls();
        _characterV2.ActivateAnimationTrigger(animationTrigger);
        canChangePosition = true;
        deltaTimeLocal = 0;
        cinemachineVirtualCamera.LookAt = refOfPlayer.transform;
    }

    private void Update()
    {
        if (!canChangePosition || _characterV2 == null) return;
        _characterV2.SetPositionAndRotation(refOfPlayer);
        deltaTimeLocal += Time.deltaTime;
        if (deltaTimeLocal >= playableDirector.duration)
        {
            canChangePosition = false;
            _characterV2.EnableControls();
            Debug.Log($"InteractiveManager: OnAction {deltaTimeLocal}");
        }
    }
}