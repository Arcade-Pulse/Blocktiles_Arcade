// using UnityEngine;
// using UnityEngine.Advertisements;
//
// namespace UnityEditor.Minesweeper.Scripts.UnityAds
// {
//     public class BannerAds : MonoBehaviour
//     {
//         [SerializeField] string _androidAdUnitId = "Banner_Android";
//         [SerializeField] string _iOSAdUnitId = "Banner_iOS";
//         private string _adUnitId = null; // This will remain null for unsupported platforms
//         public static BannerAds Instance { get; private set; }
//
//         public bool isAdLoaded;
//         void Awake()
//         {   
//             // Get the Ad Unit ID for the current platform:
//             #if UNITY_IOS
//                 _adUnitId = _iOSAdUnitId;
//             #elif UNITY_ANDROID
//                 _adUnitId = _androidAdUnitId;
//             #endif
//             if (Instance == null)
//             {
//                 Instance = this;
//                 DontDestroyOnLoad(gameObject);
//             }
//             else
//             {
//                 Destroy(gameObject);
//             }
//         }
//
//         // Call this method to load and show the banner ad
//         // public void LoadBanner()
//         // {
//         //     // Set the banner position:
//         //     Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
//         //
//         //     // Load and then show the banner ad:
//         //     Advertisement.Banner.Load(_adUnitId, new BannerLoadOptions
//         //     {
//         //         loadCallback = OnBannerLoaded,
//         //         errorCallback = OnBannerError
//         //     });
//         // }
//
//         // Callback for when the banner is loaded:
//         void OnBannerLoaded()
//         {
//             Debug.Log("Banner loaded successfully");
//
//             isAdLoaded = true;
//             // Show the banner ad:
//             ShowAd();
//             //Advertisement.Banner.Show(_adUnitId);
//         }
//
//         public void ShowAd()
//         {
//             Advertisement.Banner.Show(_adUnitId);
//         }
//
//         // Callback for when the banner fails to load:
//         void OnBannerError(string message)
//         {
//            // Debug.LogError($"Banner failed to load: {message}");
//             // Handle the error, maybe retry loading the banner ad or display a placeholder.
//         }
//
//         // Call this method to hide the banner ad
//         public void HideBanner()
//         {
//             Advertisement.Banner.Hide();
//         }
//     }
// }
