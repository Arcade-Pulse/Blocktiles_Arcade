using Firebase;
using Firebase.RemoteConfig;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseSettings : MonoBehaviour
{
    public static FirebaseSettings Instance { get; private set; }

    public  bool ShowInterstitialAds { get; private set; }
    public  bool ShowRewardedAds { get; private set; }
    public  bool ShowRewardedInterstitialAds { get; private set; }
    
    public int numberItCanEnterWithNoLives { get; private set; }

    private FirebaseApp app;

    private void Awake()
    {
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

    private void OnEnable()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                InitializeFirebaseRemoteConfig();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }
    
    private void InitializeFirebaseRemoteConfig()
    {
        // Set default values
        Dictionary<string, object> defaults = new Dictionary<string, object>();
        defaults.Add("show_interstitial_ads", true);
        defaults.Add("show_rewarded_ads", true);
        defaults.Add("show_rewarded_interstitial_ads", true);
        defaults.Add("times_it_can_enter_with_no_lives", 3); // Default value

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWith(task =>
        {
            // Set the config settings with a custom fetch interval
            ConfigSettings configSettings = new ConfigSettings
            {
                MinimumFetchIntervalInMilliseconds = 900000 // Fetch new values every 1 hour (3600 seconds)
            };

            // Apply the config settings
            FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings).ContinueWith(t => 
            {
                FetchAndActivateRemoteConfig();
            });
        });
    }
    
    private void FetchAndActivateRemoteConfig()
    {
        FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Remote Config fetched and activated.");
                ApplyRemoteConfig();
            }
            else
            {
                Debug.LogError("Failed to fetch and activate Remote Config.");
            }
        });
    }

    private void ApplyRemoteConfig()
    {
        ShowInterstitialAds = FirebaseRemoteConfig.DefaultInstance.GetValue("show_interstitial_ads").BooleanValue;
        ShowRewardedAds = FirebaseRemoteConfig.DefaultInstance.GetValue("show_rewarded_ads").BooleanValue;
        ShowRewardedInterstitialAds = FirebaseRemoteConfig.DefaultInstance.GetValue("show_rewarded_interstitial_ads").BooleanValue;
        numberItCanEnterWithNoLives = (int)FirebaseRemoteConfig.DefaultInstance.GetValue("times_it_can_enter_with_no_lives").LongValue;

        Debug.Log($"Show Interstitial Ads: {ShowInterstitialAds}");
        Debug.Log($"Show Rewarded Ads: {ShowRewardedAds}");
        Debug.Log($"Show Rewarded Interstitial Ads: {ShowRewardedInterstitialAds}");

        // You can still implement any immediate action based on these values if needed
    }
}
