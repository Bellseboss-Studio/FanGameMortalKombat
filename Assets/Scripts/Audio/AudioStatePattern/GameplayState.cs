using UnityEngine;

namespace AudioStatePattern
{
    internal class GameplayState : MixerState
    {
        private MixerController m_MixerController;
        public override void Handle(MixerController controller)
        {
            if (!m_MixerController)
            {
                m_MixerController = controller;
            }

            m_MixerController.CurrentMixerSnapshot = m_MixerController.GameplayMixerSnapshot;
            m_MixerController.CurrentMixerSnapshot.TransitionTo(TransitionTime);
        }
    }
}