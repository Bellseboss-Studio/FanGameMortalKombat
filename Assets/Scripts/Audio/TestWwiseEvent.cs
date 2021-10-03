using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWwiseEvent : MonoBehaviour
{

    [SerializeField] private AK.Wwise.Event m_SoundToPlay;

    public void PlaySound()
    {
        m_SoundToPlay.Post(gameObject);
    }
}
