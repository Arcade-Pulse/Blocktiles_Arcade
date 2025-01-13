using System;
using System.Threading.Tasks;
using UnityEngine;

public class InitialAdLoader : MonoBehaviour
{
    private const int loadAdDelay = 60000;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    

    private void Start()
    {
        GoogleMobileAdsController.OnAdReadyToServe += OnAdReadyToServe_LoadInterstitialAd;

        //OnSuccess Load Next Format
        InterstitialAdController.OnLoaded += OnSuccess_LoadRewarded;
        RewardedAdController.OnLoaded += OnSuccess_LoadRewardedInterstitial;
        RewardedInterstitialAdController.OnLoaded += OnSuccess_LoadAppOpen;
        // AppOpenAdController.OnLoaded += OnSuccess_LoadBanner;
        
        // OnFail Load Same
        InterstitialAdController.OnLoadFailed += OnFailed_LoadInterstitial;
        RewardedAdController.OnLoadFailed += OnFailed_LoadRewarded;
        RewardedInterstitialAdController.OnLoadFailed += OnFailed_LoadRewardedInterstitial;
        BannerViewController.OnLoadFailed += OnFailed_LoadBanner;
        AppOpenAdController.OnLoadFailed += OnFailed_LoadAppOpen;
    }

    #region Fail Ad load Event

    private async void OnFailed_LoadInterstitial(object sender, EventArgs e)
    {
        Debug.Log("OnFailed_LoadInterstitial");
        await Task.Delay(loadAdDelay);
        InterstitialAdController.Instance.LoadAd();
    }

    private async void OnFailed_LoadRewarded(object sender, EventArgs e)
    {
        Debug.Log("OnFailed_LoadRewarded");
        await Task.Delay(loadAdDelay);
       // RewardedAdController.Instance.LoadAd();
    }

    private async void OnFailed_LoadRewardedInterstitial(object sender, EventArgs e)
    {
        Debug.Log("OnFailed_LoadRewardedInterstitial");
        await Task.Delay(loadAdDelay);
      //  RewardedInterstitialAdController.Instance.LoadAd();
    }

    private async void OnFailed_LoadBanner(object sender, EventArgs e)
    {
        Debug.Log("OnFailed_LoadBanner");
        await Task.Delay(loadAdDelay);
        BannerViewController.Instance.LoadAd();
    }

    private async void OnFailed_LoadAppOpen(object sender, EventArgs e)
    {
        Debug.Log("OnFailed_LoadAppOpen");
        await Task.Delay(loadAdDelay);
        //AppOpenAdController.Instance.LoadAd();
    }

    #endregion

    #region Success ad load event
    private void OnAdReadyToServe_LoadInterstitialAd(object sender, EventArgs e)
    {
        InterstitialAdController.Instance.LoadAd();
        GoogleMobileAdsController.OnAdReadyToServe -= OnAdReadyToServe_LoadInterstitialAd;
    }

    private void OnSuccess_LoadRewarded(object sender, EventArgs e)
    {
        RewardedAdController.Instance.LoadAd();
        InterstitialAdController.OnLoaded -= OnSuccess_LoadRewarded;
    }

    private void OnSuccess_LoadRewardedInterstitial(object sender, EventArgs e)
    {
        //RewardedInterstitialAdController.Instance.LoadAd();
        //RewardedAdController.OnLoaded -= OnSuccess_LoadRewardedInterstitial;
    }

    private void OnSuccess_LoadAppOpen(object sender, EventArgs e)
    {
        //AppOpenAdController.Instance.LoadAd();
       // RewardedInterstitialAdController.OnLoaded -= OnSuccess_LoadAppOpen;
    }

    private void OnSuccess_LoadBanner(object sender, EventArgs e)
    {
        Debug.Log("Load Banner On Success AppOpenAd Load");
        BannerViewController.Instance.LoadAd();
        //AppOpenAdController.OnLoaded -= OnSuccess_LoadBanner;
    }

    #endregion

    private void OnDisable()
    {
        GoogleMobileAdsController.OnAdReadyToServe -= OnAdReadyToServe_LoadInterstitialAd;

        InterstitialAdController.OnLoaded -= OnSuccess_LoadRewarded;
        RewardedAdController.OnLoaded -= OnSuccess_LoadRewardedInterstitial;
        RewardedInterstitialAdController.OnLoaded -= OnSuccess_LoadAppOpen;
        // AppOpenAdController.OnLoaded -= OnSuccess_LoadBanner;
        
        InterstitialAdController.OnLoadFailed -= OnFailed_LoadInterstitial;
        RewardedAdController.OnLoadFailed -= OnFailed_LoadRewarded;
        RewardedInterstitialAdController.OnLoadFailed -= OnFailed_LoadRewardedInterstitial;
        BannerViewController.OnLoadFailed -= OnFailed_LoadBanner;
        AppOpenAdController.OnLoadFailed -= OnFailed_LoadAppOpen;
    }
}
