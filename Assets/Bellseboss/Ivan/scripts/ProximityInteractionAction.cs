using UnityEngine;
using UnityEngine.Events;

namespace DoomlingsGame.SFX
{
    public class ProximityInteractionAction : MonoBehaviour
    {
        [SerializeField] private string objectTag = "Player";
        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onExit;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(objectTag))
            {
                onEnter.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(objectTag))
            {
                onExit.Invoke();
            }
        }
    }
}