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

        public string Id => id;
        public GameObject Model3D => model3D;

        protected virtual void Start()
        {
            Instantiate(model3D, transform);
            pointInicialToPointToFar = pointFarToCamera.transform.localPosition;
        }
        protected void Update()
        {
            rb.velocity = transform.forward * (speed * Time.deltaTime * speedGlobal);
            transform.Rotate(new Vector3(0, forceRotation * rotationGlobal, 0), Space.Self);
            
            //camera
            var point = new Vector3(pointInicialToPointToFar.x + (maxInX * _inputValue.x), pointInicialToPointToFar.y + (maxInY * _inputValue.y), pointInicialToPointToFar.z);
            var moveTowards = Vector3.zero;
            if (_inputValue.sqrMagnitude > 0.1f)
            {
                //pointFarToCamera.transform.Translate(point);
                moveTowards = Vector3.MoveTowards(pointFarToCamera.transform.localPosition, point, 1);
            }
            else
            {
                //pointFarToCamera.transform.Translate(pointInicialToPointToFar);
                moveTowards = Vector3.MoveTowards(pointFarToCamera.transform.localPosition, pointInicialToPointToFar, 1);
            }
            
            pointFarToCamera.transform.localPosition = moveTowards;
        }

        private void Rotating (float horizontal, float vertical)
        {
            var targetDirection = new Vector3(horizontal, 0f, vertical);
            var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            var newRotation = Quaternion.Lerp(rb.rotation, targetRotation, forceRotation * Time.deltaTime);
            rb.MoveRotation(newRotation);
        }

        public void Configure(InputCustom inputCustom)
        {
            _inputCustom = inputCustom;
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

        public void Move(float _speed, float rotation)
        {
            speed = _speed;
            forceRotation = rotation;
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
    }
}