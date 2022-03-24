using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public class UiController : MonoBehaviour, IUIController
    {
        [SerializeField] private Slider sliderLife;

        public virtual void SetEnergyValue(float energy)
        {
            
        }

        public float GetSliderValue()
        {
            return sliderLife.value;
        }

        public void SetSliderValue(float totalLife)
        {
            sliderLife.value = totalLife;
        }

        public virtual void DefaultValue()
        {
            sliderLife.value = 1;
        }
        
    }
}