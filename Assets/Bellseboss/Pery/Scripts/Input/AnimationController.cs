using System;
using System.Collections;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    internal class AnimationController : MonoBehaviour
    {
        public Action OnFinishAnimationDamage;
        [SerializeField] private string velocityName, horizontalName, verticalName, targetName, punchName, kickName;
        private Animator _animator;
        private IAnimationController _animationController;
        private bool _isFinishAnimation;

        public void Configure( Animator animator, IAnimationController animationController)
        {
            _animator = animator;
            _animationController = animationController;
        }
        
        public void Movement(float velocity, float speed)
        {
            //Debug.Log($"AnimationController: Movement: {velocity}");
            _animator.SetFloat(velocityName, velocity);
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

        public void ActivateTrigger(string animationTrigger)
        {
            Die(animationTrigger);
        }

        public void TakeDamage(bool isQuickAttack, int numberOfCombos)
        {
            var nameOfAnimation = isQuickAttack ? "q" : "p";
            nameOfAnimation += numberOfCombos;
            StartCoroutine(FinishTimeAnimation(nameOfAnimation));
            _animator.Play(nameOfAnimation);
        }

        private IEnumerator FinishTimeAnimation(string nameOfAnimation)
        {
            var animationClip = _animator.runtimeAnimatorController.animationClips;
            foreach (var clip in animationClip)
            {
                if (clip.name == nameOfAnimation)
                {
                    yield return new WaitForSeconds(clip.length);
                    OnFinishAnimationDamage?.Invoke();
                    break;
                }
            }
            
        }

        public void Die(string animationTrigger)
        {
            _animator.SetTrigger(animationTrigger);
        }

        public void SetTrigger(string paramName)
        {
            _animator.SetTrigger(paramName);
        }
    }
}