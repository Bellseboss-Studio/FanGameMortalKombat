using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public abstract class SetFaderLevel : MonoBehaviour, ICheckDependencies
{
    [SerializeField] public Slider m_Slider;
    [SerializeField] public string m_FaderToControl;
    [SerializeField] public float m_StartingValue = 1f;
    [SerializeField] public float m_CurrentValue;

    [SerializeField] public AudioMixer m_Mixer;

    private void Awake()
    {
        CheckDependencies();

        m_Slider.minValue = 0.0001f;
        DontDestroyOnLoad(this.gameObject);
    }

    public void CheckDependencies()
    {
        if(m_Slider == null)
        {
            m_Slider = GetComponent<Slider>();
        }

        if(m_FaderToControl == null)
        {
            m_FaderToControl = this.gameObject.name.ToString();
        }
    }

    public virtual void SetVolume(float sliderValue)
    {
        m_Mixer.SetFloat(m_FaderToControl, Mathf.Log10(sliderValue) * 20);
        m_CurrentValue = m_Slider.value;
    }

}
