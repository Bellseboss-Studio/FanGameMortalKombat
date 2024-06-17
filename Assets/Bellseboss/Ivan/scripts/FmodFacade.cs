using FMOD.Studio;
using FMODUnity;

namespace MortalKombat.Audio
{
    public class FmodFacade
    {
        private string m_UiFolder;
        private EventInstance m_EventInstance;

        public FmodFacade(string eventDx, string dialogueToPlay)
        {
            m_UiFolder = eventDx;
            m_EventInstance = RuntimeManager.CreateInstance(m_UiFolder + dialogueToPlay);
        }

        public int PlayToGetMilliseconds()
        {
            m_EventInstance.start();
            m_EventInstance.release();
            m_EventInstance.getDescription(out var description);
            description.getLength(out var length);
            return length;
        }


        public static void PlayOneShot(string eventDx, string sfxToPlay)
        {
            RuntimeManager.PlayOneShot($"{eventDx}{sfxToPlay}");
        }
    }
}