using UnityEngine;

public class ActivableOpenDoor : Activable
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject finalPosition;
    private bool _isOpen;
    public override void Activate()
    {
        if (_isOpen) return;
        door.transform.position = finalPosition.transform.position;
        _isOpen = true;
    }
}