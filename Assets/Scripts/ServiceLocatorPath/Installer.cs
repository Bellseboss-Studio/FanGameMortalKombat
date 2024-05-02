using MenuUI.SystemOfExtras;
using UnityEngine;
using View.Installers;
using View.UI;

namespace ServiceLocatorPath
{
    public class Installer : MonoBehaviour
    {
        [SerializeField] private ObserverUIPlayer ui;
        //[SerializeField] private PauseMenu pause;
        private void Awake()
        {
            if (FindObjectsOfType<Installer>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            //pause.Configuracion();
            //var observerZonesGod = new ObserverZoneGod();
            //ServiceLocator.Instance.RegisterService<IGodObserver>(observerZonesGod);
            var observer = new ObserverUI(ui);
            ServiceLocator.Instance.RegisterService<IObserverUI>(observer);
            //ServiceLocator.Instance.RegisterService<IPauseMainMenu>(pause);
            //Comentario para ejemplo
            DontDestroyOnLoad(gameObject);
        }
    }
}