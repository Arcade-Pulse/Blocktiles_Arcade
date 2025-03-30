
using CodeStage.AntiCheat.ObscuredTypes;
using Firebase.Analytics;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Managers;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdLevelPlay : MonoBehaviour
{
    string appKey = "1f9d90035";
    public bool isBannerLoaded;
    public bool isInterLoaded;
    public bool isRewardedLoaded;
    
    // Ad request tracking for interstitials
    private float lastInterstitialTime = 0f;
    private int consecutiveInterstitialRequests = 0;
    private const float interstitialCooldown = 3.0f; // Max time between requests
    private const int maxConsecutiveRequests = 3; // Maximum consecutive interstitial requests before blocking
    private const float blockDuration = 10.0f; // Duration to block further ad requests
    private bool isBlocked = false;
    private float blockEndTime = 0f;

    public static AdLevelPlay Instance { get; private set; }
    private void Awake()
    {
             // If there is an instance, and it's not me, delete myself.

             if (Instance != null && Instance != this)
             {
                 Destroy(this);
             }
             else
             {
                 Instance = this;
             }

             DontDestroyOnLoad(this);


    }

    void Start()
    {
        // Initialize SDK with ad units
        // IronSource.Agent.setMetaData("is_test_suite", "enable"); 
        // IronSource.Agent.setAdaptersDebug(true);

        IronSource.Agent.setConsent(true);
        IronSource.Agent.setMetaData("is_test_suite", "enable"); 

        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitCompleted;


        // Register Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        // IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
         IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

        // Register Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;

        // Register Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

        // Load Ads
       // IronSource.Agent.launchTestSuite();
     //   Debug.Log("loading ads");
        
    }

    private void SdkInitCompleted()
    {
     //   IronSource.Agent.launchTestSuite();
        IronSource.Agent.loadInterstitial();
        IronSource.Agent.loadRewardedVideo();
        LoadBannerAd();
        IronSourceAdQuality.Initialize(appKey);

        IronSource.Agent.launchTestSuite();
    }

    // Rewarded Video Event Handlers
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
      //  Debug.Log("Rewarded Video ad opened");
      TrackAdRevenue("rewarded", adInfo);
      isRewardedLoaded = false;
    }

    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        //Debug.Log("Rewarded Video ad closed");
    }

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
      //  Debug.Log("Rewarded Video is available");
        isRewardedLoaded = true;
    }

    void RewardedVideoOnAdUnavailable()
    {
        //Debug.Log("Rewarded Video is unavailable");
    }

    void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        //Debug.Log("Rewarded Video ad show failed: " + ironSourceError.getDescription());
    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement)
    {
        //Debug.Log("User rewarded with: " + placement.getRewardAmount() + " " + placement.getRewardName());
    }

    void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo ironSourceAdInfo)
    {
        //Debug.Log("Rewarded Video ad clicked");
        TrackAdClicks("rewarded",ironSourceAdInfo);

    }
    

    public void ShowRewarded()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            isRewardedLoaded = false;
            IronSource.Agent.loadRewardedVideo();
        }
    }
    
    public void ShowInterstitial()
    {
        // If the user is temporarily blocked, prevent showing ads
        if (isBlocked && Time.time < blockEndTime)
        {
         //   Debug.LogWarning("🚨 Interstitial ad request blocked due to excessive requests.");
            return;
        }

        float currentTime = Time.time;
        
        // Check if the new request is within 3 seconds of the last request
        if (currentTime - lastInterstitialTime < interstitialCooldown)
        {
            consecutiveInterstitialRequests++;
          //  Debug.LogWarning($"⚠️ Interstitial requested too fast! Count: {consecutiveInterstitialRequests}");

            // If the user reaches the limit, block them from showing ads temporarily
            if (consecutiveInterstitialRequests >= maxConsecutiveRequests)
            {
                //Debug.LogError("🚨 Excessive Interstitial Ad Requests Detected! Blocking ads for 10 seconds.");
                Firebase.Analytics.FirebaseAnalytics.LogEvent("excessive_ad_requests");

                isBlocked = true;
                blockEndTime = Time.time + blockDuration; // Block further ads
                consecutiveInterstitialRequests = 0; // Reset the request counter
                banUser();
                return;
            }
        }
        else
        {
            consecutiveInterstitialRequests = 0; // Reset if requests are spaced out properly
        }

        lastInterstitialTime = currentTime; // Update last request time

        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            isInterLoaded = false;
            IronSource.Agent.loadInterstitial();
        }
    }

    // public void ShowInterstitial()
    // {
    //     if (IronSource.Agent.isInterstitialReady())
    //     {
    //         IronSource.Agent.showInterstitial();
    //     }
    //     else
    //     {
    //         isInterLoaded = false;
    //         IronSource.Agent.loadInterstitial(); // Ensure the ad is being loaded
    //     }
    // }
    // Interstitial Event Handlers
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
      //  Debug.Log("Interstitial ad is ready");
        isInterLoaded = true;
    }

    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
        //Debug.Log("Interstitial ad failed to load: " + ironSourceError.getDescription());
    }

    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        //Debug.Log("Interstitial ad opened");
    }

    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
       // Debug.Log("Interstitial ad clicked");
      TrackAdClicks("interstitial",adInfo);

    }

    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
      //  Debug.Log("Interstitial ad show succeeded");
      TrackAdRevenue("interstitial", adInfo);
    }
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo) { Debug.Log("Interstitial ad show failed: " + ironSourceError.getDescription()); }

    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
      //  Debug.Log("Interstitial ad closed"); 
        IronSource.Agent.loadInterstitial();
    }

    // Banner Ads
    void LoadBannerAd()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.LARGE, IronSourceBannerPosition.BOTTOM);
    }

    public void ShowBannerAd()
    {

        IronSource.Agent.displayBanner();
    }

    public void HideBannerAd()
    {
        IronSource.Agent.hideBanner();
    }

    void DestroyBannerAd()
    {
        IronSource.Agent.destroyBanner();
    }

    // Banner Event Handlers
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
       // Debug.Log("Banner ad loaded");
        isBannerLoaded = true;
    }
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError) { Debug.Log("Banner ad load failed: " + ironSourceError.getDescription()); }

    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
       // Debug.Log("Banner ad clicked");
       TrackAdClicks("banner",adInfo);

    }

    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
     //   Debug.Log("Banner ad screen presented");
    }

    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
      //  Debug.Log("Banner ad screen dismissed");
    }

    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
      //  Debug.Log("Banner ad left application");
    }

    // Clean up event subscriptions
    void OnDestroy()
    {
        // Unsubscribe Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
        // IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
        // IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;

        // Unsubscribe Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent -= InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent -= InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent -= InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent -= InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;

        // Unsubscribe Banner Events
        IronSourceBannerEvents.onAdLoadedEvent -= BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent -= BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent -= BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent -= BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent -= BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent -= BannerOnAdLeftApplicationEvent;
    }
    
    
    public void banUser() {
        
        ObscuredString funcName1 = "banUser";
        var requestCurrentChallenge = new ExecuteCloudScriptRequest
        {
            FunctionName = funcName1,
            GeneratePlayStreamEvent = true,
            RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision()
        };
        
        PlayFabClientAPI.ExecuteCloudScript(requestCurrentChallenge, result => {
            // if (result.Error != null) {
            //     Debug.LogError(result.Error.Message);
            //     return;
            // }

            var functionResult = JsonUtility.FromJson<FunctionResult>(result.FunctionResult.ToString());
            if (functionResult.expression != null) {
                PlayFabClientAPI.ForgetAllCredentials();
                Application.Quit();
            } else {
                PlayFabClientAPI.ForgetAllCredentials();
                Application.Quit();
            }
        }, error => {
            PlayFabClientAPI.ForgetAllCredentials();
            Application.Quit();
        });
    }
    
    
    // 📌 Revenue Tracking Function
    void TrackAdRevenue(string adType, IronSourceAdInfo adInfo)
    {
        if (adInfo.revenue != null)
        {
            double revenue = (double)adInfo.revenue; // Retrieves ILR revenue
            if (revenue > 0)
            {
                FirebaseAnalytics.LogEvent("custom_ad_impression", new[]
                {
                    new Parameter("value", revenue), // Actual revenue amount
                    new Parameter("currency", "USD"), // Ensure it's in USD for Google Ads tracking
                    new Parameter("ad_type", adType),
                    new Parameter("network", adInfo.adNetwork),
                    new Parameter("placement", adInfo.adUnit)
                });
                FirebaseAnalytics.LogEvent("ad_impression", new []
                {
                    new Parameter("value", revenue),
                    new Parameter("currency", "USD"),
                    new Parameter("ad_type", adType)
                });

                

               // Debug.Log($"Ad Revenue Tracked: ${revenue} | Type: {adType} | Network: {adInfo.adNetwork}");
            }
            else
            {
              //  Debug.LogWarning("Ad revenue is 0. ILR may not be enabled.");
            }
        }
    }
    
    
    void TrackAdClicks(string adType, IronSourceAdInfo adInfo)
    {
        //Debug.Log("Rewarded Video ad clicked");
        FirebaseAnalytics.LogEvent("ad_click", new Parameter[]
        {
            new Parameter("ad_type", adType),
            new Parameter("network", adInfo.adNetwork)
        });

    }
    
}
