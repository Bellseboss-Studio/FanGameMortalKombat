using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK;

public class PlaySound : MonoBehaviour
{
    public AK.Wwise.Event m_SoundToPlay;

    public void PlaysoundFromAnimation()
    {
        
        m_SoundToPlay.Post(gameObject);
    }
}
