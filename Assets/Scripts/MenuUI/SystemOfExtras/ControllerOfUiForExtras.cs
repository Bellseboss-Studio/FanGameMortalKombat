using System.Collections.Generic;
using UnityEngine;

public class ControllerOfUiForExtras : MonoBehaviour
{
    [SerializeField] private List<ContainerOfExtra> containers;
    [SerializeField] private Animator animator;

    public async void LoadData()
    {
        await ServiceLocator.Instance.GetService<ICatalog>().LoadDataCatalog();
        foreach (var container in containers)
        {
            container.Clean();
        }
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

    public void CreateItem()
    {
        ServiceLocator.Instance.GetService<ICatalog>().SaveData();
    }
}

//Imagen, texto, video, audio