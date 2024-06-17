using System;
using System.Collections;
using FMOD.Studio;
using MortalKombat.Audio;
using PlayFab.Internal;
using UnityEngine;
using UnityEngine.Serialization;


namespace MortalKombat.Audio
{
    public class ActivatableSpeaker : ActivableTutorial
    {
        [SerializeField] private NarratorDialogues m_Id;
        private FmodManagerDialogues m_FmodManager;
        private bool m_IsPlaying = false;

        private void Start()
        {
            m_FmodManager = new FmodManagerDialogues(m_Id.ToString());
        }

        public override void Activate()
        {
            try
            {
                Debug.Log($"test {m_FmodManager.ToString()}");
                if (IsFinished) return;
                m_FmodManager.PlaySfx();
                m_IsPlaying = true;
            }
            catch
            {
                Debug.Log("error on Activatable Speaker");
            }
            
        }

        private void Update()
        {
            if(!m_IsPlaying) return;
            Debug.Log(m_FmodManager.GetStatus());
            if(m_FmodManager.GetStatus() == PLAYBACK_STATE.STOPPED)
            {
                Finish();
                m_IsPlaying = false;
            }
        }
    }
}



