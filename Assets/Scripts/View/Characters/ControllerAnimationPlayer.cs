using UnityEngine;

namespace View.Characters
{
    public class ControllerAnimationPlayer : MonoBehaviour
    {
        [SerializeField] private EventsOnFightPlayer punch, kick;
        private Character _character;

        public void Configurate(Character character)
        {
            _character = character;
            punch.Configuration(character);
            kick.Configuration(character);
            punch.OnAddingEnergyToCharacter += _character.AddEnergy;
            kick.OnAddingEnergyToCharacter += _character.AddEnergy;
        }

        private void FinishedAnimator()
        {
            _character.OnFinishedAnimatorFight?.Invoke();
        }
        
        private void FinishedAnimatorDamage()
        {
            _character.OnFinishedAnimatorDamage?.Invoke();
        }
    }
}
