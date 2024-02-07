using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bellseboss.Pery.Scripts.Input
{
    public class CharacterV2 : MonoBehaviour, ICharacterV2, IMovementRigidBodyV2, IAnimationController, IRotationCharacterV2, ICombatSystem, IFocusTarget
    {
        public string Id => id;
        public Action OnAction { get; set; }
        [SerializeField] private string id;
        [SerializeField] private InputPlayerV2 inputPlayerV2;
        [SerializeField] private MovementRigidbodyV2 movementRigidbodyV2;
        [SerializeField] private CinemachineVirtualCameraBase cameraMain;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float speed;
        [SerializeField] private AnimationController animationController;
        [SerializeField] private GameObject model3D;
        private GameObject _model3DInstance;
        [SerializeField] private RotationCharacterV2 rotationCharacterV2;
        [SerializeField] private CombatSystem combatSystem;
        [SerializeField] private float forceRotation;
        [SerializeField] private TargetFocus targetFocus;

        private void Start()
        {
            inputPlayerV2.onMoveEvent += OnMove;
            inputPlayerV2.onTargetEvent += OnTargetEvent;
            inputPlayerV2.onPunchEvent += OnPunchEvent;
            inputPlayerV2.onKickEvent += OnKickEvent;
            inputPlayerV2.onJumpEvent += OnJumpEvent;
            inputPlayerV2.onActionEvent += OnActionEvent;

            ConfigCamera(cameraMain);
            _model3DInstance = Instantiate(model3D, transform);
            animationController.Configure(_model3DInstance.GetComponent<Animator>(), this);
            combatSystem.Configure(this);
            targetFocus.Configure(this);
            
            movementRigidbodyV2.GetJumpSystem().OnAttack += JumpOnAttack;
            movementRigidbodyV2.GetJumpSystem().OnMidAir += JumpOnMidAir;
            movementRigidbodyV2.GetJumpSystem().OnRelease += JumpOnRelease;
            movementRigidbodyV2.GetJumpSystem().OnEndJump += JumpOnEndJump;
        }

        private void JumpOnEndJump()
        {
            animationController.JumpRecovery();
        }

        private void JumpOnRelease()
        {
            animationController.JumpFall();
        }

        private void JumpOnMidAir()
        {
            animationController.JumpMidAir();
        }

        private void JumpOnAttack()
        {
            animationController.JumpJump();
        }

        void OnActionEvent()
        {
            OnAction?.Invoke();
        }

        private void OnJumpEvent()
        {
            movementRigidbodyV2.Jump();
        }

        private void OnKickEvent()
        {
            if (combatSystem.CanPowerAttack)
            {
                animationController.Kick();
                combatSystem.PowerAttack();
            }
        }

        private void OnPunchEvent()
        {
            if(combatSystem.CanQuickAttack)
            {
                animationController.Punch();
                combatSystem.QuickAttack();
            }
        }

        private void OnTargetEvent(bool isTarget)
        {
            animationController.IsTarget(isTarget);
            movementRigidbodyV2.IsTarget(isTarget);
        }

        private void OnMove(Vector2 vector2)
        {
            if(rotationCharacterV2.CanRotate())
            {
                rotationCharacterV2.Direction(vector2);
            }
            movementRigidbodyV2.Direction(vector2);
            animationController.Movement(vector2, vector2.y);
        }

        public void PowerAttack(float runningDistance, Vector3 runningDirection)
        {
            rotationCharacterV2.CanRotate(false);
            rotationCharacterV2.RotateToDirectionToMove(runningDirection);
            movementRigidbodyV2.AddForce(runningDirection, runningDistance);
        }

        public void QuickAttack(float runningDistance, Vector3 runningDirection)
        {
            rotationCharacterV2.CanRotate(false);
            rotationCharacterV2.RotateToDirectionToMove(runningDirection);
            movementRigidbodyV2.AddForce(runningDirection, runningDistance);
        }

        public void CanMove()
        {
            movementRigidbodyV2.CanMove();
            rotationCharacterV2.CanRotate(true);
        }

        public Vector3 RotateToTarget(Vector3 originalDirection)
        {
            return targetFocus.RotateToTarget(originalDirection);
        }

        public void SetCamera(CinemachineVirtualCameraBase currentCamera)
        {
            ConfigCamera(currentCamera);
        }

        private void ConfigCamera(CinemachineVirtualCameraBase currentCamera)
        {
            movementRigidbodyV2.Configure(rigidbody, speed, currentCamera.gameObject, this);
            rotationCharacterV2.Configure(currentCamera.gameObject, gameObject, this, forceRotation);
        }
    }
}
