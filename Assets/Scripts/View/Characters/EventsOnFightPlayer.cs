using System;
using CharacterCustom;
using ServiceLocatorPath;
using UnityEngine;
using View.UI;

namespace View.Characters
{
    public class EventsOnFightPlayer : MonoBehaviour
    {
        [SerializeField] private string type;
        private Character _playerCharacter;

        public void Configuration(Character playerCharacter)
        {
            _playerCharacter = playerCharacter;
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger");
            if(other.TryGetComponent<EnemyDefaultCharacter>(out var enemy))
            {
                var damange = 0f;
                switch (type)
                {
                    case "kick":
                        damange = _playerCharacter.GetDamageForKick();
                        break;
                    case "punch":
                        damange = _playerCharacter.GetDamageForPunch();
                        break;
                }
                enemy.ApplyDamage(damange);
            }
        }
    }
}
