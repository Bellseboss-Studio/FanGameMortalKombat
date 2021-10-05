using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenuUI.SystemOfExtras;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace ServiceLocatorPath
{
    public class PlayFabCustom : ISaveData, IPlayFabCustom
    {
        private SystemInfoCustom systemInfoCustom;
        private IsCreated isCreatedPlayer;
        private string _playerId;
        private List<IExtra> inventary;

        public PlayFabCustom()
        {
            Login();
        }

        private void Login()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
                /*
                Please change the titleId below to your own titleId from PlayFab Game Manager.
                If you have already set the value in the Editor Extensions, this can be skipped.
                */
                PlayFabSettings.staticSettings.TitleId = "42";
            }
            var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void CreatedPlayer()
        {
        
            GetTitleDataRequest request = new GetTitleDataRequest()
            {
                Keys = new List<string>(){"InitialUserData"}
            };
            PlayFabClientAPI.GetTitleData(request, (defaultData) =>
            {
                var initialUserData = JsonUtility.FromJson<InitialUserData>(defaultData.Data["InitialUserData"]);
                PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>()
                    {
                        {"isCreated",JsonUtility.ToJson(new IsCreated(){isCreated = true})},
                        {"SystemInfo",JsonUtility.ToJson(new SystemInfoCustom()
                        {
                            model = SystemInfo.deviceModel,
                            name = SystemInfo.deviceName,
                            os = SystemInfo.operatingSystem,
                            processor = SystemInfo.processorType,
                            graphicsDeviceName = SystemInfo.graphicsDeviceName
                        })}
                    }
                }, requestCreate =>
                {
                
                },OnLoginFailure);
            },OnLoginFailure);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your first API call.  :(");
            Debug.LogError("Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        }

        private void OnLoginSuccess(LoginResult result)
        {
            GetUserDataRequest requestCreated = new GetUserDataRequest(){Keys = new List<string>(){"isCreated","SystemInfo"}};
            _playerId = result.PlayFabId;
            PlayFabClientAPI.GetUserData(requestCreated, defaultResult =>
            {
                if (!defaultResult.Data.ContainsKey("isCreated"))
                {
                    CreatedPlayer();
                }
                else
                {
                    isCreatedPlayer = JsonUtility.FromJson<IsCreated>(defaultResult.Data["isCreated"].Value);
                    systemInfoCustom = JsonUtility.FromJson<SystemInfoCustom>(defaultResult.Data["SystemInfo"].Value);
                }
            },OnLoginFailure);
        }

        public async Task<bool> HasData()
        {
            var isRequestOk = false;
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
                /*
                Please change the titleId below to your own titleId from PlayFab Game Manager.
                If you have already set the value in the Editor Extensions, this can be skipped.
                */
                PlayFabSettings.staticSettings.TitleId = "42";
            }
            var requestL = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(requestL, result =>
            {
                var request = new GetUserInventoryRequest();
                PlayFabClientAPI.GetUserInventory(request, result =>
                {
                    Debug.Log($"Its items");
                    inventary = new List<IExtra>();
                    foreach (var instance in result.Inventory)
                    {
                        Debug.Log(instance.BundleParent);
                        GetCatalogItemsRequest catalog = new GetCatalogItemsRequest()
                        {
                            CatalogVersion = "Extras"
                        };
                        PlayFabClientAPI.GetCatalogItems(catalog, itemsResult =>
                        {
                            Extra extra = (from item in itemsResult.Catalog where item.ItemId == instance.ItemId select JsonUtility.FromJson<Extra>(item.CustomData)).FirstOrDefault();

                            switch (extra.type)
                            {
                                case "text":
                                    inventary.Add(new ImageComponentExtra(extra));
                                    break;
                                case "image":
                                    inventary.Add(new ImageComponentExtra(extra));
                                    break;
                            }
                            Debug.Log($"{instance.CustomData}");
                            isRequestOk = true;
                        },OnLoginFailure);
                    }
                }, error =>
                {
                    isRequestOk = true;
                });
            }, OnLoginFailure);
            while (!isRequestOk)
            {
                await Task.Delay(TimeSpan.FromSeconds(.3f));
            }
            return inventary.Count > 0;
        }

        public List<IExtra> CreateData()
        {
            return inventary;
        }

        public List<IExtra> LoadData()
        {
            return inventary;
        }

        public void SaveData(List<IExtra> listOfExtras)
        {
            throw new System.NotImplementedException();
        }

        public string GetPlayerId()
        {
            return _playerId;
        }
    }
}