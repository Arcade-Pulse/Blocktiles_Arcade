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

        private void Start()
        {
            guestLoginButton.onClick.AddListener(LoginWithDeviceButton);

            if (
                ObscuredPrefs.HasKey(PlayerPrefNameString.LAST_LOGIN) &&
                ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) == PlayerPrefNameString.GUEST
            )
            {
                LoginWithDeviceButton();
            }
        }

        public void LoginWithDeviceButton()
        {
            GameSceneManager.Instance.ShowLoadingPanel();
            StartCoroutine(CheckInternetConnectivity.CheckForInternetConnection("https://www.google.com", connected =>
            {
                if (connected)
                {
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
            if (!string.IsNullOrEmpty(android_id))
            {
               // Debug.Log("Logging in with Android Device ID " + android_id);
                PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
                {
                    CreateAccount = true,
                    AndroidDeviceId = android_id,
                    AndroidDevice = SystemInfo.deviceModel,
                    OS = SystemInfo.operatingSystem,
                    TitleId = PlayFabSettings.TitleId,
                }, response =>
                {
                    Debug.Log("Successful login with Android Device ID");
                    UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.GUEST);
                }, error =>
                {
                    Debug.Log("Unsuccessful login with Android Device ID");
                    UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
                });
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
                    Debug.Log("Unsuccessful login with iOS Device ID");
                    UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
                });

            }
            else if (!string.IsNullOrEmpty(custom_id))
            {
             //   Debug.Log("Logging in with Custom ID " + custom_id);
                PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
                {
                    CreateAccount = true,
                    CustomId = custom_id,
                    TitleId = PlayFabSettings.TitleId,
                }, response =>
                {
//                    Debug.Log("Successful login with Custom ID");
                    UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.GUEST);
                }, error =>
                {
                   // Debug.Log("Unsuccessful login with Custom ID: " + error.ErrorMessage);
                    UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
                });
            }
            else
            {
                Debug.Log("No DeviceID is found!");
            }
        }
    }
}
