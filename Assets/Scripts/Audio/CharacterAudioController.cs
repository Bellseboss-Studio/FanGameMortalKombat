using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace MortalKombat.Audio
{
    public class CharacterAudioController : MonoBehaviour
    {
        [SerializeField] private StudioEventEmitter[] m_SfxEmitters;
        private Dictionary<string, StudioEventEmitter> m_SfxDictionary = new Dictionary<string, StudioEventEmitter>();
        private void Awake()
        {
            foreach (StudioEventEmitter sfxEmitter in m_SfxEmitters)
            {
                m_SfxDictionary.Add(sfxEmitter.name, sfxEmitter);
            }
        }
        
        public void PlaySfx(string sfxName)
        {
            if (m_SfxDictionary.ContainsKey(sfxName))
            {
                if (m_SfxDictionary[sfxName] == null)
                {
                    Debug.Log($"il ya un problème avec {this.gameObject}");
                    return;
                }
                
                m_SfxDictionary[sfxName].Play();
            }
        }
    }
}