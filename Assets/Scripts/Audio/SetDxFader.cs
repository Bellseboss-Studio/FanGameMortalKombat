using UnityEngine;

public class SetDxFader : SetFaderLevel
{

    public override void SetVolume(float sliderValue)
    {
        m_Mixer.SetFloat(m_FaderToControl, Mathf.Log10(sliderValue) * 20);
        AudioManager.DxFaderValue = m_Slider.value;
    }

    private void OnEnable()
    {
        m_Slider.value = AudioManager.DxFaderValue;
    }
}