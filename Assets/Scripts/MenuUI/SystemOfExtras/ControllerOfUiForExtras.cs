using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using V2;

public class ControllerOfUiForExtras : MonoBehaviour
{
    [SerializeField] private ContainerOfExtra containerOfExtraPrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private ExtraMediator mediator;
    [SerializeField] private Button backButton;
    [SerializeField] private Button backButtonToShowExtra;
    [SerializeField] private StepsConfig stepsConfig;
    private List<ContainerOfExtra> containerOfExtraInstantiates = new List<ContainerOfExtra>();

    public async void LoadData()
    {
        mediator.Configure(this);
        await ServiceLocator.Instance.GetService<ICatalog>().LoadDataCatalog();
        foreach (var extra in containerOfExtraInstantiates?.ToArray()!)
        {
            Destroy(extra.gameObject);
        }

        containerOfExtraInstantiates = new List<ContainerOfExtra>();
        foreach (var extra in ServiceLocator.Instance.GetService<ICatalog>().GetListOfExtras)
        {
            var containerOfExtra = Instantiate(containerOfExtraPrefab, content.transform);
            containerOfExtra.Configure(extra, mediator, backButtonToShowExtra);
            containerOfExtraInstantiates.Add(containerOfExtra);
        }

        backButton.navigation = new Navigation
        {
            mode = Navigation.Mode.Explicit,
            selectOnUp = containerOfExtraInstantiates[0].ButtonToAction,
            selectOnDown = containerOfExtraInstantiates[0].ButtonToAction,
        };

        //change the navigation of all elements
        var indexIntoContent = 0;
        foreach (var extra in containerOfExtraInstantiates)
        {
            var index = containerOfExtraInstantiates.IndexOf(extra);
            if (index == 0)
            {
                extra.ButtonToAction.navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = backButton,
                    selectOnRight = containerOfExtraInstantiates[index + 1].ButtonToAction,
                    selectOnDown = backButton,
                };
            }
            else if (index == containerOfExtraInstantiates.Count - 1)
            {
                extra.ButtonToAction.navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnLeft = containerOfExtraInstantiates[index - 1].ButtonToAction,
                    selectOnUp = backButton,
                    selectOnDown = backButton,
                };
            }
            else
            {
                extra.ButtonToAction.navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnLeft = containerOfExtraInstantiates[index - 1].ButtonToAction,
                    selectOnRight = containerOfExtraInstantiates[index + 1].ButtonToAction,
                    selectOnUp = backButton,
                    selectOnDown = backButton,
                };
            }
            extra.indexIntoContent = indexIntoContent;
            indexIntoContent++;
        }
        stepsConfig.Configure(indexIntoContent-1);
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

    public void SetIndex(int indexIntoContent)
    {
        stepsConfig.SetStep(indexIntoContent);
    }
}

//Imagen, texto, video, audio