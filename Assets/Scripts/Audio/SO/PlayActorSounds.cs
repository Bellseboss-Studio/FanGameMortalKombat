using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayActorSounds : MonoBehaviour
{
   
    [SerializeField] List <GameObject> footstepsSfx = new List<GameObject>();
    Dictionary<string, GameObject> footstepsGameObjects = new Dictionary<string, GameObject>();

    private void Start()
    {
        foreach(GameObject go in footstepsSfx)
        {
            footstepsGameObjects.Add(go.name.ToString(), go);
        }
    }


    public void PlayFootstepsSfx()
    {
        string nameOfGo = GetComponent<TerrainChecker>().CheckTerrainNow();
        if (footstepsGameObjects.ContainsKey(nameOfGo))
            StartCoroutine(ActivateGO(footstepsGameObjects[nameOfGo]));
        else
            return;
    }


    IEnumerator ActivateGO(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(go.GetComponent<AudioSource>().clip.length);
        go.SetActive(false);
    }

}
