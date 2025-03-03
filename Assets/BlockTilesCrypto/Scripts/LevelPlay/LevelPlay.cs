
using UnityEngine;

public class AdLevelPlay : MonoBehaviour
{
    string appKey = "1f9d90035";
    public bool isBannerLoaded;
    public bool isInterLoaded;
    public bool isRewardedLoaded;

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
        Debug.Log("loading ads");
        
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
        Debug.Log("Rewarded Video ad opened");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression");

        isRewardedLoaded = false;
    }
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo) { Debug.Log("Rewarded Video ad closed"); }

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("Rewarded Video is available");
        isRewardedLoaded = true;
    }
    void RewardedVideoOnAdUnavailable() { Debug.Log("Rewarded Video is unavailable"); }
    void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo) { Debug.Log("Rewarded Video ad show failed: " + ironSourceError.getDescription()); }
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement) { Debug.Log("User rewarded with: " + placement.getRewardAmount() + " " + placement.getRewardName()); }

    void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo ironSourceAdInfo)
    {
        Debug.Log("Rewarded Video ad clicked");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_click");

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
        }
    }

    public void ShowInterstitial()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            isInterLoaded = false;
            IronSource.Agent.loadInterstitial(); // Ensure the ad is being loaded
        }
    }
    // Interstitial Event Handlers
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("Interstitial ad is ready");
        isInterLoaded = true;
    }
    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError) { Debug.Log("Interstitial ad failed to load: " + ironSourceError.getDescription()); }
    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo) { Debug.Log("Interstitial ad opened"); }

    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("Interstitial ad clicked");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_click");

    }

    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("Interstitial ad show succeeded");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression");
    }
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo) { Debug.Log("Interstitial ad show failed: " + ironSourceError.getDescription()); }

    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("Interstitial ad closed"); 
        IronSource.Agent.loadInterstitial();
    }

    // Banner Ads
    void LoadBannerAd()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
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
        Debug.Log("Banner ad loaded");
        isBannerLoaded = true;
    }
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError) { Debug.Log("Banner ad load failed: " + ironSourceError.getDescription()); }

    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("Banner ad clicked");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_click");

    }
    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo) { Debug.Log("Banner ad screen presented"); }
    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo) { Debug.Log("Banner ad screen dismissed"); }
    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo) { Debug.Log("Banner ad left application"); }

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
}
