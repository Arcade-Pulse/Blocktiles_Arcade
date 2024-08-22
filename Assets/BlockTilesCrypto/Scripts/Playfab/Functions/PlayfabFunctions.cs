using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

namespace PlayFabPersonal.Functions
{
    public static class PlayfabFunctions
    {
        public static void GetPlayerInventory(string key, Action<string> onValueReceived, Action<string> onError)
        {

        }

        private static Dictionary<string, string> userDataCache = new Dictionary<string, string>();

        public static void GetUserData(string key, Action<string> onValueReceived, Action<string> onError)
        {
            if (userDataCache.ContainsKey(key))
            {
                string value = userDataCache[key];
                onValueReceived?.Invoke(value);
            }
            else
            {
                PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
                {
                    if (result.Data != null && result.Data.ContainsKey(key))
                    {
                        string value = result.Data[key].Value;
                        userDataCache[key] = value;
                        onValueReceived?.Invoke(value);
                    }
                    else
                    {
                        onError?.Invoke("Value not found for key: " + key);
                    }
                }, error =>
                {
                    onError?.Invoke(error.ErrorMessage);
                });
            }
        }
    }
}
