using UnityEngine;

namespace ServiceLocatorPath
{
    public class Installer : MonoBehaviour
    {
        private void Awake()
        {
            var observerZonesGod = new ObserverZoneGod();
            ServiceLocator.Instance.RegisterService<IGodObserver>(observerZonesGod);
        }
    }
}