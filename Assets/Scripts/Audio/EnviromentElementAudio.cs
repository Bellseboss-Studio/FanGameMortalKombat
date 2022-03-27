using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class EnviromentElementAudio : EnhancedAudioSource
    {
        protected override void OnEnable()
        {
            CheckMixerGroup(m_Output);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!m_Perdurable)
                {
                    int AudioClipIndx = Random.Range(0, m_Sfx.Count);
                    m_As.clip = m_Sfx[AudioClipIndx];
                    m_As.PlayOneShot(m_As.clip);
                }
                else
                {
                    Debug.Log("Audio Is Playing");
                    StartCoroutine(ChangeClip());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StopAllCoroutines();
            }
        }
    }
}