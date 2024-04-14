using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bellseboss.Angel.KeyRebinding
{
    public class RebindingKeyManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference moveRef, jumpRef, fireRef;

        private void OnEnable()
        {
            moveRef.action.Disable();
            jumpRef.action.Disable();
            fireRef.action.Disable();
        }

        private void OnDisable()
        {
            moveRef.action.Enable();
            jumpRef.action.Enable();
            fireRef.action.Enable();
        }
    }
}