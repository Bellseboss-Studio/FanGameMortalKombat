using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayWwiseEvent : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event m_FootSteps;

    
    public void PlaySoundFromAnimation()
    {
        m_FootSteps.Post(gameObject);
    }
}
