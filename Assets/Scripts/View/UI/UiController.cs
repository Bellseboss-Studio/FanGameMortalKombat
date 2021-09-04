using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private Slider sliderLife;

        public float GetSliderValue()
        {
            return sliderLife.value;
        }

        public void SetSliderValue(float totalLife)
        {
            sliderLife.value = totalLife;
        }

        public void DefaultValue()
        {
            sliderLife.value = 1;
        }
    }
}
