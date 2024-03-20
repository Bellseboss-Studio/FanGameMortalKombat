using System;
using UnityEngine;

public class ActivableOpenDoor : Activable
{
    [SerializeField] private Animator animator;
    
    private bool _isOpen;
    private static int OpenDoor;
    [SerializeField] private string triggerEventString;

    private void Start()
    {
        OpenDoor = Animator.StringToHash(triggerEventString);
    }

    public override void Activate()
    {
        if (_isOpen) return;
        animator.SetTrigger(OpenDoor);
    }
}