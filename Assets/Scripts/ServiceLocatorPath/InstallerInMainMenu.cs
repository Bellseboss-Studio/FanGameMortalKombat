using System;
using System.Collections;
using System.Collections.Generic;
using MenuUI.SystemOfExtras;
using ServiceLocatorPath;
using UnityEngine;

public class InstallerInMainMenu : MonoBehaviour
{
    private void Awake()
    {
        
        var playFab = new PlayFabCustom();
        ServiceLocator.Instance.RegisterService<ISaveData>(playFab);
        ServiceLocator.Instance.RegisterService<IPlayFabCustom>(playFab);
        var catalog = new Catalog(playFab);
        ServiceLocator.Instance.RegisterService<ICatalog>(catalog);
    }
}
