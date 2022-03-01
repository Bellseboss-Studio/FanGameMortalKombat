using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMasterFader : SetFaderLevel
{
    public override void SetVolume(float sliderValue)
    {
        m_Mixer.SetFloat(m_FaderToControl, Mathf.Log10(sliderValue) * 20);
        AudioManager.MasterFaderValue = m_Slider.value;
    }

    private void OnEnable()
    {
        m_Slider.value = AudioManager.MasterFaderValue;
    }
}
