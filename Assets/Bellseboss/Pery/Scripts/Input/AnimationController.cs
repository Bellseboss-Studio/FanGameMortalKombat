using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class AnimationController : MonoBehaviour
    {
        [SerializeField] private string velocityName, horizontalName, verticalName, targetName, punchName, kickName;
        private Animator _animator;
        private IAnimationController _animationController;

        public void Configure( Animator animator, IAnimationController animationController)
        {
            _animator = animator;
            _animationController = animationController;
        }
        
        public void Movement(Vector2 vector2, float speed)
        {
            _animator.SetFloat(velocityName, vector2.magnitude);
            //_animator.SetFloat(horizontalName, vector2.x);
            //_animator.SetFloat(velocityName, vector2.y);
        }

        public void IsTarget(bool isTarget)
        {
            _animator.SetBool(targetName, isTarget);
        }

        public void Punch()
        {
            _animator.SetTrigger(punchName);
        }

        public void Kick()
        {
            _animator.SetTrigger(kickName);
        }

        public void JumpJump()
        {
            _animator.SetTrigger("j");
        }

        public void JumpMidAir()
        {
            _animator.SetTrigger("j_mid_air");
        }

        public void JumpFall()
        {
            _animator.SetTrigger("j_fall");
        }

        public void JumpRecovery()
        {
            _animator.SetTrigger("j_recovery");
        }
    }
}