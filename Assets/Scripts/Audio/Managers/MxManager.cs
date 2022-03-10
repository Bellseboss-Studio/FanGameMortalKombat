using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MxManager : Singleton<MxManager>
{
    
    [SerializeField] [Range (0.1f, 1f)] private float m_TransitionTime = 0.5f;
    [SerializeField] private List<GameObject> m_MusicTracks = new List<GameObject>();
    [SerializeField] private List<AudioMixerSnapshot> m_MixesSnapshots;
    [SerializeField] private AudioMixer mixer;
    private Dictionary<string, GameObject> m_MxTracks = new Dictionary<string, GameObject>(); //might be useful later
    Transform[] transforms;
    [SerializeField] private int m_CurrentState;


    private void Start()
    {
        CollectAllGameObjects();
        m_CurrentState = SceneManager.GetActiveScene().buildIndex;
        PlayMusicState(); 
    }

    private void CollectAllGameObjects()
    {
        transforms = GetComponentsInChildren<Transform>(gameObject);
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

    public void PlayMusicState()
    {
        foreach (var gO in m_MusicTracks)
        {
            gO.SetActive(false);
        }
        StartCoroutine(MakeMxGoActive());
    }

    IEnumerator MakeMxGoActive()
    {
        yield return new WaitForSeconds(0.1f);
        int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        m_MusicTracks[sceneBuildIndex].SetActive(true);
        m_MixesSnapshots[sceneBuildIndex].TransitionTo(m_TransitionTime);
    }
}