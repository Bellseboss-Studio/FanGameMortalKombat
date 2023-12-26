using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class AnimationController : MonoBehaviour
    {
        [SerializeField] private string velocityName, horizontalName, verticalName, targetName;
        private Animator _animator;

        public void Configure( Animator animator)
        {
            _animator = animator;
        }
        
        public void Movement(Vector2 vector2, float speed)
        {
            _animator.SetFloat(velocityName, speed);
            //_animator.SetFloat(horizontalName, vector2.x);
            //_animator.SetFloat(velocityName, vector2.y);
        }

        public void IsTarget(bool isTarget)
        {
            _animator.SetBool(targetName, isTarget);
        }
    }
}