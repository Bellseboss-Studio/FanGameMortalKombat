using UnityEngine;

namespace AudioStatePattern
{
    public class MenuState: MixerState
    {
        private MixerController m_MixerController;
        public override void Handle(MixerController controller)
        {
            if (!m_MixerController)
            {
                m_MixerController = controller;
            }

            m_MixerController.CurrentMixerSnapshot = m_MixerController.MenuMixerSnapshot;
            m_MixerController.CurrentMixerSnapshot.TransitionTo(TransitionTime);
        }
    }
}