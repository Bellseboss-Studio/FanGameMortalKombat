using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField] private float powerAttackTime;
        [SerializeField] private float quickAttackTime;
        [SerializeField] private float powerAttackTimeDelta;
        [SerializeField] private float quickAttackTimeDelta;
        [SerializeField] private float runningDistanceQuickAttack;
        [SerializeField] private float runningDistancePowerAttack;
        [SerializeField] private Vector3 runningDirectionQuickAttack;
        [SerializeField] private Vector3 runningDirectionPowerAttack;
        public bool CanQuickAttack => _canQuickAttack;
        public bool CanPowerAttack => _canPowerAttack;
        
        private ICombatSystem _combatSystemMediator;
        private bool _canQuickAttack, _canPowerAttack;

        public void Configure(ICombatSystem combatSystem)
        {
            _combatSystemMediator = combatSystem;
            _canPowerAttack = true;
            _canQuickAttack = true;
        }
        
        public void PowerAttack()
        {
            _canPowerAttack = false;
            _combatSystemMediator.PowerAttack(runningDistancePowerAttack, _combatSystemMediator.RotateToTarget(runningDirectionPowerAttack));
        }
        
        public void QuickAttack()
        {
            _canQuickAttack = false;
            _combatSystemMediator.QuickAttack(runningDistanceQuickAttack, _combatSystemMediator.RotateToTarget(runningDirectionQuickAttack));
        }

        private void Update()
        {
            if (_canQuickAttack == false)
            {
                powerAttackTimeDelta += Time.deltaTime;
                if (powerAttackTimeDelta >= powerAttackTime)
                {
                    powerAttackTimeDelta = 0;
                    _canQuickAttack = true;
                    _combatSystemMediator.CanMove();
                }
            }
            
            if (_canPowerAttack == false)
            {
                quickAttackTimeDelta += Time.deltaTime;
                if (quickAttackTimeDelta >= quickAttackTime)
                {
                    quickAttackTimeDelta = 0;
                    _canPowerAttack = true;
                    _combatSystemMediator.CanMove();
                }
            }
        }
    }
}