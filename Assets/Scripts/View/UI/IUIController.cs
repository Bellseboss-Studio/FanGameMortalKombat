namespace View.UI
{
    public interface IUIController
    {
        void DefaultValue();
        void SetEnergyValue(float energy);
        float GetSliderValue();
        void SetSliderValue(float totalLifePercent);
    }
}