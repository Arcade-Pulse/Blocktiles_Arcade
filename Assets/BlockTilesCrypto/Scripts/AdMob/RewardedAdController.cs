using System;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;
using System.Text;
using CommonScripts;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;

[AddComponentMenu("GoogleMobileAds/RewardedAdController")]
public class RewardedAdController : MonoBehaviour
{
    public static RewardedAdController Instance { get; private set; }
    //public static bool DevelopmentMode { get; set; } = false;
    private string encryptionKey = "A5d!9fJ#2sL@8mQ^4rT&7nW*0xY@3vZ8"; // Ensure this is 16, 24, or 32 characters long


    private string adRewardedID=AdsID.adRewardedID;
    private int retryCount = 0;
    public bool isAdLoaded = false;

    private RewardedAd _rewardedAd;
    public static event EventHandler OnLoaded;
    public static event EventHandler OnLoadFailed;
    public bool inGameRequest;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void LoadAd()
    {
//        Debug.Log("loading ad rewarded");
        if(isAdLoaded) return;

        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            DestroyAd();
        }

       // Debug.Log("Loading rewarded ad."+adRewardedID);

        // Create our request used to load the ad.
        var adRequest = new AdRequest();
//        Debug.Log("loading ad "+adRewardedID);

        // Send the request to load the ad.
        RewardedAd.Load(adRewardedID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
               // Debug.Log("Rewarded ad failed to load an ad with error : " + error);
                // // Calculate the delay based on the retry count (e.g., 5 seconds for the 1st retry, 10 seconds for the 2nd, and so on).
                // float delayInSeconds = 5f + (retryCount * 5f);

                // // Retry loading the ad after the calculated delay.
                StartCoroutine(RetryLoadAd(10));
                OnLoadFailed?.Invoke(this, EventArgs.Empty);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                //Debug.Log("Unexpected error: Rewarded load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
           // Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
          //  Debug.Log("Current loaded Rewarded ad serving adapter " + ad.GetResponseInfo().GetMediationAdapterClassName());
            _rewardedAd = ad;
            isAdLoaded = true;
          //  SetCustomData();
            OnLoaded?.Invoke(this, EventArgs.Empty);

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);
        });
    }
    
    public void SetCustomData()
    {
        var options = new ServerSideVerificationOptions();
        options.CustomData = PlayfabDataManager.Instance.currentPlayerID;
        _rewardedAd.SetServerSideVerificationOptions(options);

    }

    public void ShowAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
           // Debug.Log("Showing rewarded ad.");
            // adLoaded = false;
            _rewardedAd.Show(reward =>
            {
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));
            });
        }
        else
        {
           // Debug.Log("Rewarded ad is not ready yet.");
        }
    }

    private IEnumerator RetryLoadAd(float delayInSecond)
    {
        yield return new WaitForSeconds(delayInSecond);
        retryCount++;
        LoadAd();
    }

    public void DestroyAd()
    {
        if (_rewardedAd != null)
        {
           // Debug.Log("Destroying rewarded ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }
    
    private void SendAdRevenueToCloudScript(string encryptedValue)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateAdRevenue",
            RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
            FunctionParameter = new
            {
                encryptedAdRevenue = encryptedValue
            },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptSuccess, OnCloudScriptError);
    }

    private void OnCloudScriptSuccess(ExecuteCloudScriptResult result)
    {
      //  Debug.Log("Successfully sent encrypted ad revenue to CloudScript.");
    }

    private void OnCloudScriptError(PlayFabError error)
    {
        Debug.LogError("Error sending encrypted ad revenue to CloudScript: " + error.GenerateErrorReport());
    }

    // private string Encrypt(string plainText)
    // {
    //     char[] key = encryptionKey.ToCharArray();
    //     char[] input = plainText.ToCharArray();
    //     char[] output = new char[input.Length];
    //
    //     for (int i = 0; i < input.Length; i++)
    //     {
    //         output[i] = (char)(input[i] ^ key[i % key.Length]);
    //     }
    //
    //     return Convert.ToBase64String(Encoding.UTF8.GetBytes(output));
    // }


    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
              //  Debug.Log("ad revenue is "+adValue.Value);
                string encryptedValue = Encryption.Encrypt(adValue.Value.ToString());
                SendAdRevenueToCloudScript(encryptedValue);
            }
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
          // Debug.Log("Rewarded ad recorded an impression.");
          if (inGameRequest)
          {
              inGameRequest = false;
          }
          else
          {
              VirtualCurrency.Instance.AddLife(PlayfabDataManager.Instance.GetLifeRewardPerAd());
          }
              //inGameRequest = false;
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
           // Debug.Log("Rewarded ad was clicked.");
           // VirtualCurrency.Instance.AddLife(1);
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
          //  Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
         //   Debug.Log("Rewarded ad full screen content closed.");
            isAdLoaded = false;
            LoadAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Debug.Log("Rewarded ad failed to open full screen content with error : "
            //     + error);
            isAdLoaded = false;
            LoadAd();
        };
    }
}
