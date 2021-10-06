using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MenuUI.SystemOfExtras;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

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
            Login(OnLoginSuccess, OnLoginFailure);
        }

        private void Login(Action<LoginResult> resultCallback, Action<PlayFabError> errorCallback)
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
                /*
                Please change the titleId below to your own titleId from PlayFab Game Manager.
                If you have already set the value in the Editor Extensions, this can be skipped.
                */
                PlayFabSettings.staticSettings.TitleId = "42";
            }
            Debug.Log($"SystemInfo.deviceUniqueIdentifier {SystemInfo.deviceUniqueIdentifier}");
            var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, resultCallback, errorCallback);
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
                    AddUserVirtualCurrencyRequest reqCurrenci = new AddUserVirtualCurrencyRequest()
                    {
                        Amount = 0,
                        VirtualCurrency = "MK"
                    };
                    PlayFabClientAPI.AddUserVirtualCurrency(reqCurrenci, result =>{},OnLoginFailure);
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
            var request = new GetUserInventoryRequest();
            PlayFabClientAPI.GetUserInventory(request, result =>
            {
                inventary = new List<IExtra>();
                GetInventory(itemsResult =>
                {
                    Extra extra = null;
                    foreach (var instance in result.Inventory)
                    {
                        foreach (var item in itemsResult.Catalog.Where(item => item.ItemId == instance.ItemId))
                        {
                            extra = JsonUtility.FromJson<Extra>(item.CustomData);
                        }
                        switch (extra.type)
                        {
                            case "text":
                                inventary.Add(new ImageComponentExtra(extra));
                                break;
                            case "image":
                                inventary.Add(new ImageComponentExtra(extra));
                                break;
                            case "video":
                                inventary.Add(new ImageComponentExtra(extra));
                                break;
                        }
                    }
                    isRequestOk = true;
                });
                
            }, error =>
            {
                isRequestOk = true;
            });
            while (!isRequestOk)
            {
                await Task.Delay(TimeSpan.FromSeconds(.3f));
            }
            return inventary.Count > 0;
        }

        public async void GetInventory(Action<GetCatalogItemsResult> resultCallback)
        {
            var resultPlayfab = false;
            var catalog = new GetCatalogItemsRequest()
            {
                CatalogVersion = "Extras"
            };
            PlayFabClientAPI.GetCatalogItems(catalog, itemsResult =>
            {
                resultCallback?.Invoke(itemsResult);
                resultPlayfab = true;
            }, error =>
            {
                OnLoginFailure(error);
                resultPlayfab = true;
            });
            while (!resultPlayfab)
            {
                await Task.Delay(TimeSpan.FromSeconds(.3f));
            }
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
            throw new NotImplementedException();
        }

        public void SaveData()
        {
            CreatedItem();
        }

        public void CreatedItem()
        {
            GetInventory(result =>
            {
                var catalogItem = result.Catalog[Random.Range(0, result.Catalog.Capacity)];
                PurchaseItemRequest purchase = new PurchaseItemRequest()
                {
                    CatalogVersion = catalogItem.CatalogVersion,
                    ItemId = catalogItem.ItemId,
                    Price = 0,
                    VirtualCurrency = "MK"
                };
                PlayFabClientAPI.PurchaseItem(purchase, result =>{},OnLoginFailure);
            });
        }
    }
}