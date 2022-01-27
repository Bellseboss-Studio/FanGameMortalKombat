using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using View.Characters;

namespace View
{
    public class CameraChange : MonoBehaviour
    {
        private Transform _pointToCamera;
        private CinemachineTargetGroup _targetGroup;
        private CinemachineFreeLook _virtualCamera;
        private PlayerCharacter _playerCharacter;
        
        public void AddGameObjectToTargetGroup(Transform transformm)
        {
            _targetGroup.AddMember(transformm, 1, 5);
        }
        
        public void RemoveGameObjectToTargetGroup(Transform transformm)
        {
            _targetGroup.RemoveMember(transformm);
        }

        public void LookEnemyAndPlayer()
        {
            _virtualCamera.LookAt = _targetGroup.gameObject.transform;
        }

        public void FreeLookCamera()
        {
            _virtualCamera.LookAt = _pointToCamera;
        }

        public void Configure(CinemachineTargetGroup targetGroup, CinemachineFreeLook virtualCamera, Transform pointToCamera, PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
            _targetGroup = targetGroup;
            _virtualCamera = virtualCamera;
            _pointToCamera = pointToCamera;
            _targetGroup.AddMember(_playerCharacter.transform, 1, 4);
        }
    }
}