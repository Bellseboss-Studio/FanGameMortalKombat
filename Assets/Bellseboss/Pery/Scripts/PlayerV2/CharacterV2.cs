using System;
using Bellseboss.Angel.CombatSystem;
using Cinemachine;
using ServiceLocatorPath;
using UnityEngine;
using View.Installers;

namespace Bellseboss.Pery.Scripts.Input
{
    public class CharacterV2 : PJV2, ICharacterV2, IMovementRigidBodyV2, IAnimationController, IRotationCharacterV2, ICombatSystem, IFocusTarget, ICombatSystemAngel, IFatality, ICharacterUi
    {
        public string Id => id;
        public Action OnAction { get; set; }

        [SerializeField] private string id;
        [SerializeField] private InputPlayerV2 inputPlayerV2;
        [SerializeField] private MovementRigidbodyV2 movementRigidbodyV2;
        [SerializeField] private CinemachineVirtualCameraBase cameraMain;
        [SerializeField] private Rigidbody rigidbody;
        [Range(0, 10)]
        [SerializeField] private float speedWalk;
        [Range(0, 20)]
        [SerializeField] private float speedRun;
        [SerializeField] private AnimationController animationController;
        [SerializeField] private GameObject model3D;
        private GameObject _model3DInstance;
        [SerializeField] private RotationCharacterV2 rotationCharacterV2;
        [SerializeField] private float forceRotation;
        [SerializeField] private TargetFocus targetFocus;
        [SerializeField] private StatisticsOfCharacter statisticsOfCharacter;
        [SerializeField] private CombatSystemAngel combatSystemAngel;
        [SerializeField] private MovementADSR movementADSR;

        [SerializeField, InterfaceType(typeof(IFatalitySystem))]
        private MonoBehaviour FatalitySystem;
        private IFatalitySystem fatalitySystem => FatalitySystem as IFatalitySystem;
        private StatisticsOfCharacter _statisticsOfCharacter;
        private bool IsDead;
        
        
        public event Action<float> OnEnterDamageEvent;
        public event Action<float> OnAddingEnergy;
        public float GetLife()
        {
            return statisticsOfCharacter.life;
        }

        void Start()
        {
            Configure();
        }

        public void Configure()
        {
            inputPlayerV2.onMoveEvent += OnMove;
            inputPlayerV2.onTargetEvent += OnTargetEvent;
            inputPlayerV2.onPunchEvent += OnPunchEvent;
            inputPlayerV2.onKickEvent += OnKickEvent;
            inputPlayerV2.onJumpEvent += OnJumpEvent;
            inputPlayerV2.onActionEvent += OnActionEvent;
            inputPlayerV2.onFatalityEvent += OnFatalityEvent;

            ConfigCamera(cameraMain);
            _model3DInstance = Instantiate(model3D, transform);
            animationController.Configure(_model3DInstance.GetComponent<Animator>(), this);
            targetFocus.Configure(this);
            targetFocus.EnableCollider();

            movementRigidbodyV2.GetJumpSystem().OnAttack += JumpOnAttack;
            movementRigidbodyV2.GetJumpSystem().OnMidAir += JumpOnMidAir;
            movementRigidbodyV2.GetJumpSystem().OnRelease += JumpOnRelease;
            movementRigidbodyV2.GetJumpSystem().OnEndJump += JumpOnEndJump;

            rotationCharacterV2.CanRotate(true);

            _statisticsOfCharacter = Instantiate(statisticsOfCharacter);

            movementADSR.Configure(GetComponent<Rigidbody>(), _statisticsOfCharacter, this);

            //fatalitySystem.Configure(this);
            
            ServiceLocator.Instance.GetService<IObserverUI>().Observer(this);
        }

        private void OnFatalityEvent()
        {
            //fatalitySystem.Fatality();
        }

        private void OnPause()
        {
            ServiceLocator.Instance.GetService<IPauseMainMenu>().Pause();
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
            isAnimationWasRun = false;
            isAnimationRecovered = false;
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
            combatSystemAngel.ExecuteMovement(TypeOfAttack.Power);
            /*if (GetAttackSystem().CanAttackAgain() && !GetAttackSystem().FullCombo())
            {
                animationController.Kick();
                combatSystem.PowerAttack();
                rotationCharacterV2.RotateToLookTheTarget(targetFocus.GetTarget());
                rotationCharacterV2.CanRotateWhileAttack(true);
            }*/
        }

        private void OnPunchEvent()
        {
            combatSystemAngel.ExecuteMovement(TypeOfAttack.Quick);
            /*if (GetAttackSystem().CanAttackAgain() && !GetAttackSystem().FullCombo())
            {
                animationController.Punch();
                combatSystem.QuickAttack();
                rotationCharacterV2.RotateToLookTheTarget(targetFocus.GetTarget());
                rotationCharacterV2.CanRotateWhileAttack(true);
            }*/
        }

        private void OnTargetEvent(bool isTarget)
        {
            animationController.IsTarget(isTarget);
            movementRigidbodyV2.IsTarget(isTarget);
        }

