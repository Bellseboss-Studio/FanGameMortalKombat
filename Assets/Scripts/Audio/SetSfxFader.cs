using UnityEngine;

public class SetSfxFader : SetFaderLevel
{

    public override void SetVolume(float sliderValue)
    {
        m_Mixer.SetFloat(m_FaderToControl, Mathf.Log10(sliderValue) * 20);
        AudioManager.SfxFaderValue = m_Slider.value;
    }

    private void OnEnable()
    {
        m_Slider.value = AudioManager.SfxFaderValue;
    }
}
