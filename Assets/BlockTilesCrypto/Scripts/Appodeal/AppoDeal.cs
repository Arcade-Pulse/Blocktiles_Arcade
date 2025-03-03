// using System.Collections;
// using System.Collections.Generic;
// using AppodealAds.Unity.Api;
// using AppodealAds.Unity.Common;
// using UnityEngine;
//
// public class AppoDeal : MonoBehaviour, IAppodealInitializationListener, IBannerAdListener
// {
//     public int retryCount = 0;
//     private const int MAX_RETRIES = 3;
//     private bool bannerLoaded = false;  
//     private bool bannerShown = false;   // ✅ NEW: Tracks if banner is currently displayed
//
//     void Awake()
//     {
//         DontDestroyOnLoad(this.gameObject);
//     }
//
//     private void Start()
//     {
//         Debug.Log("Starting Appodeal");
//         int adTypes = Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO;
//         string appKey = "6d583d1437e6507603b18553f90495857e50c9748a7f1d74";
//
//         Appodeal.setSmartBanners(true);
//         Appodeal.initialize(appKey, adTypes, this);
//
//         Debug.Log("Caching banner...");
//         Appodeal.cache(Appodeal.BANNER);
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
//             Appodeal.setBannerCallbacks(this); 
//
//             if (Appodeal.isLoaded(Appodeal.BANNER))
//             {
//                 ShowBanner();
//             }
//             else
//             {
//                 Debug.Log("Banner ad is not loaded yet, waiting for callback.");
//             }
//         }
//     }
//
//     void ShowBanner()
//     {
//         if (!bannerLoaded || bannerShown)  // ✅ Prevents multiple calls to show the banner
//         {
//             Debug.Log("Banner is not loaded yet or already displayed.");
//             return;
//         }
//
//         Debug.Log("Showing banner...");
//         Appodeal.show(Appodeal.BANNER_BOTTOM);
//         bannerShown = true;  // ✅ Mark banner as displayed
//     }
//
//     IEnumerator RetryBanner()
//     {
//         if (retryCount >= MAX_RETRIES)
//         {
//             Debug.Log("Max retry attempts reached. Stopping banner retries.");
//             yield break;
//         }
//
//         retryCount++;
//         Debug.Log($"Retrying to load banner... Attempt {retryCount}");
//
//         yield return new WaitForSeconds(10);
//
//         if (Appodeal.isLoaded(Appodeal.BANNER))
//         {
//             ShowBanner();
//         }
//         else
//         {
//             Appodeal.cache(Appodeal.BANNER);
//             StartCoroutine(RetryBanner());
//         }
//     }
//
//     // IBannerAdListener implementation
//     public void onBannerLoaded(int height, bool isPrecache)
//     {
//         Debug.Log($"✅ Banner Loaded - Height: {height}, Precache: {isPrecache}");
//         bannerLoaded = true;
//         retryCount = 0;
//
//         if (!bannerShown)  // ✅ Only show if it's not already displayed
//         {
//             ShowBanner();
//         }
//     }
//
//     public void onBannerFailedToLoad()
//     {
//         Debug.Log("❌ Banner ad failed to load");
//         bannerLoaded = false;
//
//         if (retryCount < MAX_RETRIES)
//         {
//             StartCoroutine(RetryBanner());
//         }
//         else
//         {
//             Debug.Log("Giving up on banner retry after multiple failures.");
//         }
//     }
//
//     public void onBannerShown()
//     {
//         Debug.Log("✅ Banner ad is shown");
//         bannerShown = true;  // ✅ Mark that the banner is currently visible
//     }
//
//     public void onBannerShowFailed()
//     {
//         Debug.Log("❌ Banner failed to show");
//         bannerShown = false;
//     }
//
//     public void onBannerClicked()
//     {
//         Debug.Log("🖱️ Banner ad clicked");
//     }
//
//     public void onBannerExpired()
//     {
//         Debug.Log("🔄 Banner ad expired, reloading...");
//         bannerLoaded = false;
//         bannerShown = false;  // ✅ Reset the shown state so a new banner can be displayed
//         Appodeal.cache(Appodeal.BANNER);
//     }
// }
