using MenuUI.SystemOfExtras;
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
            var playFab = new PlayFabCustom();
            ServiceLocator.Instance.RegisterService<ISaveData>(playFab);
            ServiceLocator.Instance.RegisterService<IPlayFabCustom>(playFab);
            var catalog = new Catalog(playFab);
            ServiceLocator.Instance.RegisterService<ICatalog>(catalog);
            //Comentario para ejemplo
        }
    }
}