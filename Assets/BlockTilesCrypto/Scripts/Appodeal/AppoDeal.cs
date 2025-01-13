// using System.Collections;
// using System.Collections.Generic;
// using AppodealAds.Unity.Api;
// using AppodealAds.Unity.Common;
// using UnityEngine;
//
// public class AppoDeal : MonoBehaviour, IAppodealInitializationListener, IBannerAdListener
// {
//     public int retryCount;
//
//     void Awake() {
//         DontDestroyOnLoad(this.gameObject);  // Called only once when object is created
//     }
//
//     private void Start()
//     {
//         Debug.Log("Starting Appodeal");
//         int adTypes = Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO;
//         string appKey = "a13a870393ce2ce161df6d08bf61c13d8de9b2d0525947ce";
//         //Appodeal.setTesting(true); // Enable test ads for all formats
//         Appodeal.initialize(appKey, adTypes, this);
//         Appodeal.cache(Appodeal.BANNER);  // Force caching of the banner ad
//     }
//
//     public void onInitializationFinished(List<string> errors)
//     {
//         Debug.Log("Ads init finished");
//         if (errors != null && errors.Count > 0)
//         {
//             foreach (var error in errors)
//             {
//                 Debug.Log("Appodeal initialization error: " + error);
//             }
//         }
//         else
//         {
//             // Check if banner is already loaded
//             if (Appodeal.isLoaded(Appodeal.BANNER))
//             {
//                 ShowBanner();
//             }
//             else
//             {
//                 Debug.Log("Banner ad is not loaded yet.");
//                 
//                 StartCoroutine(RetryBanner()); // Retry loading if not loaded
//             }
//
//             Appodeal.setSmartBanners(true);  // Enable smart banners
//             Appodeal.setBannerCallbacks(this); // Register banner callbacks
//         }
//     }
//     
//     IEnumerator RetryBanner()
//     {
//         // Wait for a few seconds before retrying to show banner
//         yield return new WaitForSeconds(10);
//
//         if (Appodeal.isLoaded(Appodeal.BANNER))
//         {
//             ShowBanner();
//         }
//         else
//         {
//             Debug.Log("Banner still not loaded. Retrying...");
//             if (retryCount < 5) // Limit retries
//             {
//                 ShowBanner();
//                 retryCount++;
//                 StartCoroutine(RetryBanner());
//             }
//             else
//             {
//                 Debug.Log("Max retry attempts reached.");
//                 yield break;
//             }
//         }
//     }
//     
//     void ShowBanner()
//     {
//         Appodeal.show(Appodeal.BANNER_BOTTOM);  // Show banner ad at the bottom
//     }
//
//     // IBannerAdListener implementation
//     public void onBannerLoaded(int height, bool isPrecache) {
//         Debug.Log("Banner ad loaded successfully");
//       //  ShowBanner(); // Show banner when loaded
//     }
//
//     public void onBannerFailedToLoad() {
//         Debug.Log("Banner ad failed to load");
//         if (retryCount < 5)  // Retry up to 5 times
//         {
//             StartCoroutine(RetryBanner());
//         }
//     }
//
//     public void onBannerShown() {
//         Debug.Log("Banner ad is shown");
//     }
//
//     public void onBannerShowFailed()
//     {
//         Debug.Log("Banner failed to show");
//     }
//
//     public void onBannerClicked() {
//         Debug.Log("Banner ad clicked");
//     }
//
//     public void onBannerExpired() {
//         Debug.Log("Banner ad expired");
//     }
// }
