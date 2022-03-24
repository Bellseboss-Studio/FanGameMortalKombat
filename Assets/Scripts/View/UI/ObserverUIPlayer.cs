using UnityEngine;
using UnityEngine.UI;
using View.UI;

public class ObserverUIPlayer : UiController 
{
    [SerializeField] private Image sliderEnergy;
    
    public override void DefaultValue()
    {
        base.DefaultValue();
        sliderEnergy.fillAmount = 0;
    }

    public override void SetEnergyValue(float totalLife)
    {
        Debug.Log("Add energy");
        sliderEnergy.fillAmount = totalLife;
    }
}