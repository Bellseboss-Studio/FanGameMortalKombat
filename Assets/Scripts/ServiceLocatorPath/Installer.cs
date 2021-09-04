using UnityEngine;
using View.Installers;
using View.UI;

namespace ServiceLocatorPath
{
    public class Installer : MonoBehaviour
    {
        [SerializeField] private UiController ui;
        private void Awake()
        {
            var observerZonesGod = new ObserverZoneGod();
            ServiceLocator.Instance.RegisterService<IGodObserver>(observerZonesGod);
            var observer = new ObserverUI(ui);
            ServiceLocator.Instance.RegisterService<IObserverUI>(observer);
        }
    }
}