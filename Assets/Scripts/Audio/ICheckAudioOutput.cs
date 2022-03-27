using UnityEngine.Audio;

namespace Audio
{
    public interface ICheckAudioOutput
    {
        void CheckMixerGroup(AudioMixerGroup audioMixerGroup);
    }
}