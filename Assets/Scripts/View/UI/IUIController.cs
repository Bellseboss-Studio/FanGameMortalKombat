namespace View.UI
{
    public interface IUIController
    {
        void DefaultValue();
        void SetEnergyValue(float energyToAdd);
        float GetSliderValue();
        void SetSliderValue(float totalLifePercent);
    }
}