using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bellseboss.Pery.Scripts.Input
{
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField] private float powerAttackTime;
        [SerializeField] private float quickTime;
        [SerializeField] private float powerAttackTimeDelta;
        [SerializeField] private float quickTimeDelta;
        public bool CanPunch => _canPunch;
        public bool CanKick => _canKick;
        
        private ICombatSystem _combatSystem;
        private bool _canPunch, _canKick;

        public void Configure(ICombatSystem combatSystem)
        {
            _combatSystem = combatSystem;
            _canKick = true;
            _canPunch = true;
        }
        
        public void Kick()
        {
            _canKick = false;
        }
        
        public void Punch()
        {
            _canPunch = false;
        }

        private void Update()
        {
            if (_canPunch == false)
            {
                powerAttackTimeDelta += Time.deltaTime;
                if (powerAttackTimeDelta >= powerAttackTime)
                {
                    powerAttackTimeDelta = 0;
                    _canPunch = true;
                }
            }
            
            if (_canKick == false)
            {
                quickTimeDelta += Time.deltaTime;
                if (quickTimeDelta >= quickTime)
                {
                    quickTimeDelta = 0;
                    _canKick = true;
                }
            }
        }
    }
}