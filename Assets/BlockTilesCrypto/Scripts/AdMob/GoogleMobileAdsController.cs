// using System.Collections.Generic;
// using UnityEngine;
// using GoogleMobileAds.Api;
// using GoogleMobileAds.Ump.Api;
// // using GoogleMobileAds.Mediation.AdColony.Api;
// // using GoogleMobileAds.Mediation.AppLovin.Api;
// // using GoogleMobileAds.Mediation.InMobi.Api;
// // using GoogleMobileAds.Mediation.LiftoffMonetize.Api;
// using System;
// using System.Collections;
// using System.IO;
// using GoogleMobileAds.Api.Mediation.InMobi;
// using GoogleMobileAds.Api.Mediation.LiftoffMonetize;
// using GoogleMobileAds.Api.Mediation.UnityAds;
// using UnityEngine.Networking;
// using UnityEngine.SceneManagement;
//
// [AddComponentMenu("GoogleMobileAds/GoogleMobileAdsController")]
// public class GoogleMobileAdsController : MonoBehaviour
// {
//     public static GoogleMobileAdsController Instance { get; private set; }
//    // public static bool DevelopmentMode { get; set; } = false;
//
//     private static bool? _isInitialized;
//
//     // Use RequestConfiguration.Builder().setTestDeviceIds(Arrays.asList("838650C16D6454B87A42CF28C46886C0")) to get test ads on this device.
//
//     internal static List<string> TestDeviceIds = new()
//     {
//         AdRequest.TestDeviceSimulator,
// #if UNITY_IPHONE
//         "96e23e80653bb28980d3f40beb58915c",
// #elif UNITY_ANDROID
//         "838650C16D6454B87A42CF28C46886C0"
// #endif
//     };
//
//     private GoogleMobileAdsConsentController consentController;
//     public static event EventHandler OnAdReadyToServe;
//
//     private void OnEnable()
//     {
//         DontDestroyOnLoad(this);
//     }
//
//     private void Awake()
//     {
//
//         consentController = GetComponent<GoogleMobileAdsConsentController>();
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
//         InAppUpdateManager.OnUpdateAvailable += InitializedAds_OnUpdateNotAvailable;
//     }
//     
//     private void InitializedAds_OnUpdateNotAvailable(object sender, InAppUpdateManager.OnUpdateAvailableEventArgs e)
//     {
//        // Debug.Log("Is Update Available: " + e.isUpdateAvailable);
//         if (e.isUpdateAvailable) return;
//         
//         //AdsConfiguration();
//         InAppUpdateManager.OnUpdateAvailable -= InitializedAds_OnUpdateNotAvailable;
//         SceneManager.LoadScene("AuthenticationScene");
//     }
//
//     public void AdsConfiguration()
//     {
//         // On Android, Unity is paused when displaying interstitial or rewarded video.
//         // This setting makes iOS behave consistently with Android.
//         MobileAds.SetiOSAppPauseOnBackground(true);
//
//         // When true all events raised by GoogleMobileAds will be raised
//         // on the Unity main thread. The default value is false.
//         // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
//         MobileAds.RaiseAdEventsOnUnityMainThread = true;
//
//         // Configure your RequestConfiguration with Child Directed Treatment
//         // and the Test Device Ids.
//         // MobileAds.SetRequestConfiguration(new RequestConfiguration
//         // {
//         //     TestDeviceIds = TestDeviceIds
//         // });
//
//         // ResetConsent();
//
//         // If we can request ads, we should initialize the Google Mobile Ads Unity plugin.
//         if (consentController.CanRequestAds)
//         {
//         InitializeGoogleMobileAds();
//         }
//
//         // Ensures that privacy and consent information is up to date.
//          InitializeGoogleMobileAdsConsent();
//     }
//
//     // public void ResetConsent()
//     // {
//     //     var playerId = PlayFabDataManager.Instance.playfabID;
//     //     var listContainsId = PlayFabDataManager.Instance.debuggersList.Contains(playerId);
//     //     if (PlayFabDataManager.Instance.debugNewUserConsentGDPR == 1 && listContainsId)
//     //     {
//     //         Debug.Log("CONSENT IS RESET");
//     //         ConsentInformation.Reset();
//     //     }
//     //     Debug.Log("can request Ads " + consentController.CanRequestAds);
//     // }
//
//     /// <summary>
//     /// Ensures that privacy and consent information is up to date.
//     /// </summary>
//     public void InitializeGoogleMobileAdsConsent()
//     {
//       //  Debug.Log("Google Mobile Ads gathering consent.");
//
//         consentController.GatherConsent((string error) =>
//         {
//             if (error != null)
//             {
//                 Debug.Log("Failed to gather consent with error: " +
//                                error);
//             }
//             else
//             {
//                // Debug.Log("Google Mobile Ads consent updated.");
//             }
//
//            // Debug.Log("can request Ads " + consentController.CanRequestAds);
//             if (consentController.CanRequestAds)
//             {
//                 InitializeGoogleMobileAds();
//             }
//         });
//     }
//
//     public void InitializeGoogleMobileAds()
//     {
//         // The Google Mobile Ads Unity plugin needs to be run only once and before loading any ads.
//         if (_isInitialized.HasValue)
//         {
//             return;
//         }
//
//         _isInitialized = false;
//
//         // Initialize the Google Mobile Ads SDK.
//      //   Debug.Log("Google Mobile Ads Initializing.");
//         MobileAds.Initialize((InitializationStatus initstatus) =>
//         {
//             if (initstatus == null)
//             {
//                 //Debug.Log("Google Mobile Ads initialization failed.");
//                 _isInitialized = null;
//                 return;
//             }
//
//             // If you use mediation, you can check the status of each adapter.
//             var adapterStatusMap = initstatus.getAdapterStatusMap();
//             if (adapterStatusMap != null)
//             {
//                 foreach (var item in adapterStatusMap)
//                 {
//                     Debug.Log(string.Format("Adapter {0} is {1}",
//                         item.Key,
//                         item.Value.InitializationState));
//                 }
//             }
//
//          //   Debug.Log("Google Mobile Ads initialization complete.");
//             _isInitialized = true;
//            // StartCoroutine(LoadConfiguration());
//            LoadAds();
//             OnAdReadyToServe?.Invoke(this, EventArgs.Empty);
//         });
//
//          ConsentGDPR();
//     }
//
//     public void LoadAds()
//     {
//         RewardedInterstitialAdController.Instance.LoadAd();
//         BannerViewController.Instance.LoadAd();
//         
//     }
//     
//     private IEnumerator LoadConfiguration()
//     {
//         string path = Path.Combine(Application.streamingAssetsPath, "config.json");
//         string json = string.Empty;
//
//         if (Application.platform == RuntimePlatform.Android)
//         {
//             UnityWebRequest request = UnityWebRequest.Get(path);
//             yield return request.SendWebRequest();
//
//             if (request.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError("Configuration file not found at path: " + path);
//                 yield break;
//             }
//
//             json = request.downloadHandler.text;
//         }
//         else
//         {
//             if (!File.Exists(path))
//             {
//                 Debug.LogError("Configuration file not found at path: " + path);
//                 yield break;
//             }
//
//             json = File.ReadAllText(path);
//         }
//
//         var config = JsonUtility.FromJson<PlayIntegrityManager.Configuration>(json);
//         AdsID.adAppOpenID = config.app_open_android;
//         AdsID.adBannerID = config.banner_android;
//         AdsID.adInterstitialID = config.interstitial_android;
//         AdsID.adRewardedID = config.rewarded_android;
//         AdsID.adRewardedInterstitialID = config.rewarded_interstitial;
//         
//      //  Debug.Log("rewarded ID:"+AdsID.adRewardedID);
//
//        //Debug.Log("Configuration loaded successfully.");
//         OnAdReadyToServe?.Invoke(this, EventArgs.Empty);
//         InterstitialAdController.Instance.LoadAd();
//         //RewardedInterstitialAdController.Instance.LoadAd();
//         
//         // BannerViewController.Instance.LoadAd();
//
//     }
//
//     private void ConsentGDPR()
//     {
//         UnityAds.SetConsentMetaData("privacy.consent", true);
//         UnityAds.SetConsentMetaData("gdpr.consent", true);
//
//
//         Dictionary<string, string> consentObject = new Dictionary<string, string>();
//         consentObject.Add("gdpr_consent_available", "true");
//         consentObject.Add("gdpr", "1");
//
//         InMobi.UpdateGDPRConsent(consentObject);
//
//         // LiftoffMonetize.UpdateConsentStatus(VungleConsentStatus.OPTED_IN, "1.0.0");
//         // LiftoffMonetize.UpdateCCPAStatus(VungleCCPAStatus.OPTED_IN);
//     }
//
//     public void OpenAdInspector()
//     {
//        // Debug.Log("Opening ad Inspector.");
//         MobileAds.OpenAdInspector((AdInspectorError error) =>
//         {
//             // If the operation failed, an error is returned.
//             if (error != null)
//             {
//                 Debug.Log("Ad Inspector failed to open with error: " + error);
//                 return;
//             }
//
//    //         Debug.Log("Ad Inspector opened successfully.");
//         });
//     }
// }
