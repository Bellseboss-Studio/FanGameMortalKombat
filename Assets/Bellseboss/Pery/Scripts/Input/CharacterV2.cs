using System;
using Cinemachine;
using UnityEngine;

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
        [Range(0,10)]
        [SerializeField] private float speedWalk;
        [Range(0,20)]
        [SerializeField] private float speedRun;
        [SerializeField] private AnimationController animationController;
        [SerializeField] private GameObject model3D;
        private GameObject _model3DInstance;
        [SerializeField] private RotationCharacterV2 rotationCharacterV2;
        [SerializeField] private CombatSystem combatSystem;
        [SerializeField] private float forceRotation;
        [SerializeField] private TargetFocus targetFocus;
        [SerializeField] private StatisticsOfCharacter statisticsOfCharacter;

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
            targetFocus.EnableCollider();
            
            movementRigidbodyV2.GetJumpSystem().OnAttack += JumpOnAttack;
            movementRigidbodyV2.GetJumpSystem().OnMidAir += JumpOnMidAir;
            movementRigidbodyV2.GetJumpSystem().OnRelease += JumpOnRelease;
            movementRigidbodyV2.GetJumpSystem().OnEndJump += JumpOnEndJump;
            
            rotationCharacterV2.CanRotate(true);
        }
        

        public void ActivateAnimationTrigger(string animationTrigger)
        {
            animationController.ActivateTrigger(animationTrigger);
        }

        public void SetPositionAndRotation(GameObject refOfPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, refOfPlayer.transform.position, 0.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, refOfPlayer.transform.rotation, 0.5f);
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
            if (GetAttackSystem().CanAttackAgain() && !GetAttackSystem().FullCombo())
            {
                animationController.Kick();
                combatSystem.PowerAttack();
                rotationCharacterV2.RotateToLookTheTarget(targetFocus.GetTarget());
                rotationCharacterV2.CanRotate(false);
            }
        }

        private void OnPunchEvent()
        {
            if (GetAttackSystem().CanAttackAgain() && !GetAttackSystem().FullCombo())
            {
                animationController.Punch();
                combatSystem.QuickAttack();
                rotationCharacterV2.RotateToLookTheTarget(targetFocus.GetTarget());
                rotationCharacterV2.CanRotate(false);
            }
        }

        private void OnTargetEvent(bool isTarget)
        {
            animationController.IsTarget(isTarget);
            movementRigidbodyV2.IsTarget(isTarget);
        }

        private void OnMove(Vector2 vector2)
        {
            
            if(rotationCharacterV2.CanRotate() && !movementRigidbodyV2.IsJump)
            {
                rotationCharacterV2.Direction(vector2);
            }

            movementRigidbodyV2.Direction(vector2);

        }

        public void PowerAttack(float runningDistance, Vector3 runningDirection)
        {
            if (GetAttackSystem().CanAttackAgain() && !GetAttackSystem().FullCombo())
            {
                rotationCharacterV2.Direction(runningDirection);
                movementRigidbodyV2.AddForce(runningDirection, runningDistance, AttackMovementSystem.TypeOfAttack.Power);
            }
        }

        public void QuickAttack(float runningDistance, Vector3 runningDirection)
        {
            if (GetAttackSystem().CanAttackAgain() && !GetAttackSystem().FullCombo())
            {
                rotationCharacterV2.Direction(runningDirection);
                movementRigidbodyV2.AddForce(runningDirection, runningDistance,
                    AttackMovementSystem.TypeOfAttack.Quick);
            }
        }
        
        public void DisableControls()
        {
            rotationCharacterV2.CanRotate(false);
            movementRigidbodyV2.CanMove(false);
        }

        public void CanMove()
        {
            movementRigidbodyV2.CanMove(true);
            rotationCharacterV2.CanRotate(true);
        }

        public Vector3 RotateToTarget(Vector3 originalDirection)
        {
            return targetFocus.RotateToTarget(originalDirection);
        }

        public bool CanAttack()
        {
            return GetAttackSystem().CanAttackAgain();
        }

        public AttackMovementSystem GetAttackSystem()
        {
            return movementRigidbodyV2.GetAttackSystem();
        }

        public void SetCamera(CinemachineVirtualCameraBase currentCamera)
        {
            ConfigCamera(currentCamera);
        }

        private void ConfigCamera(CinemachineVirtualCameraBase currentCamera)
        {
            movementRigidbodyV2.Configure(rigidbody, speedWalk, speedRun, currentCamera.gameObject, this, statisticsOfCharacter);
            rotationCharacterV2.Configure(currentCamera.gameObject, gameObject, this, forceRotation);
        }
        
        public void UpdateAnimation()
        {
            animationController.Movement(movementRigidbodyV2.GetVelocity(), 0);
        }

        public void ChangeToNormalJump()
        {
            movementRigidbodyV2.ChangeToNormalJump();
        }

        public void ChangeRotation(Vector3 rotation)
        {
            rotationCharacterV2.ChangeDirection(rotation);
        }

        public void RestoreRotation()
        {
            rotationCharacterV2.RestoreRotation();
        }

        public void LeaveGround(bool leave, float forceToGravitate, Vector3 direction)
        {
            movementRigidbodyV2.IsScalableWall(leave, forceToGravitate, direction);
        }

        public void ExitToWall()
        {
            movementRigidbodyV2.ExitToWall();
        }
    }
}
