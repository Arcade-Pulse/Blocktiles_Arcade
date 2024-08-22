using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityEditor.Minesweeper.Scripts.UnityAds
{
    public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] string _androidAdUnitId = "Interstitial_Android";
        [SerializeField] string _iOSAdUnitId = "Interstitial_iOS";
        private string _adUnitId = null; // This will remain null for unsupported platforms
        public static InterstitialAds Instance { get; private set; }

        public bool isAdLoaded;

        void Awake()
        {   
            // Get the Ad Unit ID for the current platform:
            #if UNITY_IOS
                _adUnitId = _iOSAdUnitId;
            #elif UNITY_ANDROID
                _adUnitId = _androidAdUnitId;
            #endif
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Call this public method when you want to get an ad ready to show.
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Interstitial Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads, show it immediately:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Interstitial Ad Loaded: " + adUnitId);
            isAdLoaded = true;
           // EnableCashoutButton();
            // ShowAd();
        }
        // public void EnableCashoutButton()
        // {
        //     if (SceneManager.GetActiveScene().name=="Menu")
        //     {
        //         var mainmenu = FindObjectOfType<MenuManager>();
        //         mainmenu.cashOutButton.interactable = enabled;
        //     }
        // }

        // Implement a method to show the interstitial ad:
        public void ShowAd()
        {
            Debug.Log("Showing Interstitial Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method if you need to take action after the ad is shown:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            isAdLoaded = false;
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Unity Ads Interstitial Ad Completed");
                // Take any action after the ad is completed, if needed.
            }
        }

        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Interstitial Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Interstitial Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to show the ad again or load another ad.
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
            
        }
        public void OnUnityAdsShowClick(string adUnitId) { }

        void OnDestroy()
        {
            // Clean up if needed
        }
    }
}
