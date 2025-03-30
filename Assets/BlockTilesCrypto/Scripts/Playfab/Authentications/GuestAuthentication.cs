using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using CommonScripts;
using PlayFabPersonal.Users;
using Global;
using CodeStage.AntiCheat.Storage;

namespace PlayFabPersonal.Authentications
{
    public class GuestAuthentication : MonoBehaviour
    {
        [SerializeField] private Button guestLoginButton;
        private int retryCount = 0;
        private const int maxRetryAttempts = 3;

        private void Start()
        {
            LoginWithDeviceButton();
            // Optionally, you can enable the button to allow manual triggering of login
            // guestLoginButton.onClick.AddListener(LoginWithDeviceButton);
        }

        public void LoginWithDeviceButton()
        {
            GameSceneManager.Instance.ShowLoadingPanel();
            StartCoroutine(CheckInternetConnectivity.CheckForInternetConnection("https://www.google.com", connected =>
            {
                if (connected)
                {
                 //   Debug.Log("Starting guest login");
                    LoginWithDevice();
                }
                else
                {
                    GameSceneManager.Instance.HideLoadingPanel();
                    //Debug.LogWarning("No internet connection available.");
                }
            }));
        }

        private void LoginWithDevice()
        {
            GeneralFunctions.GetDeviceID(out string android_id, out string ios_id, out string custom_id);

            if (!string.IsNullOrEmpty(android_id))
            {
                // Attempt login with Android Device ID
              //  Debug.Log("Attempting login with Android Device ID: " + android_id);
                PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
                {
                    CreateAccount = true,
                    AndroidDeviceId = android_id,
                    AndroidDevice = SystemInfo.deviceModel,
                    OS = SystemInfo.operatingSystem,
                    TitleId = PlayFabSettings.TitleId,
                }, OnLoginSuccess, OnLoginFailure);
            }
            else if (!string.IsNullOrEmpty(ios_id))
            {
                // Attempt login with iOS Device ID
              //  Debug.Log("Attempting login with iOS Device ID: " + ios_id);
                PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
                {
                    CreateAccount = true,
                    DeviceId = ios_id,
                    DeviceModel = SystemInfo.deviceModel,
                    OS = SystemInfo.operatingSystem,
                    TitleId = PlayFabSettings.TitleId,
                }, OnLoginSuccess, OnLoginFailure);
            }
            else
            {
                // Fallback to Custom ID if no Android/iOS ID is found
                if (string.IsNullOrEmpty(custom_id))
                {
                    custom_id = UnityEngine.Random.Range(100000, 999999).ToString();
                   // Debug.LogWarning("No valid Android or iOS ID found. Using fallback Custom ID: " + custom_id);
                }

                PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
                {
                    CustomId = custom_id,
                    TitleId = PlayFabSettings.TitleId,
                    CreateAccount = true
                }, OnLoginSuccess, OnLoginFailure);
            }
        }

        private void OnLoginSuccess(LoginResult result)
        {
          //  Debug.Log("Login successful.");
            retryCount = 0; // Reset the retry count on success
            GameSceneManager.Instance.HideLoadingPanel();
            UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.GUEST);
        }

        private void OnLoginFailure(PlayFabError error)
        {
           // Debug.LogError("Login failed: " + error.GenerateErrorReport());
            GameSceneManager.Instance.HideLoadingPanel();

            // Handle detailed logging of error
            if (error.ErrorDetails != null)
            {
                foreach (KeyValuePair<string, List<string>> entry in error.ErrorDetails)
                {
                   // Debug.LogError($"Error Category: {entry.Key}");
                    foreach (string item in entry.Value)
                    {
                       // Debug.LogError($" - {item}");
                    }
                }
            }

            // Retry logic
            retryCount++;
            if (retryCount < maxRetryAttempts)
            {
            //    Debug.LogWarning($"Retrying login, attempt {retryCount}/{maxRetryAttempts}");
                LoginWithDevice(); // Retry login
            }
            else
            {
              //  Debug.LogError("Max retry attempts reached. Login failed.");
                retryCount = 0; // Reset retry count after max attempts
                UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
                // Optionally notify the user about the failure
            }
        }
    }
}
