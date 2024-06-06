using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;


namespace MortalKombat.Audio
{
    public class FmodManagerUI : IFmodManager
    {
        private const string m_UiFolder = "event:/UI/";
        public void PlaySfx(UISoundList sfxToPlay)
        {
            RuntimeManager.PlayOneShot($"{m_UiFolder}{sfxToPlay}");
        }
    }
}
