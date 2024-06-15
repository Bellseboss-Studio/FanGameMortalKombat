using System.Collections;
using UnityEngine;

public class ActivableSpeaker : ActivableTutorial
{
    [SerializeField] private string id;
    [SerializeField] private AudioSource audioSource;
    public override void Activate()
    {
        if(IsFinished) return;
        //Send an action to fmod to play the sound
        audioSource.Play();
        StartCoroutine(WaitForSound());
    }

    private IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Finish();
    }
}