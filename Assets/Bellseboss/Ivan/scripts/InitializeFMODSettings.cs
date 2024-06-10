using UnityEngine;
using FMODUnity;

namespace MortalKombat.Audio
{
    [System.Serializable]
    public class InitializeFMODSettings : MonoBehaviour
    {
        [SerializeField] private string busName;
        [Range(0f, 1f)]
        [SerializeField] private float defaultVolume = 0.75f;
        [SerializeField] private string volumePrefKey;

        void Start()
        {
            InitializeVolume();
        }

        private void InitializeVolume()
        {
            float volume = PlayerPrefs.GetFloat(volumePrefKey, defaultVolume);
            var bus = RuntimeManager.GetVCA(busName);
            bus.setVolume(volume);
        }

        private void OnApplicationQuit()
        {
            SaveVolumeSettings();
        }

        private void SaveVolumeSettings()
        {
            var bus = RuntimeManager.GetVCA(busName);
            bus.getVolume(out float volume);
            PlayerPrefs.SetFloat(volumePrefKey, volume);
        }
    }
}