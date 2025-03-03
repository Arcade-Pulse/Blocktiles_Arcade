// using System.Collections;
// using System.Collections.Generic;
// using AppodealAds.Unity.Api;
// using AppodealAds.Unity.Common;
// using Firebase.Analytics;
// using UnityEngine;
//
// public class Interstitial : MonoBehaviour, IInterstitialAdListener, IAdRevenueListener
// {
//     public static Interstitial Instance { get; private set; }
//     private float lastInterstitialTime = 0f;  // Tracks last interstitial time
//     private const float interstitialCooldown = 90f; // 90 seconds cooldown
//     public bool isAdLoaded;
//
//     private void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(this);
//             return;
//         }
//
//         Instance = this;
//         DontDestroyOnLoad(this);
//
//         // Register ad callbacks (Only once)
//         Appodeal.setInterstitialCallbacks(this);
//         Appodeal.setAdRevenueCallback(this);
//
//         // Preload the first ad
//         LoadInterstitialAd();
//     }
//
//     public void ShowAd()
//     {          
//         if (Time.time - lastInterstitialTime < interstitialCooldown)
//         {
//             Debug.Log("Interstitial cooldown is active, skipping ad request.");
//             return;
//         }
//
//         if (Appodeal.isLoaded(Appodeal.INTERSTITIAL)) 
//         {
//             Appodeal.show(Appodeal.INTERSTITIAL);
//             lastInterstitialTime = Time.time; // Update cooldown time
//         }
//         else
//         {
//             Debug.Log("Interstitial not loaded. Caching now...");
//             LoadInterstitialAd();
//         }
//     }
//
//     public void LoadInterstitialAd()
//     {
//         if (!Appodeal.isLoaded(Appodeal.INTERSTITIAL))
//         {
//             Appodeal.cache(Appodeal.INTERSTITIAL);
//             Debug.Log("🔄 Interstitial Ad is being loaded.");
//         }
//         else
//         {
//             Debug.Log("✅ Interstitial Ad is already loaded.");
//         }
//     }
//     
//     #region Interstitial callback handlers
//
//     public void onInterstitialLoaded(bool isPrecache)
//     {
//         Debug.Log("✅ Interstitial loaded.");
//         isAdLoaded = true;
//     }
//
//     public void onInterstitialFailedToLoad()
//     {
//         Debug.Log("❌ Interstitial failed to load.");
//         isAdLoaded = false;
//         StartCoroutine(RetryLoadInterstitial()); // Retry after delay
//     }
//
//     public void onInterstitialShowFailed()
//     {
//         Debug.Log("❌ Interstitial show failed.");
//     }
//
//     public void onInterstitialShown()
//     {
//         Debug.Log("▶️ Interstitial shown.");
//         isAdLoaded = false;
//     }
//
//     public void onInterstitialClosed()
//     {
//         Debug.Log("🔙 Interstitial closed. Preloading next ad.");
//         LoadInterstitialAd(); // Load next ad after closing
//     }
//
//     public void onInterstitialClicked()
//     {
//         Debug.Log("🖱️ Interstitial clicked.");
//     }
//
//     public void onInterstitialExpired()
//     {
//         Debug.Log("⏳ Interstitial expired, reloading...");
//         isAdLoaded = false;
//         LoadInterstitialAd();
//     }
//
//     public void onAdRevenueReceived(AppodealAdRevenue adRevenue)
//     {
//         Debug.Log($"💰 Ad Revenue Received: {adRevenue.Revenue} {adRevenue.Currency}");
//
//         FirebaseAnalytics.LogEvent("custom_ad_impression", new Parameter[]
//         {
//             new Parameter("ad_platform", "Appodeal"),
//             new Parameter("ad_source", adRevenue.NetworkName),
//             new Parameter("ad_format", adRevenue.AdUnitName),
//             new Parameter("ad_placement", adRevenue.Placement),
//             new Parameter("ad_revenue", adRevenue.Revenue),
//             new Parameter("currency", adRevenue.Currency),
//             new Parameter("precision", adRevenue.RevenuePrecision) 
//         });
//     }
//
//     IEnumerator RetryLoadInterstitial()
//     {
//         Debug.Log("⏳ Retrying interstitial load in 15 seconds...");
//         yield return new WaitForSeconds(15);
//         LoadInterstitialAd();
//     }
//
//     #endregion
// }
