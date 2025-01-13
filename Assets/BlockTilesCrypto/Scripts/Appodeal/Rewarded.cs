// using AppodealAds.Unity.Common;
//
// using UnityEngine;
// using AppodealAds.Unity.Api;
// using PlayFabPersonal.Economy;
// using PlayFabPersonal.Managers;
//
// public class Rewarded:MonoBehaviour,IRewardedVideoAdListener
//     {
//         public static Rewarded Instance { get; private set; }
//         public bool isAdLoaded;
//
//         public void ShowAd()
//         {
//             
//             if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO)) {
//                 Appodeal.show(Appodeal.REWARDED_VIDEO);
//             }
//             else
//             {
//                 LoadRewardedAd();
//             }
//         }
//         
//         public void LoadRewardedAd()
//         {
//             if (!AppodealAds.Unity.Api.Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
//             {
//                 // Ad is not loaded, so we need to load it
//                 AppodealAds.Unity.Api.Appodeal.cache(Appodeal.REWARDED_VIDEO);
//                 Debug.Log("rewarded Ad is being loaded.");
//             }
//             else
//             {
//                 // Ad is already loaded
//                 Debug.Log("rewarded Ad is already loaded.");
//                 isAdLoaded = true;
//             }
//         }
//         private void Awake()
//         {
//             // If there is an instance, and it's not me, delete myself.
//
//             if (Instance != null && Instance != this)
//             {
//                 Destroy(this);
//             }
//             else
//             {
//                 Instance = this;
//             }
//
//             DontDestroyOnLoad(this);
//
//
//         }
//         public void onRewardedVideoLoaded(bool precache)
//         {
//             Debug.Log("rewarded video loaded");
//
//             isAdLoaded = true;
//         }
//
//         public void onRewardedVideoFailedToLoad()
//         {
//             Debug.Log("rewarded video failed to load");
//             isAdLoaded = false;
//
//         }
//
//         public void onRewardedVideoShowFailed()
//         {
//             Debug.Log("rewarded video show failed");
//         }
//
//         public void onRewardedVideoShown()
//         {
//             Debug.Log("rewarded video shown ");
//            // VirtualCurrency.Instance.AddLife(PlayfabDataManager.Instance.GetLifeRewardPerAd());
//             LoadRewardedAd();
//         }
//
//         public void onRewardedVideoFinished(double amount, string name)
//         {
//             Debug.Log("rewarded video show finished");
//             LoadRewardedAd();
//
//         }
//
//         public void onRewardedVideoClosed(bool finished)
//         {
//             Debug.Log("rewarded video show closed");
//             LoadRewardedAd();
//
//         }
//
//         public void onRewardedVideoExpired()
//         {
//             Debug.Log("rewarded video expired");
//         }
//
//         public void onRewardedVideoClicked()
//         {
//             Debug.Log("rewarded video clicked");
//         }
//     }