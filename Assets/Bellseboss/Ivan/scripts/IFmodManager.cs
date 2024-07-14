using FMODUnity;

namespace MortalKombat.Audio
{
    public interface IFmodManager
    {
        void PlaySfx(UISoundList sfxToPlay);
    }
    
    public interface IFmodManagerDx
    {
        void PlaySfx();
    }
}