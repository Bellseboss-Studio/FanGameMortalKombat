using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class BehaviourOfJumpSystemWalls : MonoBehaviour, IBehaviourOfJumpSystem
    {
        private TeaTime _attack, _decay, _sustain, _release;

        public Action OnAttack { get; set; }
        public Action OnMidAir { get; set; }
        public Action OnSustain { get; set; }
        public Action OnRelease { get; set; }
        public Action OnEndJump { get; set; }

        public void Configure(Rigidbody _rigidbody)
        {
            //config all components
            _attack = this.tt().Pause().Add(() =>
            {
                //Debug.Log("JumpSystem: Attack");
            }).Add(() =>
            {
                OnAttack?.Invoke();
            }).Loop(loop =>
            {
                //Debug.Log("JumpSystem: Attack Loop");
            }).Add(()=>
            {
                _decay.Play();
            });
            
            _decay = this.tt().Pause().Add(() =>
            {
                OnMidAir?.Invoke();
            }).Loop(loop =>
            {
                //Debug.Log("JumpSystem: Decreasing Loop");
            }).Add(() =>
            {
                _sustain.Play();
            });
            
            _sustain = this.tt().Pause().Add(() =>
            {
                OnSustain?.Invoke();
            }).Loop(loop =>
            {
                //Debug.Log("JumpSystem: Sustain Loop");
            }).Add(() =>
            {
                _release.Play();
            });
            
            _release = this.tt().Pause().Add(() =>
            {
                OnRelease?.Invoke();
            }).Loop(loop =>
            {
                //Debug.Log("JumpSystem: Release Loop");
            }).Add(() =>
            {
                OnEndJump?.Invoke();
            });
        }
        
        public TeaTime GetAttack()
        {
            return _attack;
        }

        public TeaTime GetDecay()
        {
            return _decay;
        }

        public TeaTime GetSustain()
        {
            return _sustain;
        }

        public TeaTime GetRelease()
        {
            return _release;
        }

        public void StopAll()
        {
            _attack.Stop();
            _decay.Stop();
            _sustain.Stop();
            _release.Stop();
        }
    }
}