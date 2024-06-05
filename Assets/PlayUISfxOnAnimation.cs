using System.Reflection;
using System.Collections.Generic;
using FMODUnity;
using MortalKombat.Audio;
using UnityEngine;

public class PlayUISfxOnAnimation : StateMachineBehaviour
{
    [SerializeField] private EventReference m_SfxToPlay;
    private IFmodManager m_FmodManager;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_FmodManager = new FmodManagerUI();
        m_FmodManager.PlaySfx(m_SfxToPlay);
    }
}
