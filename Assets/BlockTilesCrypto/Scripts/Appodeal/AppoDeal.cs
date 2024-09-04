using System.Collections;
using System.Collections.Generic;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;
using AppodealAds.Unity.Common;


public class AppoDeal : MonoBehaviour,IAppodealInitializationListener
{
    void OnEnable() {
        // AppodealCallbacks.Sdk.OnInitialized += OnSdkInitialized;
        // AppodealCallbacks.Interstitial.OnLoaded += OnInterstitialLoaded;
        // AppodealCallbacks.RewardedVideo.OnFinished += OnRewardedVideoFinished;
        DontDestroyOnLoad(this.gameObject);
    }

    void OnDisable() {
        // AppodealCallbacks.Sdk.OnInitialized -= OnSdkInitialized;
        // AppodealCallbacks.Interstitial.OnLoaded -= OnInterstitialLoaded;
        // AppodealCallbacks.RewardedVideo.OnFinished -= OnRewardedVideoFinished;
        
    }

    void OnSdkInitialized(bool precache) {
        Debug.Log("Appodeal SDK Initialized");
    }

    void OnInterstitialLoaded(bool isPrecache) {
        Debug.Log("Interstitial Ad Loaded");
    }

    void OnRewardedVideoFinished(double amount, string name) {
        Debug.Log("Rewarded Video Watched: " + name + ", Reward: " + amount);
    }
    private void Start()
    {
        Debug.Log("starting appodeal");
        int adTypes = Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO | Appodeal.MREC;
        string appKey = "70dc1dac1501e8d5111311c91fc2e3abd9d8339066365187";
        Appodeal.initialize(appKey, adTypes, this);

    }

    public void onInitializationFinished(List<string> errors)
    {
        Debug.Log("ads init finished");
        Appodeal.setSmartBanners(true);
        Appodeal.show(Appodeal.BANNER_BOTTOM);
    }
    

}
