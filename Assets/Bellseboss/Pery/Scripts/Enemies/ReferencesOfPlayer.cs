using UnityEngine;

public class ReferencesOfPlayer : MonoBehaviour
{
    [SerializeField] private GameObject referenceOfFatalitySystem;
    
    public GameObject ReferenceOfFatalitySystem => referenceOfFatalitySystem;
}