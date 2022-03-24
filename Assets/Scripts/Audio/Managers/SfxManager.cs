using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : Singleton<SfxManager>
{
    
    [SerializeField] Transform[] AudioObjects;
    public Dictionary<string, GameObject> AudioObjectsDic = new Dictionary<string, GameObject>();
    
    private void Start()
    {
        AudioObjects = new Transform[transform.childCount];
        
        for (int i = 0; i < transform.childCount; i++)
        {
            AudioObjects[i] = transform.GetChild(i);
        }


        foreach(Transform t in AudioObjects)
        {
            AudioObjectsDic.Add(t.gameObject.name, t.gameObject);
        }
    }


    public void PlaySound(string goName)
    {
        StartCoroutine(ActivateGameObject(goName));
    }

    IEnumerator ActivateGameObject(string goName)
    {
        if(!AudioObjectsDic[goName].activeInHierarchy)
        {
            AudioObjectsDic[goName].SetActive(true);
            yield return new WaitForSeconds(AudioObjectsDic[goName].GetComponentInChildren<AudioSource>().clip.length);
            AudioObjectsDic[goName].SetActive(false);
        } 
    }
}
