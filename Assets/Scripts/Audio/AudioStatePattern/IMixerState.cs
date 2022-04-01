namespace AudioStatePattern
{
    public interface IMixerState
    {
        public void Handle(MixerController controller);
    }
}