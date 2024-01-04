using Cinemachine;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class CharacterV2 : MonoBehaviour, ICharacterV2, IMovementRigidBodyV2, IAnimationController, IRotationCharacterV2, ICombatSystem
    {
        public string Id => id;
        [SerializeField] private string id;
        [SerializeField] private InputPlayerV2 inputPlayerV2;
        [SerializeField] private MovementRigidbodyV2 movementRigidbodyV2;
        [SerializeField] private CinemachineFreeLook cameraFreeLook;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float speed;
        [SerializeField] private AnimationController animationController;
        [SerializeField] private GameObject model3D;
        private GameObject _model3DInstance;
        [SerializeField] private RotationCharacterV2 rotationCharacterV2;
        [SerializeField] private CombatSystem combatSystem;
        [SerializeField] private float forceRotation;
        private Vector2 _localVector2;
        private bool _localIsTarget;

        private void Start()
        {
            inputPlayerV2.onMoveEvent += OnMove;
            inputPlayerV2.onTargetEvent += OnTargetEvent;
            inputPlayerV2.onPunchEvent += OnPunchEvent;
            inputPlayerV2.onKickEvent += OnKickEvent;
            movementRigidbodyV2.Configure(rigidbody, speed, cameraFreeLook.gameObject, this);
            _model3DInstance = Instantiate(model3D, transform);
            animationController.Configure(_model3DInstance.GetComponent<Animator>(), this);
            rotationCharacterV2.Configure(cameraFreeLook.gameObject, gameObject, this);
            combatSystem.Configure(this);
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
            _localIsTarget = isTarget;
        }

        private void OnMove(Vector2 vector2)
        {
            rotationCharacterV2.Direction(vector2);
            movementRigidbodyV2.Direction(vector2);
            animationController.Movement(vector2, vector2.y);
            _localVector2 = vector2;
            //RotatingCharacterObject3D(vector2);
        }

        public void PowerAttack(float runningDistance, Vector3 runningDirection)
        {
            if(movementRigidbodyV2.GetVelocity() > 0.1f)
                movementRigidbodyV2.AddForce(runningDirection, runningDistance);
        }

        public void QuickAttack(float runningDistance, Vector3 runningDirection)
        {
            if(movementRigidbodyV2.GetVelocity() > 0.1f)
                movementRigidbodyV2.AddForce(runningDirection, runningDistance);
        }

        public void CanMove()
        {
            movementRigidbodyV2.CanMove();
        }

        private void Update()
        {
            _model3DInstance.transform.position = transform.position;
        }
        
        private void RotatingCharacterObject3D(Vector2 input)
        {
            if (_localIsTarget) return;
            var transformForward = transform.TransformDirection(new Vector3(input.x, 0, input.y));
            var eulerAnglesY = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + transform.eulerAngles.y;
            Debug.Log($"transformForward {transformForward}");
            float reference = 0;
            var smoothDampAngle = Mathf.SmoothDampAngle(_model3DInstance.transform.eulerAngles.y, eulerAnglesY, ref reference, forceRotation);
            _model3DInstance.transform.localRotation = Quaternion.Euler(0f, smoothDampAngle, 0);
        }
    }
}
