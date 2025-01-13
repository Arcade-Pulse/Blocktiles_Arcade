// using System;
// using System.Collections;
// using System.Collections.Generic;
// using AppodealAds.Unity.Api;
// using AppodealAds.Unity.Common;
// using UnityEngine;
//
// public class Interstitial : MonoBehaviour,IInterstitialAdListener
// {
//     public static Interstitial Instance { get; private set; }
//
//
//     public bool isAdLoaded;
//     public void ShowAd()
//     {          
//         if(Appodeal.isLoaded(Appodeal.INTERSTITIAL)) {
//             Appodeal.show(Appodeal.INTERSTITIAL);
//         }
//         else
//         {
//             LoadInterstitialAd();
//         }
//     }
//
//     private void OnEnable()
//     {
//         Appodeal.setInterstitialCallbacks(this);
//
//     }
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
//
//
//     }
//
//     public void LoadInterstitialAd()
//     {
//         if (!Appodeal.isLoaded(Appodeal.INTERSTITIAL))
//         {
//             // Ad is not loaded, so we need to load it
//             Appodeal.cache(Appodeal.INTERSTITIAL);
//             Debug.Log("Interstitial Ad is being loaded.");
//         }
//         else
//         {
//             // Ad is already loaded
//             Debug.Log("Interstitial Ad is already loaded.");
//         }
//     }
//     
//     #region Interstitial callback handlers
//
// // Called when interstitial was loaded (precache flag shows if the loaded ad is precache)
//     public void onInterstitialLoaded(bool isPrecache)
//     {
//         Debug.Log("Interstitial loaded");
//     }
//
// // Called when interstitial failed to load
//     public void onInterstitialFailedToLoad()
//     {
//         Debug.Log("Interstitial failed to load");
//     }
//
// // Called when interstitial was loaded, but cannot be shown (internal network errors, placement settings, etc.)
//     public void onInterstitialShowFailed()
//     {
//         Debug.Log("Interstitial show failed");
//     }
//
// // Called when interstitial is shown
//     public void onInterstitialShown()
//     {
//         Debug.Log("Interstitial shown");
//     }
//
// // Called when interstitial is closed
//     public void onInterstitialClosed()
//     {
//         Debug.Log("Interstitial closed");
//     }
//
// // Called when interstitial is clicked
//     public void onInterstitialClicked()
//     {
//         Debug.Log("Interstitial clicked");
//     }
//
// // Called when interstitial is expired and can not be shown
//     public void onInterstitialExpired()
//     {
//         Debug.Log("Interstitial expired");
//     }
//
//     #endregion
// }
