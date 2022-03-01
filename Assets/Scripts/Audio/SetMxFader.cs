using UnityEngine;

public class SetMxFader : SetFaderLevel
{

    public override void SetVolume(float sliderValue)
    {
        m_Mixer.SetFloat(m_FaderToControl, Mathf.Log10(sliderValue) * 20);
        AudioManager.MxFaderValue = m_Slider.value;
    }

    private void OnEnable()
    {
        m_Slider.value = AudioManager.MxFaderValue;
    }
}
