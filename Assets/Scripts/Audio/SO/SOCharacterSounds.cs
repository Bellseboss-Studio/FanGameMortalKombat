using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "Create Actor SFXs", menuName =
                "Create Sounds for Actor")]
public class SOCharacterSounds : ScriptableObject
{
    [SerializeField] public List<GameObject> footsteps = new List<GameObject>();
    //[SerializeField] public List<AudioClip> kick;
    //[SerializeField] public List<AudioClip> punch;
}
