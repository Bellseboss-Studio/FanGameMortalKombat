namespace AudioStatePattern
{
    public class MixerStateContext
    {
        public IMixerState CurrentState
        {
            get;
            set;
        }
        private readonly MixerController m_MixerController;
        public MixerStateContext(MixerController mixerController)
        {
            m_MixerController = mixerController;
        }

        public void Transition()
        {
            CurrentState.Handle(m_MixerController);
        }

        public void Transition(IMixerState state)
        {
            CurrentState = state;
            CurrentState.Handle(m_MixerController);
        }
    }
}