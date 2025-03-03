// using System;
// using UnityEngine;
// using GoogleMobileAds.Api;
// using System.Collections;
//
// [AddComponentMenu("GoogleMobileAds/InterstitialAdController")]
// public class InterstitialAdController : MonoBehaviour
// {
//     public static InterstitialAdController Instance { get; private set; }
//   //  public static bool DevelopmentMode { get; set; } = false;
//
//     private string adInterstitialID=AdsID.adInterstitialID;
//     private int retryCount = 0;
//     public bool isAdLoaded = false;
//
//     private InterstitialAd _interstitialAd;
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
//         if(isAdLoaded ) return;
//         // Clean up the old ad before loading a new one.
//         if (_interstitialAd != null)
//         {
//             DestroyAd();
//         }
//
//        // Debug.Log("Loading interstitial ad.");
//
//         // Create our request used to load the ad.
//         var adRequest = new AdRequest();
//
//         // Send the request to load the ad.
//         InterstitialAd.Load(adInterstitialID, adRequest, (InterstitialAd ad, LoadAdError error) =>
//         {
//             // If the operation failed with a reason.
//             if (error != null)
//             {
//                // Debug.Log("Interstitial ad failed to load an ad with error : " + error);
//                 // // Calculate the delay based on the retry count (e.g., 5 seconds for the 1st retry, 10 seconds for the 2nd, and so on).
//                 // float delayInSeconds = 5f + (retryCount * 5f);
//
//                 // // Retry loading the ad after the calculated delay.
//                 StartCoroutine(RetryLoadAd(4));
//                 OnLoadFailed?.Invoke(this, EventArgs.Empty);
//                 return;
//             }
//             // If the operation failed for unknown reasons.
//             // This is an unexpected error, please report this bug if it happens.
//             if (ad == null)
//             {
//                // Debug.Log("Unexpected error: Interstitial load event fired with null ad and null error.");
//                 return;
//             }
//
//             // The operation completed successfully.
//            // Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
//            // Debug.Log("Current loaded Interstitial ad serving adapter " + ad.GetResponseInfo().GetMediationAdapterClassName());
//             _interstitialAd = ad;
//             isAdLoaded = true;
//             OnLoaded?.Invoke(this, EventArgs.Empty);
//             // retryCount = 0;
//
//             // Register to ad events to extend functionality.
//             RegisterEventHandlers(ad);
//         });
//     }
//
//     public void ShowAd()
//     {
//        // var interstitialFirebase = FirebaseSettings.Instance.ShowInterstitialAds;
//       //  Debug.Log("show interstitial ad is "+interstitialFirebase);
//         if (_interstitialAd != null && _interstitialAd.CanShowAd() )
//         {
//             //Debug.Log("Showing interstitial ad.");
//             _interstitialAd.Show();
//         }
//         else
//         {
//             Debug.Log("Interstitial ad is not ready yet.");
//              LoadAd();
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
//         if (_interstitialAd != null)
//         {
//             //Debug.Log("Destroying interstitial ad.");
//             _interstitialAd.Destroy();
//             _interstitialAd = null;
//         }
//     }
//
//     private void RegisterEventHandlers(InterstitialAd ad)
//     {
//         // Raised when the ad is estimated to have earned money.
//         ad.OnAdPaid += (AdValue adValue) =>
//         {
//             // Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
//             //     adValue.Value,
//             //     adValue.CurrencyCode));
//         };
//         // Raised when an impression is recorded for an ad.
//         ad.OnAdImpressionRecorded += () =>
//         {
//             //Debug.Log("Interstitial ad recorded an impression.");
//         };
//         // Raised when a click is recorded for an ad.
//         ad.OnAdClicked += () =>
//         {
//            // Debug.Log("Interstitial ad was clicked.");
//         };
//         // Raised when an ad opened full screen content.
//         ad.OnAdFullScreenContentOpened += () =>
//         {
//             //Debug.Log("Interstitial ad full screen content opened.");
//         };
//         // Raised when the ad closed full screen content.
//         ad.OnAdFullScreenContentClosed += () =>
//         {
//             //Debug.Log("Interstitial ad full screen content closed.");
//             isAdLoaded = false;
//             LoadAd();
//         };
//         // Raised when the ad failed to open full screen content.
//         ad.OnAdFullScreenContentFailed += (AdError error) =>
//         {
//             // Debug.Log("Interstitial ad failed to open full screen content with error : "
//             //     + error);
//             isAdLoaded = false;
//             LoadAd();
//         };
//     }
// }
