using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;


namespace MortalKombat.Audio
{
    public class FmodManagerUI : IFmodManager
    {
        public void PlaySfx(EventReference sfxToPlay)
        {
            RuntimeManager.PlayOneShot(sfxToPlay);
        }
    }
}
