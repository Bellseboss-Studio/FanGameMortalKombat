using Cinemachine;
using InputSystemCustom;
using UnityEngine;

namespace CharacterCustom
{
    public abstract class Character: MonoBehaviour
    {
        [SerializeField] protected string id;
        [SerializeField] protected GameObject model3D;
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected float speedGlobal;
        [SerializeField] protected float velocityOfAttack;
        [SerializeField] protected float life;
        protected float speed;
        protected float forceRotation;
        protected InputCustom _inputCustom;
        public delegate void OnInputChanged(Vector2 input);
        public delegate void OnInputButton();
        public OnInputChanged OnInputChangedExtend;
        public OnInputChanged OnCameraMovementExtend;
        public OnInputButton OnLeftShitOn;
        public OnInputButton OnLeftShitOff;
        private Vector2 _inputValue;
        protected Vector3 movementPlayer;
        private Transform cameraForward;
        public delegate void OnEnterDamage(float damage);
        public event OnEnterDamage OnEnterDamageEvent;

        public string Id => id;
        public GameObject Model3D => model3D;

        protected virtual void Start()
        {
            Instantiate(model3D, transform);
        }
        protected void Update()
        {
            var rbVelocity = movementPlayer * (Time.deltaTime * speedGlobal);
            rbVelocity.y = rb.velocity.y;
            rb.velocity = rbVelocity;
        }

        private void Rotating (float horizontal, float vertical)
        {
            var targetDirection = new Vector3(horizontal, 0f, vertical);
            var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            var newRotation = Quaternion.Lerp(rb.rotation, targetRotation, forceRotation * Time.deltaTime);
            rb.MoveRotation(newRotation);
        }
        
        private void Rotating (float angle)
        {
            var targetRotation = Quaternion.Euler(0, angle, 0);;
            transform.rotation = targetRotation;
        }

        public void Configure(InputCustom inputCustom)
        {
            _inputCustom = inputCustom;
            cameraForward = transform;
            ConfigureExplicit();
        }

        protected abstract void ConfigureExplicit();

        public InputCustom GetInput()
        {
            return _inputCustom;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void Move(Vector3 movementVector2)
        {
            movementPlayer = movementVector2;
        }

        public void CameraMovement(Vector2 input)
        {
            _inputValue = input;
        }

        public void SetCameraForward(Transform cameraTransform)
        {
            cameraForward = cameraTransform;
        }

        public void NormalMotionInCamera()
        {
            if (cameraForward.gameObject.TryGetComponent<CinemachineFreeLook>(out var cinemachineFreeLook))
            {
                cinemachineFreeLook.m_XAxis.m_MaxSpeed = 300;    
                cinemachineFreeLook.m_YAxis.m_MaxSpeed = 1;    
            }
            else
            {
                Debug.Log("Es nulo");
            }
        }

        public void SlowMotionInCamera()
        {
            if (cameraForward.gameObject.TryGetComponent<CinemachineFreeLook>(out var cinemachineFreeLook))
            {
                cinemachineFreeLook.m_XAxis.m_MaxSpeed = 50;
                cinemachineFreeLook.m_YAxis.m_MaxSpeed = .3f;
            }
            else
            {
                Debug.Log("Es nulo");
            }
        }

        public void ApplyDamage(float damage)
        {
            life -= damage;
            Debug.Log($"life is {life}");
            OnEnterDamageEvent?.Invoke(damage);
            
        }
    }
}