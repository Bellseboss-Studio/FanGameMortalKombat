using UnityEngine;
using UnityEngine.Audio;

namespace AudioStatePattern
{
    public class MixerController : MonoBehaviour
    {
        [SerializeField] private IMixerState m_MenuState, m_GameplayState, m_CombatState, m_PausedState, m_CurrentState;
        [SerializeField] private MixerStateContext m_MixerStateContext;

        [SerializeField] private AudioMixer AudioMixer;
        public AudioMixerSnapshot MenuMixerSnapshot;
        public AudioMixerSnapshot GameplayMixerSnapshot;
        public AudioMixerSnapshot CombatMixerSnapshot;
        public AudioMixerSnapshot PauseMixerSnapshot;
        public AudioMixerSnapshot CurrentMixerSnapshot
        {
            get;
            set;
        }
        
        private void Start()
        {
            m_MixerStateContext = new MixerStateContext(this);
            m_MenuState = gameObject.AddComponent<MenuState>();
            m_GameplayState = gameObject.AddComponent<GameplayState>();
            m_CombatState = gameObject.AddComponent<CombatState>();
            m_PausedState = gameObject.AddComponent<PausedState>();
            m_MixerStateContext.Transition(m_MenuState);
        }

        public void MenuAudio()
        { 
            m_MixerStateContext.Transition(m_MenuState);
        }

        public void GameplayAudio()
        {
            m_MixerStateContext.Transition(m_GameplayState);
        }
        
        public void CombatAudio()
        {
            m_MixerStateContext.Transition(m_CombatState);
        }
        public void PausedAudio()
        {
            m_MixerStateContext.Transition(m_PausedState);
        }
    }
}