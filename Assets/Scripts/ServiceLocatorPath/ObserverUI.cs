using View.Characters;
using View.Installers;
using View.UI;

namespace ServiceLocatorPath
{
    public class ObserverUI : IObserverUI
    {
        private readonly UiController _ui;
        private float totalLife;

        public ObserverUI(UiController ui)
        {
            _ui = ui;
        }

        public void Observer(PlayerCharacter character)
        {
            character.OnEnterDamageEvent += CharacterOnEnterDamageEvent;
            totalLife = character.GetLife();
            _ui.DefaultValue();
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