        private void OnMove(Vector2 vector2)
        {
            if (combatSystemAngel.Attacking)
            {
                combatSystemAngel.oneTimeOnEndAttack += () =>
                {
                    if (rotationCharacterV2.CanRotate() && !movementRigidbodyV2.IsJump)
                    {
                        rotationCharacterV2.Direction(vector2);
                    }
                };
            }
            else
            {
                if (rotationCharacterV2.CanRotate() && !movementRigidbodyV2.IsJump)
                {
                    rotationCharacterV2.Direction(vector2);
                }
            }
            movementRigidbodyV2.Direction(vector2);
        }

        public void PowerAttack(float runningDistance, Vector3 runningDirection)
        {
            /*if (GetAttackSystem().CanAttackAgain() && !GetAttackSystem().FullCombo())
            {
                rotationCharacterV2.Direction(runningDirection);
                movementRigidbodyV2.AddForce(runningDirection, runningDistance, AttackMovementSystem.TypeOfAttack.Power);
            }*/
        }

        public void QuickAttack(float runningDistance, Vector3 runningDirection)
        {
            /*if (GetAttackSystem().CanAttackAgain() && !GetAttackSystem().FullCombo())
            {
                rotationCharacterV2.Direction(runningDirection);
                movementRigidbodyV2.AddForce(runningDirection, runningDistance,
                    AttackMovementSystem.TypeOfAttack.Quick);
            }*/
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

        public Action<string> GetActionToAnimate()
        {
            return animationController.SetTrigger;
        }

        public void PlayerTouchEnemy()
        {
            OnAddingEnergy?.Invoke(_statisticsOfCharacter.energyToAdd);
        }

        public Vector3 RotateToTargetAngel(Vector3 originalDirection)
        {
            return targetFocus.RotateToTarget(originalDirection);
        }

        public Vector3 RotateToTarget(Vector3 originalDirection)
        {
            return targetFocus.RotateToTarget(originalDirection);
        }

        /*public bool CanAttack()
        {
            return GetAttackSystem().CanAttackAgain();
        }*/

        /*public AttackMovementSystem GetAttackSystem()
        {
            return movementRigidbodyV2.GetAttackSystem();
        }*/

        public void SetCamera(CinemachineVirtualCameraBase currentCamera)
        {
            ConfigCamera(currentCamera);
        }

        private void ConfigCamera(CinemachineVirtualCameraBase currentCamera)
        {
            movementRigidbodyV2.Configure(rigidbody, speedWalk, speedRun, currentCamera.gameObject, this, statisticsOfCharacter);
            combatSystemAngel.Configure(rigidbody, statisticsOfCharacter, this, this);
            rotationCharacterV2.Configure(currentCamera.gameObject, gameObject, this, forceRotation);
        }

        public void UpdateAnimation()
        {
            animationController.Movement(movementRigidbodyV2.GetXZVelocity(), 0);
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

        public void EndAttackMovement()
        {
            rotationCharacterV2.Direction(Vector3.zero);
            rotationCharacterV2.CanRotateWhileAttack(false);
        }

        private bool isAnimationWasRun, isAnimationRecovered;
        public void PlayerFall()
        {
            /*if (isAnimationWasRun || movementRigidbodyV2.IsJumpingFromADRS()) return;
            //animationController.JumpFall();
            animationController.Fall();
            isAnimationWasRun = true;*/
        }

        public void PlayerRecovery()
        {
            /*if (isAnimationRecovered || !isAnimationWasRun) return;
            animationController.JumpRecovery();
            isAnimationRecovered = true;*/
        }

        public void PlayerFallV2()
        {
            animationController.Fall();
        }

        public void PlayerRecoveryV2()
        {
            animationController.JumpRecovery();
        }

        public bool IsAttacking()
        {
            return combatSystemAngel.Attacking;
        }

        /*public void PlayerTouchingFloorChanged(bool isTouching)
        {
            if (isTouching)
            {
                if ()
                {
                    throw new NotImplementedException();
                }
            }
        }*/

        public void TouchedScallableWall(bool leave, float forceToGravitate, Vector3 direction)
        {
            movementRigidbodyV2.IsScalableWall(leave, forceToGravitate, direction);
        }

        public void ExitScalableWall()
        {
            movementRigidbodyV2.ExitToWall();
        }

        public override void ReceiveDamage(int damage, Vector3 transformForward)
        {
            if (IsDead) return;
            _statisticsOfCharacter.life -= damage;
            if (_statisticsOfCharacter.life <= 0)
            {
                IsDead = true;
                //OnDead?.Invoke(this);
                Debug.Log("CharacterV2: Dead");
            }
            if (movementADSR.CanAttackAgain() && !IsDead)
            {
                movementADSR.Attack(transformForward);
            }
            rotationCharacterV2.RotateToDirection(transformForward);
        }

        public override void SetAnimationToHit(bool isQuickAttack, int numberOfCombosQuick)
        {
            if (IsDead) return;
            Debug.Log($"EnemyV2: SetAnimationToHit isQuickAttack: {isQuickAttack} numberOfCombos: {numberOfCombosQuick}");
            animationController.TakeDamage(isQuickAttack, numberOfCombosQuick);
            OnEnterDamageEvent?.Invoke(10);
        }

        public override void Stun(bool isStun)
        {
            movementRigidbodyV2.CanMove(!isStun);
            rotationCharacterV2.CanRotate(!isStun);
        }

    }

    public interface ICharacterUi
    {
        event Action<float> OnEnterDamageEvent;
        event Action<float> OnAddingEnergy;
        float GetLife();
    }
}
