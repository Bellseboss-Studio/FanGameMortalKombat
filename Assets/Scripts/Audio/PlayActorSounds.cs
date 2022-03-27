using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class PlayActorSounds : MonoBehaviour, ICheckDependencies
    {
   
        [SerializeField] List <GameObject> footstepsSfx = new List<GameObject>();
        [SerializeField] TerrainChecker terrainChecker;
        Dictionary<string, GameObject> footstepsGameObjects = new Dictionary<string, GameObject>();

        private void Start()
        {
            CheckDependencies();
            foreach(GameObject go in footstepsSfx)
            {
                footstepsGameObjects.Add(go.name.ToString().ToLower(), go);
            }
        }

        public void PlayFootstepsSfx()
        {
            string nameOfGo = terrainChecker.CheckTerrainNow().ToLower();
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

        public void CheckDependencies()
        {
            if(terrainChecker == null)
            {
                terrainChecker = GetComponent<TerrainChecker>();
            }
        }
    }
}
