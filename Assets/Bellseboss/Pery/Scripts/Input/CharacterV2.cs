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
            if (combatSystem.CanKick)
            {
                animationController.Kick();
                combatSystem.Kick();
            }
        }

        private void OnPunchEvent()
        {
            if(combatSystem.CanPunch)
            {
                animationController.Punch();
                combatSystem.Punch();
            }
        }

        private void OnTargetEvent(bool isTarget)
        {
            animationController.IsTarget(isTarget);
            movementRigidbodyV2.IsTarget(isTarget);
        }

        private void OnMove(Vector2 vector2)
        {
            movementRigidbodyV2.Direction(vector2);
            animationController.Movement(vector2, vector2.y);
            rotationCharacterV2.Direction(vector2);
        }
    }
}
