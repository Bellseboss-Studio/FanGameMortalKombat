using System.Collections;
using System.Collections.Generic;
using MenuUI.SystemOfExtras;
using ServiceLocatorPath;
using UnityEngine;

public class ControllerOfUiForExtras : MonoBehaviour
{
    [SerializeField] private List<ContainerOfExtra> containers;

    public void LoadData()
    {
        
        var index = 0;
        foreach (var extra in ServiceLocator.Instance.GetService<ICatalog>().GetListOfExtras)
        {
            containers[index].Add(extra);
            index++;
        }

        foreach (var container in containers)
        {
            container.Configure();
        }
    }
    public void SaveData()
    {
        ServiceLocator.Instance.GetService<ICatalog>().AddExtra(new Extra());
        ServiceLocator.Instance.GetService<ICatalog>().SaveData();
    }

    public void HideExtra()
    {
        GetComponent<Animator>().SetBool("show", false);
    }
}

//Imagen, texto, video, audio