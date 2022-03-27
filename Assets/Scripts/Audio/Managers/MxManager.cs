using System.Collections;
using System.Collections.Generic;
using AudioStatePattern;
using Singleton;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Audio.Managers
{
    public class MxManager : Singleton<MxManager>
    {
    
        
        [SerializeField] private List<GameObject> m_MusicTracks = new List<GameObject>();
        private Dictionary<string, GameObject> m_MxTracks = new Dictionary<string, GameObject>(); //might be useful later
        private Transform[] m_Transforms;



        private void Start()
        {
            CollectAllGameObjects();
            ChangeSceneMx(GameStates.MainMenu); 
        }

        private void CollectAllGameObjects()
        {
            m_Transforms = GetComponentsInChildren<Transform>(gameObject);
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
                if (child.gameObject.CompareTag("MxPlayer"))
                {
                    m_MusicTracks.Add(child.gameObject);
                    m_MxTracks.Add(child.gameObject.name, child.gameObject);
                }
            }
        }

        public void ChangeSceneMx(GameStates gameState)
        {
            foreach (var gO in m_MusicTracks)
            {
                gO.SetActive(false);
            }
            StartCoroutine(MakeMxGoActive(gameState));
        }
        

        IEnumerator MakeMxGoActive(GameStates gameState)
        {
            yield return new WaitForSeconds(0.1f);
            int objectToActivate = (int)gameState;
            m_MusicTracks[objectToActivate].SetActive(true);
        }
    }
}