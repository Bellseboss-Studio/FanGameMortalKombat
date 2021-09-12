using System.Collections;
using System.Diagnostics;
using CharacterCustom;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace View.Characters
{
    public class PlayerCharacter : Character
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] protected GameObject pointToCamera, pointFarToCamera;
        private bool changeIdle;

        private Vector3 pointInicialToPointToFar;
        protected override void Start()
        {
            base.Start();
            pointInicialToPointToFar = pointFarToCamera.transform.localPosition;
        }

        protected override void UpdateLegacy()
        {
            if (!changeIdle)
            {
                if (Random.Range(0, 100) < 2)
                {
                    changeIdle = true;
                    animator.SetTrigger("change_idle");
                    StartCoroutine(DelayToIdle());
                }
            }
        }

        private IEnumerator DelayToIdle()
        {
            yield return new WaitForSeconds(5f);
            changeIdle = false;
        }

        protected override void ConfigureExplicit()
        {
            _inputCustom.playerInput = playerInput;
        }
        private void OnMovementControllers(InputValue value)
        {
            OnInputChangedExtend(value.Get<Vector2>());
        }

        private void OnCameraMovement(InputValue value)
        {
            var vector2 = value.Get<Vector2>();
            OnCameraMovementExtend(vector2);
        }

        public Transform GetPointToCamera()
        {
            return pointToCamera.transform;
        }

        public Transform GetPointToGroupCamera()
        {
            return pointFarToCamera.transform;
        }

        private bool _isOn = true;
        private void OnButtonsToAction(InputValue value)
        {
            if (_isOn)
            {
                IsOnPress();
            }
            else
            {
                IsOffPress();
            }
            Debug.Log($"is on {_isOn}");
        }

        private void IsOnPress()
        {
            OnLeftShitOn?.Invoke();
            Debug.Log($"Press On");
            _isOn = false;
        }

        private void IsOffPress()
        {
            OnLeftShitOff?.Invoke();
            Debug.Log($"Press Off");
            _isOn = true;
        }

        public float GetLife()
        {
            return life;
        }
    }
}