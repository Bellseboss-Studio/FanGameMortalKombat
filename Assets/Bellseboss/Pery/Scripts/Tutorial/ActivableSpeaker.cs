using System;
using System.Collections;
using FMOD.Studio;
using MortalKombat.Audio;
using UnityEngine;

public class ActivableSpeaker : ActivableTutorial
{
    [SerializeField] private NarrationDialog id;
    private FmodManagerDialogs m_FmodManager;
    private bool m_IsPlaying = false;

    private void Start()
    {
        m_FmodManager = new FmodManagerDialogs();
    }

    public override void Activate()
    {
        if(IsFinished) return;
        //Send an action to fmod to play the sound
        m_FmodManager.PlaySfx((UISoundList) id);
        m_IsPlaying = true;
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


public enum NarrationDialog
{
    UI_MouseHover,
    DX_TUTORIAL_ROOM_2,
    DX_TUTORIAL_ROOM_3,
    DX_TUTORIAL_ROOM_4,
}