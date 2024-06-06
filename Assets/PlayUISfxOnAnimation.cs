using System.Reflection;
using System.Collections.Generic;
using FMODUnity;
using MortalKombat.Audio;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayUISfxOnAnimation : StateMachineBehaviour
{
    
    [SerializeField] private UISoundList[] m_SoundsToPlay;
    private IFmodManager m_FmodManager;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (UISoundList sfx in m_SoundsToPlay)
        {
            m_FmodManager = new FmodManagerUI();
            m_FmodManager.PlaySfx(sfx);
        }
    }
}
