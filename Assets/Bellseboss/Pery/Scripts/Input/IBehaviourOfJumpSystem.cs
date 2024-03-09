using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public interface IBehaviourOfJumpSystem
    {
        public Action OnAttack { get; set; }
        public Action OnMidAir { get; set; }
        public Action OnSustain { get; set; }
        public Action OnRelease { get; set; }
        public Action OnEndJump { get; set; }
        public void Configure(Rigidbody _rigidbody);
        public TeaTime GetAttack();
        public TeaTime GetDecay();
        public TeaTime GetSustain();
        public TeaTime GetRelease();
        void StopAll();
    }
}