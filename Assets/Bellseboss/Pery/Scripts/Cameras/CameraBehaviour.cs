using System;
using Bellseboss.Pery.Scripts.Input;
using Cinemachine;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour, ICameraBehaviour
{
    [SerializeField] private CharacterV2 character;
    [SerializeField] private CameraBehaviourWithReel[] rooms;
    private CameraBehaviourWithReel _currentRoom;

    private void Start()
    {
        for (var i = 0; i < rooms.Length; i++)
        {
            var room = rooms[i];
            var index = i;
            room.Config(this, index);
            room.ChangeCamera += OnChangeCamera;
        }
        _currentRoom = rooms[0];
        character.SetCamera(_currentRoom.Camera);
    }

    private void OnChangeCamera(int index, bool isOpen, CinemachineVirtualCamera camera)
    {
        if (isOpen)
        {
            _currentRoom.Camera.gameObject.SetActive(false);
            _currentRoom = rooms[index];
            _currentRoom.Camera.gameObject.SetActive(true);
            character.SetCamera(camera);
        }
    }
}