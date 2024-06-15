using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace MortalKombat.Audio
{
    public class FmodManagerDialogs : IFmodManager
    {
        private const string m_UiFolder = "event:/UI/";
        private EventInstance m_EventInstance;
        
        private StatusFmod m_StatusFmod;
        
        public StatusFmod StatusFmod => m_StatusFmod;
        
        public FmodManagerDialogs()
        {
            m_EventInstance = RuntimeManager.CreateInstance(m_UiFolder);

            m_EventInstance.setCallback(CallbackMethod);
        }

        private FMOD.RESULT CallbackMethod(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr eventInstance, IntPtr parameters)
        {
            Debug.Log($"CallbackMethod: {type}");
            if (type == FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED)
            {
                // Code to execute when the SFX stops
                Debug.Log("SFX has stopped.");
            }

            return FMOD.RESULT.OK;
        }
        public void PlaySfx(UISoundList sfxToPlay)
        {
            m_EventInstance.start();
            m_EventInstance.release();
        }
        
        public float GetSoundLength(UISoundList sfxToPlay)
        {
            return 5;
        }

        public PLAYBACK_STATE GetStatus()
        {
            m_EventInstance.getPlaybackState(out var state);
            return state;
        }
    }

    public enum StatusFmod
    {
        PLAYING,
        STOPPED
    }
}