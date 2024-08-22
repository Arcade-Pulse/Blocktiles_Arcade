// using System;
// using System.Collections.Generic;
// using CodeStage.AntiCheat.Storage;
// using CommonScripts;
// using Facebook.Unity;
// using Global;
// using PlayFab;
// using PlayFab.ClientModels;
// using PlayFabPersonal.Managers;
// using PlayFabPersonal.Users;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace PlayFabPersonal.Authentications
// {
//     public class FacebookAuthentication : MonoBehaviour
//     {
//         [SerializeField] private Button facebookLoginButton;
//
//         private void Start()
//         {
//             facebookLoginButton.onClick.AddListener(FacebookLoginButton);
//
//             if (
//                 ObscuredPrefs.HasKey(PlayerPrefNameString.LAST_LOGIN) &&
//                 ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) == PlayerPrefNameString.FACEBOOK
//             )
//             {
//                 AutoLoginWithFacebook();
//             }
//             else
//             {
//                 GameSceneManager.Instance.HideLoadingPanel();
//             }
//         }
//
//         private void FacebookLoginButton()
//         {
//             GameSceneManager.Instance.ShowLoadingPanel();
//             Debug.Log("button pressed");
//             // check internet connectivity first
//             StartCoroutine(CheckInternetConnectivity.CheckForInternetConnection("https://www.google.com", connected =>
//             {
//                 if (connected)
//                 {
//                     Debug.Log("connected");
//                     //LoginMenu.Instance.AllButtonsNoTinteractable();
//
//                     if (!FB.IsInitialized)
//                     {
//                         // This call is required before any other calls to the Facebook API. We pass in the callback to be invoked once initialization is finished
//                         FB.Init(FacebookLogin);
//
//                     }
//                     else
//                     {
//                       //  Debug.Log("facebook login");
//                         FacebookLogin();
//                     }
//                     
//                     
//                 }
//                 else
//                 {
//                     Debug.LogFormat("No internet connection", 2, Color.white);
//                 }
//             }));
//         }
//
//         private void AutoLoginWithFacebook()
//         {
//             GameSceneManager.Instance.ShowLoadingPanel();
//             var request = new LoginWithFacebookRequest
//             {
//                 AccessToken = ObscuredPrefs.Get(PlayerPrefNameString.FACEBOOK, null)
//             };
//             
//             try
//             {
//                 PlayFabClientAPI.LoginWithFacebook(request, OnSuccess, OnError);
//             }
//             catch (Exception e)
//             {
//                // Debug.Log(e.Message);
//             }
//         }
//
//         private void FacebookLogin()
//         {
//             List<string> perms = new() { "public_profile", "email" };
//
//             FB.LogInWithReadPermissions(perms, (ILoginResult result) =>
//             {
//                 if (FB.IsLoggedIn)
//                 {
//                     // If result has no errors, it means we have authenticated in Facebook successfully
//                     if (result == null || string.IsNullOrEmpty(result.Error))
//                     {
//                       //  Debug.Log("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");
//
//                         /*
//                          * We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
//                          * and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results
//                          */
//
//                         var accessToken = AccessToken.CurrentAccessToken.TokenString;
//                         var request = new LoginWithFacebookRequest
//                         {
//                             AccessToken = accessToken,
//                             CreateAccount = true
//                         };
//
//                         PlayFabClientAPI.LoginWithFacebook(request, OnSuccess, OnError);
//                     }
//                     else
//                     {
//                       //  Debug.Log("authentication failed " + result.Error);
//                     }
//                 }
//                 else
//                 {
//                     // Debug.Log("User cancelled login");
//                     GameSceneManager.Instance.HideLoadingPanel();
//                     UserAccount.OnLoginFailed?.Invoke("User cancelled login");
//                 }
//             });
//         }
//
//         private void OnSuccess(PlayFab.ClientModels.LoginResult result)
//         {
//             if (ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) !=
//                 PlayerPrefNameString.FACEBOOK) // save access token to ObscuredPrefs so it can be automatically sign in
//             {
//                 ObscuredPrefs.Set(PlayerPrefNameString.FACEBOOK, AccessToken.CurrentAccessToken.TokenString);
//                 ObscuredPrefs.Set(PlayerPrefNameString.LAST_LOGIN, PlayerPrefNameString.FACEBOOK);
//                 Debug.Log("set player prefs to facebook");
//             }
//
//             PlayfabDataManager.Instance.SetPlayerID(result.PlayFabId);
//             UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.FACEBOOK);
//         }
//
//         private void OnError(PlayFabError error)
//         {
//             UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
//             GameSceneManager.Instance.HideLoadingPanel();
//         }
//     }
// }