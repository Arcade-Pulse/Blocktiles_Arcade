// using System;
// using CodeStage.AntiCheat.Storage;
// using CommonScripts;
// using Global;
// using GooglePlayGames;
// using GooglePlayGames.BasicApi;
// using PlayFab;
// using PlayFab.ClientModels;
// using PlayFabPersonal.Managers;
// using PlayFabPersonal.Users;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace PlayFabPersonal.Authentications
// {
//     public class GoogleAuthentication : MonoBehaviour
//     {
//         [SerializeField] private Button googleLoginButton;
// #if UNITY_ANDROID
//
//         private string serverAuthCode = null;
//         public static GoogleAuthentication Instance { get; private set; }
//
//         private void OnEnable()
//         {
//             googleLoginButton.onClick.AddListener(GPGSLoginButton);
//             if (Instance != null && Instance != this)
//             {
//                 Destroy(this);
//             }
//             else
//             {
//                 Instance = this;
//             }
//         }
//
//         private void Start()
//         {
//
//             if (
//                 ObscuredPrefs.HasKey(PlayerPrefNameString.LAST_LOGIN) &&
//                 ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) == PlayerPrefNameString.GOOGLE
//             )
//             { 
//                 GameSceneManager.Instance.ShowLoadingPanel();
//                 AutoLoginWithGoogle();
//             }
//         }
//
//         private void AutoLoginWithGoogle()
//         {
//             StartCoroutine(CheckInternetConnectivity.CheckForInternetConnection("https://www.google.com", connected =>
//             {
//                 if (connected)
//                 {
//                     PlayGamesPlatform.Activate();
//                     PlayGamesPlatform.Instance.Authenticate(AutoLogin);
//                 }
//                 else
//                 {
//                     GameSceneManager.Instance.HideLoadingPanel();
//                     Debug.LogFormat("No internet connection", 2, Color.white);
//                 }
//             }));
//            
//         }
//
//         public void GPGSLoginButton()
//         {
//             GameSceneManager.Instance.ShowLoadingPanel();
//
//             StartCoroutine(CheckInternetConnectivity.CheckForInternetConnection("https://www.google.com", connected =>
//            {
//                if (connected)
//                {
//                    StartLogin();
//                }
//                else
//                {
//                    GameSceneManager.Instance.HideLoadingPanel();
//                    Debug.LogFormat("No internet connection", 2, Color.white);
//                }
//            }));
//
//         }
//
//         private void StartLogin()
//         {
//           //  Debug.Log("starting login");
//             PlayGamesPlatform.Activate();
//             PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
//         }
//
//         public void AutoLogin(SignInStatus status)
//         {
//             if (status == SignInStatus.Success)
//             {
//               //  Debug.Log("sign in status is success");
//                 
//                 // Continue with Play Games Services
//                 PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
//                 {
//                     serverAuthCode = code;
//                   //  Debug.Log("Authenticated " + serverAuthCode + "Hello, " + Social.localUser.userName + " (" + Social.localUser.id + ")");
//
//                     LoginWithGooglePlayGamesServicesRequest loginWithGPGSRequest = new LoginWithGooglePlayGamesServicesRequest()
//                     {
//                         TitleId = PlayFabSettings.TitleId,
//                         ServerAuthCode = serverAuthCode,
//                         CreateAccount = true
//                     };
//
//                     PlayFabClientAPI.LoginWithGooglePlayGamesServices(loginWithGPGSRequest, OnSuccess, true ? OnError : OnErrorAuto);
//                 });
//             }
//             else
//             {
//                 GameSceneManager.Instance.HideLoadingPanel();
//
//             }
//         }
//
//         internal void ProcessAuthentication(SignInStatus status)
//         {
//             if (status == SignInStatus.Success)
//             {
//                // Debug.Log("sign in status is success");
//
//                 //GameSceneManager.Instance.ShowLoadingPanel();
//                 // Continue with Play Games Services
//                 PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
//                 {
//                     serverAuthCode = code;
//                    // Debug.Log("Authenticated " + serverAuthCode + "Hello, " + Social.localUser.userName + " (" + Social.localUser.id + ")");
//
//                     LoginWithGooglePlayGamesServicesRequest loginWithGPGSRequest = new LoginWithGooglePlayGamesServicesRequest()
//                     {
//                         TitleId = PlayFabSettings.TitleId,
//                         ServerAuthCode = serverAuthCode,
//                         CreateAccount = true
//                     };
//
//                     PlayFabClientAPI.LoginWithGooglePlayGamesServices(loginWithGPGSRequest, OnSuccess, true ? OnError : OnErrorAuto);
//                 });
//             }
//             else
//             {
//                 GameSceneManager.Instance.HideLoadingPanel();
//                 ObscuredPrefs.DeleteKey(PlayerPrefNameString.LAST_LOGIN);
//                 // Disable your integration with Play Games Services or show a login button
//                 // to ask users to sign-in. Clicking it should call
//                 Debug.Log("starting manual login");
//                 PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
//             }
//         }
//
//         private void OnSuccess(LoginResult result)
//         {
//             if (ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) != PlayerPrefNameString.GOOGLE)  // save access token to ObscuredPrefs so it can be automatically sign in
//             {
//                 ObscuredPrefs.Set(PlayerPrefNameString.LAST_LOGIN, PlayerPrefNameString.GOOGLE);
//             }
//
//             PlayfabDataManager.Instance.SetPlayerID(result.PlayFabId);
//             UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.GOOGLE);
//         }
//
//         private void OnErrorAuto(PlayFabError error) { UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage); }
//
//         private void OnError(PlayFabError error) { UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage); }
// #endif
//     }
// }