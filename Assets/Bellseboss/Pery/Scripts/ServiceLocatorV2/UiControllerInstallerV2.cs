using ServiceLocatorPath;
using UnityEngine;
using View.Installers;

public class UiControllerInstallerV2 : MonoBehaviour
{
    [SerializeField] private ObserverUIPlayer ui;
    // Start is called before the first frame update
    void Awake()
    {
        var observer = new ObserverUI(ui);
        ServiceLocator.Instance.RegisterService<IObserverUI>(observer);
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.UnregisterService<IObserverUI>();
    }
}
