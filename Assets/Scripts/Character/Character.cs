using InputSystemCustom;
using UnityEngine;

namespace CharacterCustom
{
    public abstract class Character: MonoBehaviour
    {
        [SerializeField] protected string id;
        [SerializeField] protected GameObject model3D;
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected float speed;
        [SerializeField] protected float forceRotation;
        protected InputCustom _inputCustom;
        public delegate void OnInputChanged(Vector2 input);
        public OnInputChanged OnInputChangedExtend;

        public string Id => id;
        public GameObject Model3D => model3D;

        protected virtual void Start()
        {
            Instantiate(model3D, transform);
        }
        protected void Update()
        {
            rb.velocity = new Vector3(_inputCustom.GetDirection().x, rb.velocity.y, _inputCustom.GetDirection().y) * (Time.deltaTime * speed);
            Rotating(_inputCustom.GetLasPosition().x, _inputCustom.GetLasPosition().y);
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
    }
}