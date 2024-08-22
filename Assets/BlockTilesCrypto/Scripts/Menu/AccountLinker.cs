using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using CommonScripts;
using Facebook.Unity;
using Global;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Managers;
using UnityEngine;
using UnityEngine.UI;

public class AccountLinker : MonoBehaviour
{
    [SerializeField] private Button facebookLinkingButton;
    [SerializeField] private Button googleLinkingButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject guestNotAllowedPanel;

    private string serverAuthCode = null;

    
    private void OnEnable()
    {
        backButton.onClick.AddListener(HideAccountLinkerPanel);
        facebookLinkingButton.onClick.AddListener(FacebookLinkingButtonClicked);
        googleLinkingButton.onClick.AddListener(googleLinkingButtonClicked);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(HideAccountLinkerPanel);
    }

    private void googleLinkingButtonClicked()
    {
        googleLinkingButton.interactable = false;
        facebookLinkingButton.interactable = false;
        StartCoroutine(CheckInternetConnectivity.CheckForInternetConnection("https://www.google.com", connected =>
        {
            if (connected)
            {
                LinkGooglePlayGamesAccount();
            }
            else
            {
                googleLinkingButton.interactable = true;
                facebookLinkingButton.interactable = true;
                Debug.LogFormat("No internet connection", 2, Color.white);
            }
        }));
    }

    private void LinkGooglePlayGamesAccount()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        
    }
    
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
           // Debug.Log("sign in status is success");

            //GameSceneManager.Instance.ShowLoadingPanel();
            // Continue with Play Games Services
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
            {
                serverAuthCode = code;
              //  Debug.Log("Authenticated " + serverAuthCode + "Hello, " + Social.localUser.userName + " (" + Social.localUser.id + ")");
                if (!string.IsNullOrEmpty(serverAuthCode))
                {
                
                    var request = new LinkGooglePlayGamesServicesAccountRequest()
                    {
                        ServerAuthCode = code
                    };

                    PlayFabClientAPI.LinkGooglePlayGamesServicesAccount(request, OnSuccessLinkGooglePlayGamesAccount, OnError);
                }
                else
                {
        //            Debug.Log("faled to obtain valid server auth code");
                }
            });
        }
        else
        {
            GameSceneManager.Instance.HideLoadingPanel();
            ObscuredPrefs.DeleteKey(PlayerPrefNameString.LAST_LOGIN);
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            Debug.Log("starting manual login");
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
    }

    private void OnSuccessLinkGooglePlayGamesAccount(LinkGooglePlayGamesServicesAccountResult result)
    {
        MessageManager.OnSuccessShowMessage?.Invoke("Linking succesful");
        PlayfabDataManager.Instance.isGuestAccountLinked = true;

        //  ObscuredPrefs.Set(PlayerPrefNameString.LAST_LOGIN, PlayerPrefNameString.FACEBOOK);
        CashoutLoader.Instance.LoadCashoutPanel();
        this.gameObject.SetActive(false);

    }

    private void FacebookLinkingButtonClicked()
    {
        googleLinkingButton.interactable = false;
        facebookLinkingButton.interactable = false;
        StartCoroutine(CheckInternetConnectivity.CheckForInternetConnection("https://www.google.com", connected =>
        {
            if (connected)
            {
               FBInit();
            }
            else
            {
                googleLinkingButton.interactable = true;
                facebookLinkingButton.interactable = true;
                Debug.LogFormat("No internet connection", 2, Color.white);
            }
        }));
    }

    public void FBInit()
    {
        if (!FB.IsInitialized)
        {
            // This call is required before any other calls to the Facebook API. We pass in the callback to be invoked once initialization is finished
            FB.Init(LinkFacebookAccount);

        }
        else
        {
            Debug.Log("facebook login");
            LinkFacebookAccount();
        }
    }

    private void LinkFacebookAccount()
    {
        List<string> perms = new() { "public_profile", "email" };

        FB.LogInWithReadPermissions(perms, (ILoginResult result) =>
        {
            if (FB.IsLoggedIn)
            {
                // If result has no errors, it means we have authenticated in Facebook successfully
                if (result == null || string.IsNullOrEmpty(result.Error))
                {
                 //   Debug.Log("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");

                    var accessToken = AccessToken.CurrentAccessToken.TokenString;

                    var request = new LinkFacebookAccountRequest
                    {
                        AccessToken = accessToken
                    };

                    PlayFabClientAPI.LinkFacebookAccount(request, OnSuccessFacebookLink, OnError);
                }
                else
                {
              //      Debug.Log("authentication failed " + result.Error);
                }
            }
            else
            {
                googleLinkingButton.interactable = true;
                facebookLinkingButton.interactable = true;
               // Debug.Log("User cancelled login");
            }
        });
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnSuccessFacebookLink(LinkFacebookAccountResult result)
    {
        MessageManager.OnSuccessShowMessage?.Invoke("Linking succesful");
        PlayfabDataManager.Instance.isGuestAccountLinked = true;

        //  ObscuredPrefs.Set(PlayerPrefNameString.LAST_LOGIN, PlayerPrefNameString.FACEBOOK);
        CashoutLoader.Instance.LoadCashoutPanel();
        this.gameObject.SetActive(false);

    }

    private void OnError(PlayFabError error)
    {
        googleLinkingButton.interactable = true;
        facebookLinkingButton.interactable = true;
        MessageManager.OnErrorShowMessage?.Invoke(error.ErrorMessage);
        this.gameObject.SetActive(false);
    }

    private void HideAccountLinkerPanel()
    {
        guestNotAllowedPanel.SetActive(true);
        this.gameObject.SetActive(false);

    }

}
