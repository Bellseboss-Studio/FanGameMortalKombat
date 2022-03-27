using UnityEngine;

namespace AudioStatePattern
{
    public class CombatState : MixerState
    {
        private MixerController m_MixerController;

        public override void Handle(MixerController controller)
        {
            if (!m_MixerController)
            {
                m_MixerController = controller;
            }

            m_MixerController.CurrentMixerSnapshot = m_MixerController.CombatMixerSnapshot;
            m_MixerController.CurrentMixerSnapshot.TransitionTo(TransitionTime);
        }
    }
}