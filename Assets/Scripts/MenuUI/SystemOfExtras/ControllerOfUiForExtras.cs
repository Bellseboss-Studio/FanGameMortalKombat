using System.Collections.Generic;
using UnityEngine;

public class ControllerOfUiForExtras : MonoBehaviour
{
    [SerializeField] private ContainerOfExtra containerOfExtraPrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private ExtraMediator mediator;
    private List<ContainerOfExtra> containerOfExtraInstantiates;

    public async void LoadData()
    {
        await ServiceLocator.Instance.GetService<ICatalog>().LoadDataCatalog();
        foreach (var extra in containerOfExtraInstantiates?.ToArray()!)
        {
            Destroy(extra.gameObject);
        }
        containerOfExtraInstantiates = new List<ContainerOfExtra>();
        foreach (var extra in ServiceLocator.Instance.GetService<ICatalog>().GetListOfExtras)
        {
            var containerOfExtra = Instantiate(containerOfExtraPrefab, content.transform);
            containerOfExtra.Configure(extra);
            containerOfExtraInstantiates.Add(containerOfExtra);
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