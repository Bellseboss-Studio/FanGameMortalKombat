using UnityEngine;

namespace View.Characters
{
    public class CharacterAnimatorController : MonoBehaviour
    {
        [SerializeField] private string speedName, isTargetName, horizontalName, verticalName;
        private Character _character;
        private Animator _animator;

        public void Configurate(Character character)
        {
            _character = character;
            _animator = _character.GetAnimator();
            _animator.runtimeAnimatorController = _character.GetController();
        }
        
        public void Movement(float speed, Vector2 direction)
        {
            _animator.SetFloat(speedName, speed);
            //_animator.SetFloat(horizontalName, direction.x);
            //_animator.SetFloat(verticalName, direction.y);
        }
        
        public void IsTarget(bool isTarget)
        {
            _animator.SetBool(isTargetName, isTarget);
        }

        public void Damage()
        {
            
        }

        public void PauseGame(bool ispause)
        {
            
        }
    }
}