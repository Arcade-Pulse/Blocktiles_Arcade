// using System.Collections;
// using AppodealAds.Unity.Common;
// using UnityEngine;
// using AppodealAds.Unity.Api;
//
// public class Rewarded : MonoBehaviour, IRewardedVideoAdListener
// {
//     public static Rewarded Instance { get; private set; }
//     public bool isAdLoaded;
//     private float lastRewardedAdTime = 0f; // Tracks last ad time
//     private const float rewardedAdCooldown = 60f; // 60 seconds cooldown
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
//         // Register rewarded video callbacks
//         Appodeal.setRewardedVideoCallbacks(this);
//
//         // Preload first rewarded ad
//         LoadRewardedAd();
//     }
//
//     public void ShowAd()
//     {
//         if (Time.time - lastRewardedAdTime < rewardedAdCooldown)
//         {
//             Debug.Log("⏳ Rewarded ad cooldown active, skipping request.");
//             return;
//         }
//
//         if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
//         {
//             Appodeal.show(Appodeal.REWARDED_VIDEO);
//             lastRewardedAdTime = Time.time; // Update cooldown time
//         }
//         else
//         {
//             Debug.Log("❌ Rewarded ad not loaded, requesting now...");
//             LoadRewardedAd();
//         }
//     }
//
//     public void LoadRewardedAd()
//     {
//         if (!Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
//         {
//             Debug.Log("🔄 Requesting new Rewarded Ad...");
//             Appodeal.cache(Appodeal.REWARDED_VIDEO);
//         }
//         else
//         {
//             Debug.Log("✅ Rewarded Ad is already cached.");
//             isAdLoaded = true;
//         }
//     }
//
//     #region Rewarded Video Callbacks
//
//     public void onRewardedVideoLoaded(bool precache)
//     {
//         Debug.Log("✅ Rewarded Video Loaded.");
//         isAdLoaded = true;
//     }
//
//     public void onRewardedVideoFailedToLoad()
//     {
//         Debug.Log("❌ Rewarded Video Failed to Load.");
//         isAdLoaded = false;
//         StartCoroutine(RetryLoadRewardedAd());
//     }
//
//     public void onRewardedVideoShowFailed()
//     {
//         Debug.Log("❌ Rewarded Video Show Failed.");
//     }
//
//     public void onRewardedVideoShown()
//     {
//         Debug.Log("▶️ Rewarded Video Shown.");
//         isAdLoaded = false;  // Mark as not available until reloaded
//     }
//
//     public void onRewardedVideoFinished(double amount, string name)
//     {
//         Debug.Log($"🎉 Rewarded Video Finished! Player received {amount} {name}.");
//         LoadRewardedAd(); // Load next ad
//     }
//
//     public void onRewardedVideoClosed(bool finished)
//     {
//         Debug.Log($"🔚 Rewarded Video Closed. Completed: {finished}");
//         if (finished)
//         {
//             LoadRewardedAd(); // Reload only if the ad was completed
//         }
//     }
//
//     public void onRewardedVideoExpired()
//     {
//         Debug.Log("⏳ Rewarded Video Expired. Requesting new ad...");
//         isAdLoaded = false;
//         LoadRewardedAd();
//     }
//
//     public void onRewardedVideoClicked()
//     {
//         Debug.Log("🖱️ Rewarded Video Clicked.");
//     }
//
//     IEnumerator RetryLoadRewardedAd()
//     {
//         Debug.Log("⏳ Retrying rewarded ad load in 15 seconds...");
//         yield return new WaitForSeconds(15);
//         LoadRewardedAd();
//     }
//
//     #endregion
// }
