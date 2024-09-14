using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using CommonScripts;
using PlayFabPersonal.Users;
using Global;
using CodeStage.AntiCheat.Storage;
using Random = UnityEngine.Random;

namespace PlayFabPersonal.Authentications
{
    public class GuestAuthentication : MonoBehaviour
    {
        [SerializeField] private Button guestLoginButton;

        private void Start()
        {
            LoginWithDeviceButton();
            // guestLoginButton.onClick.AddListener(LoginWithDeviceButton);
            //
            // if (
            //     ObscuredPrefs.HasKey(PlayerPrefNameString.LAST_LOGIN) &&
            //     ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) == PlayerPrefNameString.GUEST
            // )
            // {
            //     LoginWithDeviceButton();
            // }
        }

        public void LoginWithDeviceButton()
        {
            GameSceneManager.Instance.ShowLoadingPanel();
            StartCoroutine(CheckInternetConnectivity.CheckForInternetConnection("https://www.google.com", connected =>
            {
                if (connected)
                {
                    Debug.Log("starting guest login");
                    LoginWithDevice();
                }
                else
                {
                    GameSceneManager.Instance.HideLoadingPanel();
                    Debug.LogFormat("No internet connection", 2, Color.white);
                }
            }));
        }


        
        private void LoginWithDevice()
        {
            GeneralFunctions.GetDeviceID(out string android_id, out string ios_id, out string custom_id);
            // Debug.Log("Logging in with Android Device ID " + android_id);
            PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            {
                CreateAccount = true,
                AndroidDevice = SystemInfo.deviceModel,
                OS = SystemInfo.operatingSystem,
                TitleId = PlayFabSettings.TitleId,
                AndroidDeviceId = android_id
            }, response =>
            {
                Debug.Log("Successful login with Android Device ID");
                UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.GUEST);
            }, error =>
            {
                Debug.Log("Unsuccessful login with Android Device ID");
                Debug.Log("Unsuccessful login with Android ID: " + error.ErrorMessage);
              //  Debug.Log("Unsuccessful login with iOS Device ID");
                if (string.IsNullOrEmpty(custom_id))
                {
                    custom_id = UnityEngine.Random.Range(100000, 999999).ToString(); // Unity's Random class
                }

                PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
                {
                    CustomId = custom_id,
                    TitleId = PlayFabSettings.TitleId,
                    CreateAccount = true
                }, response =>
                {
                    Debug.Log("Successful login with Custom ID");
                    UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.GUEST);
                }, error =>
                {
                    Debug.Log("login title id "+PlayFabSettings.TitleId);
                     Debug.Log("Unsuccessful login with Custom ID: " + error.CustomData);
                     Debug.Log("Unsuccessful login with Custom ID code: " + error.Error);
                     var errordetails = error.ErrorDetails;
                     foreach (KeyValuePair<string, List<string>> entry in errordetails)
                     {
                         Debug.Log($"Category: {entry.Key}");
                     
                         foreach (string item in entry.Value)
                         {
                             Debug.Log($" Item - {item}");
                         }
                     }

                     Debug.Log("Unsuccessful login with Custom ID code: " + error.ErrorMessage);

                    UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
                });
              //  UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
            });
            if (!string.IsNullOrEmpty(android_id))
            {

            }
            else if (!string.IsNullOrEmpty(ios_id))
            {
             //   Debug.Log("Logging in with iOS Device ID " + ios_id);
                PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
                {
                    CreateAccount = true,
                    DeviceId = ios_id,
                    DeviceModel = SystemInfo.deviceModel,
                    OS = SystemInfo.operatingSystem,
                    TitleId = PlayFabSettings.TitleId,
                }, response =>
                {
                    Debug.Log("Successful login with iOS Device ID");
                    UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.GUEST);
                }, error =>
                {
                  
                });

            }
            else if (!string.IsNullOrEmpty(custom_id))
            {
             //   Debug.Log("Logging in with Custom ID " + custom_id);

            }
            else
            {
                Debug.Log("No DeviceID is found!");
            }
        }
    }
}
