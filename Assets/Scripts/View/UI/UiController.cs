using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public class UiController : MonoBehaviour, IUIController
    {
        [SerializeField] private Slider sliderLife, sliderSecondLife;

        public virtual void SetEnergyValue(float energyToAdd)
        {
            
        }

        public float GetSliderValue()
        {
            return sliderLife.value;
        }

        public void SetSliderValue(float totalLife)
        {
            sliderLife.value = totalLife;
            StartCoroutine(SetAsyncSliderValue(totalLife, 2));
        }
        
        private IEnumerator SetAsyncSliderValue(float totalLife, float timeToWait = 1)
        {
            //create a loop to update the slider value in total x seconds
            float elapsedTime = 0;
            while (elapsedTime < timeToWait)
            {
                sliderSecondLife.value = Mathf.Lerp(sliderSecondLife.value, totalLife, (elapsedTime / timeToWait));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public virtual void DefaultValue()
        {
            sliderLife.value = 1;
        }
        
    }
}