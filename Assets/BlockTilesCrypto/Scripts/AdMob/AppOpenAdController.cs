using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System.Collections;

[AddComponentMenu("GoogleMobileAds/AppOpenAdController")]
public class AppOpenAdController : MonoBehaviour
{
    public static AppOpenAdController Instance { get; private set; }
  //  public static bool DevelopmentMode { get; set; } = false;

    private string adAppOpenID=AdsID.adAppOpenID;
    private int retryCount = 0;

    // App open ads can be preloaded for up to 4 hours.
    private readonly TimeSpan TIMEOUT = TimeSpan.FromHours(4);
    private DateTime _expireTime;

    private AppOpenAd _appOpenAd;
    private bool isAdLoaded = false;

    public static event EventHandler OnLoaded;
    public static event EventHandler OnLoadFailed;

    private void OnEnable()
    {
    }

    private void Awake()
    {
        // // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    public void LoadAd()
    {
        if(isAdLoaded) return;

        // Clean up the old ad before loading a new one.
        if (_appOpenAd != null)
        {
            DestroyAd();
        }

     //   Debug.Log("Loading app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        AppOpenAd.Load(adAppOpenID, adRequest, (AppOpenAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.Log("App open ad failed to load an ad with error : "
                                + error);
                // // Calculate the delay based on the retry count (e.g., 5 seconds for the 1st retry, 10 seconds for the 2nd, and so on).
                // float delayInSeconds = 5f + (retryCount * 5f);

                // // Retry loading the ad after the calculated delay.
               // StartCoroutine(RetryLoadAd(5));
                OnLoadFailed?.Invoke(this, EventArgs.Empty);
                return;
            }

            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.Log("Unexpected error: App open ad load event fired with " +
                               " null ad and null error.");
                return;
            }

            // The operation completed successfully.
         //   Debug.Log("App open ad loaded with response : " + ad.GetResponseInfo());
            //Debug.Log("Current loaded App open ad serving adapter " + ad.GetResponseInfo().GetMediationAdapterClassName());
            _appOpenAd = ad;
            isAdLoaded = true;
            OnLoaded?.Invoke(this, EventArgs.Empty);

            // App open ads can be preloaded for up to 4 hours.
            _expireTime = DateTime.Now + TIMEOUT;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);
        });
    }

    public void ShowAd()
    {
        // App open ads can be preloaded for up to 4 hours.
        if (_appOpenAd != null && _appOpenAd.CanShowAd() && DateTime.Now < _expireTime)
        {
           // Debug.Log("Showing app open ad.");
            _appOpenAd.Show();
        }
        else
        {
          //  Debug.Log("App open ad is not ready yet.");
        }
    }

    private IEnumerator RetryLoadAd(float delayInSecond)
    {
        yield return new WaitForSeconds(delayInSecond);
        retryCount++;
        LoadAd();
    }

    public void DestroyAd()
    {
        if (_appOpenAd != null)
        {
           // Debug.Log("Destroying app open ad.");
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }
    }

    private void OnAppStateChanged(AppState state)
    {
       Debug.Log("App State changed to : " + state);

      //  If the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (isAdLoaded)
            {
                ShowAd();
            }
            else
            {
                LoadAd();
            }
          
        }
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            // Debug.Log(String.Format("App open ad paid {0} {1}.",
            //     adValue.Value,
            //     adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            //Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
           // Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            //Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
           // Debug.Log("App open ad full screen content closed.");

            // It may be useful to load a new ad when the current one is complete.
            isAdLoaded = false;
          //  LoadAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Debug.Log("App open ad failed to open full screen content with error : "
            //                 + error);
            isAdLoaded = false;
            LoadAd();
        };
    }
}