using System.Collections;
using System.Collections.Generic;
using AudioStatePattern;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio.Managers
{
    public class SfxManager : MonoBehaviour
    {
    
        [SerializeField] Transform[] AudioObjects;
        private string m_CurrentState;
        private Dictionary<string, GameObject> m_AudioObjectsDic = new Dictionary<string, GameObject>();
    

        private void Start()
        {
            AddItemsToDictionary();
            ChangeSceneAmbient(GameStates.MainMenu);
        }
        //TODO deactivate go on scene change. Implement State Pattern???
        public void ChangeSceneAmbient(GameStates gameStates)
        {
            foreach (var audioObject in m_AudioObjectsDic)
            {
                m_AudioObjectsDic[audioObject.Key].SetActive(false);
            }

            
            string nameOfAudioObject = gameStates.ToString() + "Ambience";
            Debug.Log($"The current state is:{nameOfAudioObject}");
            m_AudioObjectsDic[nameOfAudioObject].SetActive(true);
        }

        void AddItemsToDictionary()
        {
            AudioObjects = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                AudioObjects[i] = transform.GetChild(i);
            }


            foreach (Transform t in AudioObjects)
            {
                m_AudioObjectsDic.Add(t.gameObject.name, t.gameObject);
            }
        }

        public void PlaySound(string goName)
        {
            StartCoroutine(ActivateNonLoopableGameObject(goName));
        }

        IEnumerator ActivateNonLoopableGameObject(string goName)
        {
            if(!m_AudioObjectsDic[goName].activeInHierarchy)
            {
                m_AudioObjectsDic[goName].SetActive(true);
                yield return new WaitForSeconds(m_AudioObjectsDic[goName].GetComponentInChildren<AudioSource>().clip.length);
                m_AudioObjectsDic[goName].SetActive(false);
            }
        }
    }
}

