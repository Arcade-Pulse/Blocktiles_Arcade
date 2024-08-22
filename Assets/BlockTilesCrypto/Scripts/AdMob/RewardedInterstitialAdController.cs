// using System;
// using UnityEngine;
// using GoogleMobileAds.Api;
// using System.Collections;
// using System.IO;
// using System.Security.Cryptography;
// using System.Text;
// using CommonScripts;
// using PlayFab;
// using PlayFab.ClientModels;
// using PlayFabPersonal.Managers;
// using UnityEngine.SceneManagement;
//
// [AddComponentMenu("GoogleMobileAds/RewardedInterstitialAdController")]
// public class RewardedInterstitialAdController : MonoBehaviour
// {
//     public static RewardedInterstitialAdController Instance { get; private set; }
//    // public static bool DevelopmentMode { get; set; } = false;
//
//     private string adRewardedInterstitialID=AdsID.adRewardedInterstitialID;
//     private int retryCount = 0;
//     public bool isAdLoaded = false;
//
//     public bool adRewarded;
//     private RewardedInterstitialAd _rewardedInterstitialAd;
//
//     
//     public static event EventHandler OnLoaded;
//     public static event EventHandler OnLoadFailed;
//
//     private void Awake()
//     {
//         // If there is an instance, and it's not me, delete myself.
//
//         if (Instance != null && Instance != this)
//         {
//             Destroy(this);
//         }
//         else
//         {
//             Instance = this;
//         }
//
//         DontDestroyOnLoad(this);
//     }
//
//     public void LoadAd()
//     {
//         if (isAdLoaded) return;
//
//         // Clean up the old ad before loading a new one.
//         if (_rewardedInterstitialAd != null)
//         {
//             DestroyAd();
//         }
//
//      //   Debug.Log("Loading rewarded interstitial ad." + adRewardedInterstitialID);
//
//         // Create our request used to load the ad.
//         var adRequest = new AdRequest();
//
//         // Send the request to load the ad.
//         RewardedInterstitialAd.Load(adRewardedInterstitialID, adRequest,
//             (RewardedInterstitialAd ad, LoadAdError error) =>
//             {
//                 // If the operation failed with a reason.
//                 if (error != null)
//                 {
//                    // Debug.Log("Rewarded interstitial ad failed to load an ad with error: " + error);
//                     StartCoroutine(RetryLoadAd(2));
//                     OnLoadFailed?.Invoke(this, EventArgs.Empty);
//                     return;
//                 }
//                 // If the operation failed for unknown reasons.
//                 if (ad == null)
//                 {
//                   //  Debug.Log("Unexpected error: Rewarded interstitial load event fired with null ad and null error.");
//                     return;
//                 }
//
//                 // The operation completed successfully.
//                 //Debug.Log("Rewarded interstitial ad loaded with response: " + ad.GetResponseInfo());
//                 _rewardedInterstitialAd = ad;
//                 isAdLoaded = true;
//                 StartCoroutine(SetCustomData());
//                 EnableCashoutButton();
//                 OnLoaded?.Invoke(this, EventArgs.Empty);
//
//                 // Register to ad events to extend functionality.
//                 RegisterEventHandlers(ad);
//             });
//     }
//
//
//     public void EnableCashoutButton()
//     {
//         StartCoroutine(EnableCashoutButtonCoroutine());
//     }
//
//     public IEnumerator SetCustomData()
//     {
//         yield return new WaitUntil(()=>PlayfabDataManager.Instance != null);
//         yield return new WaitUntil(() => PlayfabDataManager.Instance.currentPlayerID != null);
//        // Debug.Log("checked here 1");
//         if (isAdLoaded)
//         {
//             var options = new ServerSideVerificationOptions();
//             options.CustomData = PlayfabDataManager.Instance.currentPlayerID;
//             _rewardedInterstitialAd.SetServerSideVerificationOptions(options);
//             //Debug.Log("checked here 2");
//
//         }
//         else
//         {
//             LoadAd();
//           //  Debug.Log("checked here 3");
//
//         }
//
//         
//     }
//
//     public void ShowAd()
//     {
//         var firebaseRewInterst = FirebaseSettings.Instance.ShowRewardedInterstitialAds;
//         if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd() && firebaseRewInterst)
//         {
//             _rewardedInterstitialAd.Show((Reward reward) =>
//             {
//                // Debug.Log("Rewarded interstitial ad rewarded : " + reward.Amount);
//                adRewarded = true;
//             });
//         }
//         else
//         {
//            // Debug.Log("Rewarded interstitial ad is not ready yet.");
//         }
//     }
//     
//     private IEnumerator EnableCashoutButtonCoroutine()
//     {
//         // Wait until the PlayerUIManager is loaded into the scene
//         while (PlayerUIManager.Instance == null)
//         {
//             yield return null;
//         }
//
//         // Ensure the UIManager has been found
//         var uiManager = PlayerUIManager.Instance;
//         if (uiManager != null)
//         {
//           //  Debug.Log("PlayerUIManager found.");
//
//             // Make the CashoutButton interactable
//             if (uiManager.cashoutButton != null)
//             {
//               //  Debug.Log("CashoutButton found, making it interactable.");
//                 uiManager.cashoutButton.interactable = true;
//             }
//             else
//             {
//               //  Debug.LogWarning("CashoutButton reference is missing in PlayerUIManager.");
//             }
//         }
//         else
//         {
//            // Debug.LogError("PlayerUIManager not found in the scene.");
//         }
//     }
//
//     private IEnumerator RetryLoadAd(float delayInSecond)
//     {
//         yield return new WaitForSeconds(delayInSecond);
//         retryCount++;
//         LoadAd();
//     }
//
//     public void DestroyAd()
//     {
//         if (_rewardedInterstitialAd != null)
//         {
//           //  Debug.Log("Destroying rewarded interstitial ad.");
//             _rewardedInterstitialAd.Destroy();
//             _rewardedInterstitialAd = null;
//         }
//     }
//
//     public void CheckForRewarded()
//     {
//         if (!adRewarded)
//         {
//             // Attempt to find the MenuPanel object
//             if (MenuPanel.Instance != null)
//             {
//                 //Debug.Log("MenuPanel found, setting it active.");
//                 MenuPanel.Instance.gameObject.SetActive(true);
//             }
//             else
//             {
//                // Debug.LogWarning("MenuPanel not found in the scene.");
//             }
//
//             // Attempt to find the CashoutPanel object
//             var cashoutPanel = FindObjectOfType<CashoutPanel>();
//             if (CashoutPanel.Instance != null)
//             {
//                // Debug.Log("CashoutPanel found, setting it inactive.");
//                 CashoutPanel.Instance.gameObject.SetActive(false);
//             }
//             else
//             {
//                 //Debug.LogWarning("CashoutPanel not found in the scene.");
//             }
//         }
//     }
//     
//     private void SendAdRevenueToCloudScript(string encryptedValue)
//     {
//         var request = new ExecuteCloudScriptRequest
//         {
//             FunctionName = "UpdateAdRevenue",
//             RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
//             FunctionParameter = new
//             {
//                 encryptedAdRevenue = encryptedValue
//             },
//             GeneratePlayStreamEvent = true
//         };
//
//         PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptSuccess, OnCloudScriptError);
//     }
//     
//     private void OnCloudScriptSuccess(ExecuteCloudScriptResult result)
//     {
//       //  Debug.Log("Successfully sent encrypted ad revenue to CloudScript.");
//     }
//
//     private void OnCloudScriptError(PlayFabError error)
//     {
//        // Debug.LogError("Error sending encrypted ad revenue to CloudScript: " + error.GenerateErrorReport());
//     }
//     
//
//
//
//
//     protected void RegisterEventHandlers(RewardedInterstitialAd ad)
//     {
//         // Raised when the ad is estimated to have earned money.
//         ad.OnAdPaid += (AdValue adValue) =>
//         {
//             // Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
//             //     adValue.Value,
//             //     adValue.CurrencyCode));
//             if (PlayFabClientAPI.IsClientLoggedIn())
//             {
//               //  Debug.Log("ad revenue is "+adValue.Value);
//                 string encryptedValue = Encryption.Encrypt(adValue.Value.ToString());
//                 SendAdRevenueToCloudScript(encryptedValue);
//             }
//
//         };
//         // Raised when an impression is recorded for an ad.
//         ad.OnAdImpressionRecorded += () =>
//         {
//             //Debug.Log("Rewarded interstitial ad recorded an impression.");
//         };
//         // Raised when a click is recorded for an ad.
//         ad.OnAdClicked += () =>
//         {
//           //  Debug.Log("Rewarded interstitial ad was clicked.");
//         };
//         // Raised when an ad opened full screen content.
//         ad.OnAdFullScreenContentOpened += () =>
//         {
//           //  Debug.Log("Rewarded interstitial ad full screen content opened.");
//         };
//         // Raised when the ad closed full screen content.
//         ad.OnAdFullScreenContentClosed += () =>
//         {
//           //  Debug.Log("Rewarded interstitial ad full screen content closed.");
//             isAdLoaded = false;
// #if UNITY_ANDROID && !UNITY_EDITOR
//             CheckForRewarded();
// #endif
//             StartCoroutine(RetryLoadAd(6));
//
//         };
//         // Raised when the ad failed to open full screen content.
//         ad.OnAdFullScreenContentFailed += (AdError error) =>
//         {
//             // Debug.Log("Rewarded interstitial ad failed to open full screen content" +
//             //                " with error : " + error);
//             isAdLoaded = false;
//             LoadAd();
//         };
//     }
// }
