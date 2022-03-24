using ServiceLocatorPath;
using View;
using View.Characters;
using View.Installers;
using View.UI;

namespace ServiceLocatorPath
{
    public class ObserverUI : IObserverUI
    {
        private readonly IUIController _ui;
        private float totalLife;

        public ObserverUI(IUIController ui)
        {
            _ui = ui;
        }

        public void Observer(Character character)
        {
            character.OnEnterDamageEvent += CharacterOnEnterDamageEvent;
            character.OnAddingEnergy += AddingEnergy;
            totalLife = character.GetLife();
            _ui.DefaultValue();
        }

        private void AddingEnergy(float energy)
        {
            _ui.SetEnergyValue(energy);
        }

        private void CharacterOnEnterDamageEvent(float damage)
        {
            var percentageDamage = damage / totalLife;
            var valueSlider = _ui.GetSliderValue();
            var totalLifePercent = valueSlider - percentageDamage;
            _ui.SetSliderValue(totalLifePercent);
        }
    }
}
