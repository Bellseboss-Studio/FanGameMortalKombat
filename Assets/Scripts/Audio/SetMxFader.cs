﻿using Audio.Managers;
using UnityEngine;

namespace Audio
{
    public class SetMxFader : SetFaderLevel
    {
        private void Awake()
        {
            m_StartingValue = m_Slider.value;
        }
        public override void SetVolume(float sliderValue)
        {
            m_Mixer.SetFloat(m_FaderToControl, Mathf.Log10(sliderValue) * 20);
            UIAudioPerdurableValues.MxFaderValue = this.m_Slider.value;
        }

        private void OnEnable()
        {
            m_Slider.value = UIAudioPerdurableValues.MxFaderValue;
        }
    }
}
