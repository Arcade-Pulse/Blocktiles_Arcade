using System.Collections;
using System.Collections.Generic;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

public class AppoDeal : MonoBehaviour, IAppodealInitializationListener, IBannerAdListener
{
    public int retryCount;
    void OnEnable() {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnDisable() {
    }

    void OnSdkInitialized(bool precache) {
        Debug.Log("Appodeal SDK Initialized");
    }

    private void Start()
    {
        Debug.Log("Starting Appodeal");
        int adTypes = Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO;
        string appKey = "a13a870393ce2ce161df6d08bf61c13d8de9b2d0525947ce";
        Appodeal.initialize(appKey, adTypes, this);
    }

    public void onInitializationFinished(List<string> errors)
    {
        Debug.Log("Ads init finished");
        if (errors != null && errors.Count > 0)
        {
            foreach (var error in errors)
            {
                Debug.LogError("Appodeal initialization error: " + error);
            }
        }
        else
        {
            Appodeal.setSmartBanners(true);
            Appodeal.setBannerCallbacks(this); // Register banner callbacks
            Appodeal.show(Appodeal.BANNER_BOTTOM);
        }
    }

    // IBannerAdListener implementation
    public void onBannerLoaded(int height, bool isPrecache) {
        Debug.Log("Banner ad loaded successfully");
        Appodeal.show(Appodeal.BANNER_BOTTOM);

    }

    public void onBannerFailedToLoad() {
        Debug.LogError("Banner ad failed to load");
        if (retryCount<5)
        {
            StartCoroutine(RetryLoadAd(4));
        }

    }
    private IEnumerator RetryLoadAd(float delayInSecond)
    {
        yield return new WaitForSeconds(delayInSecond);
        retryCount++;
        Appodeal.cache(Appodeal.BANNER_BOTTOM);

    }

    public void onBannerShown() {
        Debug.Log("Banner ad is shown");
    }

    public void onBannerShowFailed()
    {
       // throw new System.NotImplementedException();
    }

    public void onBannerClicked() {
        Debug.Log("Banner ad clicked");
    }

    public void onBannerExpired() {
        Debug.Log("Banner ad expired");
    }
}