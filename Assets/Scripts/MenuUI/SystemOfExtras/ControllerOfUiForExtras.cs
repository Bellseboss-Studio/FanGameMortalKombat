using System.Collections;
using System.Collections.Generic;
using MenuUI.SystemOfExtras;
using UnityEngine;

public class ControllerOfUiForExtras : MonoBehaviour
{
    private Catalog catalogOfExtras;
    [SerializeField] private List<ContainerOfExtra> containers;

    private void Start()
    {
        /*
        catalogOfExtras = new Catalog(new PlayerPrefDataContainer());

        var index = 0;
        foreach (var extra in catalogOfExtras.GetListOfExtras)
        {
            containers[index].Add(extra);
            index++;
        }

        foreach (var container in containers)
        {
            container.Configure();
        }
        */
    }

    public void SaveData()
    {
        catalogOfExtras.AddExtra(new Extra());
        catalogOfExtras.SaveData();
    }

    public void HideExtra()
    {
        GetComponent<Animator>().SetBool("show", false);
    }
}

//Imagen, texto, video, audio