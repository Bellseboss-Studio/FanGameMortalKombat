using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class CombatSystem : MonoBehaviour
    {
        private ICombatSystem _combatSystem;

        public void Configure(ICombatSystem combatSystem)
        {
            _combatSystem = combatSystem;
        }
    }
}