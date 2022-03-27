using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class EnhancedAudioSource : MonoBehaviour, ICheckDependencies, ICheckAudioOutput
    {
        [SerializeField] protected List<AudioClip> m_Sfx;
        [SerializeField] protected AudioSource m_As;
        [SerializeField] protected AudioMixerGroup m_Output;
        [SerializeField] [Range(0f, 0.5f)] private float m_PitchVariation;
        [SerializeField] [Range(0f, 0.5f)] private float m_VolumeVariation;
        [SerializeField] private float m_MaxVolume = 0.7f;
        [SerializeField] private float m_Pitch = 1;
        [SerializeField] private bool m_PlayOnAwake = false;
        [SerializeField] protected bool m_Perdurable = false;
        [SerializeField] private bool m_HasWaitingTime = false;
        [SerializeField] private float m_WaitTime;
    
    
        private void Awake()
        {
            CheckDependencies();
            if (m_PlayOnAwake)
            {
                m_As.playOnAwake = true;  
            }
       
            m_As.pitch = m_Pitch;
        }

        protected virtual void OnEnable()
        {
            CheckMixerGroup(m_Output);
            m_As.outputAudioMixerGroup = m_Output;
            m_As.pitch = Random.Range((m_Pitch - m_PitchVariation), (m_Pitch + m_PitchVariation));
            m_As.volume = Random.Range((m_MaxVolume - m_VolumeVariation), m_MaxVolume);
            if (!m_Perdurable)
            {
                int AudioClipIndx = Random.Range(0, m_Sfx.Count);
                m_As.clip = m_Sfx[AudioClipIndx];
                m_As.PlayOneShot(m_As.clip);
            }
            else
            {
                StartCoroutine(ChangeClip());
            }
        
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void CheckDependencies()
        {
            if (m_As == null)
            {
                m_As = GetComponent<AudioSource>();
            }
        }

        public void CheckMixerGroup(AudioMixerGroup audioMixerGroup)
        {
            if (audioMixerGroup == null)
            {
                Debug.LogError($"Audio output for: {this.gameObject.name} is not assigned");
            }
        }

        protected IEnumerator ChangeClip()
        {
            int audioClipIndx = Random.Range(0, m_Sfx.Count);
            m_As.clip = m_Sfx[audioClipIndx];
            
           // UnityEditor.EditorGUIUtility.PingObject(m_As.clip);
       
            if (m_HasWaitingTime) 
            { 
                m_As.PlayOneShot(m_As.clip);
                m_WaitTime = m_As.clip.length + Random.Range(5f, 15f);
            }
            else if (!m_HasWaitingTime)
            {
                m_As.Play();
                m_WaitTime = m_As.clip.length;
            }
            yield return new WaitForSeconds(m_WaitTime);
            StartCoroutine(ChangeClip());
        }
    }
}
