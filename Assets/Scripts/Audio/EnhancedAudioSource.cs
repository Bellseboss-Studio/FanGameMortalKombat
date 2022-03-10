using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnhancedAudioSource : MonoBehaviour, ICheckDependencies
{
    [SerializeField] List<AudioClip> Sfx;
    [SerializeField] AudioSource As;
    [SerializeField] [Range(0f, 0.5f)] float pitchVariation;
    [SerializeField] [Range(0f, 0.5f)] float volumeVariation;
    [SerializeField] float maxVolume = 0.7f;
    [SerializeField] float pitch;

    private void Awake()
    {
        CheckDependencies();
        As.playOnAwake = false;
        pitch = As.pitch;
    }

    private void OnEnable()
    {
        int AudioClipIndx = Random.Range(0, Sfx.Count);
        As.clip = Sfx[AudioClipIndx];
        As.pitch = Random.Range((pitch - pitchVariation), (pitch + pitchVariation));
        As.volume = Random.Range((maxVolume - volumeVariation), maxVolume);
        As.PlayOneShot(As.clip);
    }

    public void CheckDependencies()
    {
        if (As == null)
        {
            As = GetComponent<AudioSource>();
        }
    }
}

