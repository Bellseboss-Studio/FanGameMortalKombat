using System.Collections.Generic;
using System.Threading.Tasks;
using MenuUI.SystemOfExtras;
using UnityEngine;

public class PlayerPrefDataContainer : ISaveData
{
    public static string NAME_OF_DATA_SAVES = "dataSave";
    private string data;
    private Extra[] extraParse;

    public async Task<bool> HasData()
    {
        data = PlayerPrefs.GetString(NAME_OF_DATA_SAVES);
        if (data == "")
        {
            return false;
        }
        extraParse = JsonHelper.FromJson<Extra>(data);
        return extraParse.Length > 0;
    }

    public List<IExtra> CreateData()
    {
        return new List<IExtra>();
    }

    public List<IExtra> LoadData()
    {
        var list = new List<IExtra>();
        foreach (var extra in extraParse)
        {
            list.Add(new ImageComponentExtra(extra));
        }
        return list;
    }

    public void SaveData(List<IExtra> listOfExtras)
    {
        Extra[] lista = new Extra[listOfExtras.Count];
        var index = 0;
        foreach (var extra in lista)
        {
            lista[index] = new Extra(listOfExtras[index]);
            index++;
        }

        var stringJson = JsonHelper.ToJson(lista);
        Debug.Log(stringJson);
        PlayerPrefs.SetString(NAME_OF_DATA_SAVES, stringJson);
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
}