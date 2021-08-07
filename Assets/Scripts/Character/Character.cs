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
        [SerializeField] protected float rotationGlobal;
        [SerializeField] protected GameObject pointToCamera, pointFarToCamera;
        [SerializeField] private int maxInX, maxInY;
        protected float speed;
        protected float forceRotation;
        protected InputCustom _inputCustom;
        public delegate void OnInputChanged(Vector2 input);
        public OnInputChanged OnInputChangedExtend;
        public OnInputChanged OnCameraMovementExtend;
        private Vector3 pointInicialToPointToFar;
        private Vector2 _inputValue;
        private Vector2 movementPlayer;
        private Transform cameraForward;

        public string Id => id;
        public GameObject Model3D => model3D;

        protected virtual void Start()
        {
            Instantiate(model3D, transform);
            pointInicialToPointToFar = pointFarToCamera.transform.localPosition;
        }
        protected void Update()
        {
            Vector3 targetDir = pointToCamera.transform.position - cameraForward.position;
            var forward = cameraForward.forward;
            var angleBetween = Vector3.Angle(forward, targetDir);
            var anglr = Vector3.Cross(forward, targetDir);
            if (anglr.y < 0)
            {
                angleBetween *= -1;
            }
            Rotating(angleBetween);

            var transformForward = transform.TransformDirection(new Vector3(movementPlayer.x,0,movementPlayer.y));
            Debug.Log($"transformForward  {transformForward}");
            
            var rbVelocity = transformForward * (Time.deltaTime * speedGlobal);
            rbVelocity.y = rb.velocity.y;
            rb.velocity = rbVelocity;
            //transform.Rotate(new Vector3(0, forceRotation * rotationGlobal, 0), Space.Self);//Este se tiene que hacer con respecto al vector hacia donde se esta movimiendo.
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

        public void Move(Vector2 movementVector2)
        {
            movementPlayer = movementVector2;
        }

        public Transform GetPointToCamera()
        {
            return pointToCamera.transform;
        }

        public Transform GetPointToGroupCamera()
        {
            return pointFarToCamera.transform;
        }

        public void CameraMovement(Vector2 input)
        {
            _inputValue = input;
        }

        public void SetCameraForward(Transform cameraTransform)
        {
            cameraForward = cameraTransform;
        }
    }
}