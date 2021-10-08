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
        [SerializeField] private string punch, kick;
        private bool changeIdle;
        private EventsOfFightPlayerInput playerInputFight;

        private Vector3 pointInicialToPointToFar;
        public bool CanMove;
        public bool CanReadInputs;

        private Vector2 movementinputValue;
        
        protected override void Start()
        {
            base.Start();
            pointInicialToPointToFar = pointFarToCamera.transform.localPosition;
            OnPunchEvent+=OnPunchEventInPlayer;
            OnKickEvent += OnKickEventInPlayer;
            CanMove = true;
            CanReadInputs = true;
            playerInputFight = new EventsOfFightPlayerInput(this, playerInput);
            OnFinishedAnimatorFight += OnFinishedAnimatorPlayer;
        }

        private void OnFinishedAnimatorPlayer()
        {
            CanMove = true;
            CanReadInputs = true;
            OnInputChangedExtend(movementinputValue);
        }

        private void OnKickEventInPlayer()
        {
            if (!CanReadInputs) return;
            
            animator.SetTrigger(kick);
            CanMove = false;
            CanReadInputs = false;
            Move(Vector3.zero);
            OnInputChangedExtend(movementinputValue);
        }

        private void OnPunchEventInPlayer()
        {
            if (!CanReadInputs) return;
            
            animator.SetTrigger(punch);
            CanMove = false;
            CanReadInputs = false;
            Move(Vector3.zero);
            OnInputChangedExtend(Vector2.zero);
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

        public override float GetDamageForKick()
        {
            return power * 4;
        }

        public override float GetDamageForPunch()
        {
            return power * 2;
        }

        private void OnMovementControllers(InputValue value)
        {
            movementinputValue = value.Get <Vector2>();
            OnInputChangedExtend(movementinputValue);
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
        
        public void OnPunch()
        {
            OnPunchEvent?.Invoke();
        }
        
        public void OnKick()
        {
            OnKickEvent?.Invoke();
        }
    }
}