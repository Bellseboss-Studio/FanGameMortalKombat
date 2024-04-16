using System;
using Cinemachine;
using UnityEngine;
using Bellseboss.Pery.Scripts.Input;

public class CharacterV3 : MonoBehaviour, IAnimationController, IMovementRigidBodyV2, IRotationCharacterV2, ICharacterV2
{
    public Action OnAction { get; set; }
    [SerializeField] private string id;
    [SerializeField] private StatisticsOfCharacter statisticsOfCharacter;
    [SerializeField] private InputPlayerV2 inputPlayerV2;
    [SerializeField] private MovementRigidbodyV2 movementRigidbodyV2;
    [SerializeField] private CinemachineVirtualCameraBase cameraMain;
    [SerializeField] private RotationCharacterV2 rotationCharacterV2;
    [SerializeField] private Rigidbody rigidbody;
    [Range(0, 10)]
    [SerializeField] private float speedWalk;
    [Range(0, 20)]
    [SerializeField] private float speedRun;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private GameObject model3D;
    [SerializeField] private float forceRotation;
    [SerializeField] private TargetFocus targetFocus;
    private GameObject _model3DInstance;
    private StatisticsOfCharacter _statisticsOfCharacter;

    void Start()
    {

        _statisticsOfCharacter = Instantiate(statisticsOfCharacter);
        _model3DInstance = Instantiate(model3D, transform);

        inputPlayerV2.onMoveEvent += OnMove;
        inputPlayerV2.onTargetEvent += OnTargetEvent;
        inputPlayerV2.onPunchEvent += OnPunchEvent;
        inputPlayerV2.onKickEvent += OnKickEvent;
        inputPlayerV2.onJumpEvent += OnJumpEvent;
        inputPlayerV2.onActionEvent += OnActionEvent;


        animationController.Configure(_model3DInstance.GetComponent<Animator>(), this);

        movementRigidbodyV2.Configure(rigidbody, speedWalk, speedRun, cameraMain.gameObject, this, _statisticsOfCharacter);

        rotationCharacterV2.Configure(cameraMain.gameObject, gameObject, this, forceRotation);
    }

    private void OnMove(Vector2 vector2)
    {

        if (rotationCharacterV2.CanRotate() && !movementRigidbodyV2.IsJump)
        {
            rotationCharacterV2.Direction(vector2);
        }

        movementRigidbodyV2.Direction(vector2);

    }

    private void OnKickEvent()
    {

    }

    private void OnPunchEvent()
    {

    }

    private void OnJumpEvent()
    {
        movementRigidbodyV2.Jump();
    }

    void OnActionEvent()
    {
        OnAction?.Invoke();
    }
    private void OnTargetEvent(bool isTarget)
    {
        animationController.IsTarget(isTarget);
        movementRigidbodyV2.IsTarget(isTarget);
    }

    public void UpdateAnimation()
    {
        
    }

    public void ChangeToNormalJump()
    {
        
    }

    public void ChangeRotation(Vector3 rotation)
    {
        
    }

    public void RestoreRotation()
    {
        
    }

    public void EndAttackMovement()
    {
        
    }

    public void PlayerFall()
    {
        
    }

    public void PlayerRecovery()
    {
        
    }

    public void DisableControls()
    {
        movementRigidbodyV2.CanMove(false);
        rotationCharacterV2.CanRotate(false);
    }

    public void ActivateAnimationTrigger(string animationTrigger)
    {
        animationController.ActivateTrigger(animationTrigger);
    }

    public void SetPositionAndRotation(GameObject refOfPlayer)
    {
        transform.position = refOfPlayer.transform.position;
        transform.rotation = refOfPlayer.transform.rotation;
    }

    public void CanMove()
    {
        movementRigidbodyV2.CanMove(true);
        rotationCharacterV2.CanRotate(true);
    }
}