using UnityEngine;

namespace AudioStatePattern
{
    public abstract class MixerState : MonoBehaviour, IMixerState
    {
        private MixerController m_MixerController;
        private float m_TransitionTime = 2f;

        public float TransitionTime
        {
            get
            {
                return m_TransitionTime;
            }
            set
            {
                m_TransitionTime = value;
            }
        }

        public virtual void Handle(MixerController controller)
        {
        }
    }
}