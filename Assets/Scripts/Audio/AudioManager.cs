using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] float test;

    private static float m_MasterFaderValue;
    public static float MasterFaderValue
    {
        get { return m_MasterFaderValue; }
        set { m_MasterFaderValue = value; }
    }

    private static float m_MxFaderValue;
    public static float MxFaderValue
    {
        get { return m_MxFaderValue; }
        set { m_MxFaderValue = value; }
    }

    private static float m_SfxFaderValue;
    public static float SfxFaderValue
    {
        get { return m_SfxFaderValue; }
        set { m_SfxFaderValue = value; }
    }

    private static float m_DxFaderValue;
    public static float DxFaderValue
    {
        get { return m_DxFaderValue; }
        set { m_DxFaderValue = value; }
    }


    private void Update()
    {
        test = m_MasterFaderValue;
    }
}

