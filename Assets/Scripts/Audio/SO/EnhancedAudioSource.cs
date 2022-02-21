using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnhancedAudioSource : MonoBehaviour
{
    [SerializeField] List<AudioClip> Sfx;
    [SerializeField] AudioSource As;
    [SerializeField] [Range(0f, 0.5f)] float pitchVariation;
    [SerializeField] [Range(0f, 0.5f)] float volumeVariation;
    [SerializeField] float maxVolume = 0.7f;

    private void Awake()
    {
        CheckDependencies();
        As.playOnAwake = false;
    }

    private void OnEnable()
    {
        int AudioClipIndx = Random.Range(0, Sfx.Count);
        As.clip = Sfx[AudioClipIndx];
        As.pitch = Random.Range((1f - pitchVariation), (1f + pitchVariation));
        As.volume = Random.Range((maxVolume - pitchVariation), maxVolume);
        As.Play();
    }

    private void CheckDependencies()
    {
        if (As == null)
        {
            As = GetComponent<AudioSource>();
        }
    }
}

