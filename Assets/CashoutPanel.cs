using System;
using System.Collections;
using System.Collections.Generic;
using Cashout;
using PlayFabPersonal.Managers;
using UnityEngine;


public class CashoutPanel : MonoBehaviour
{
    public GameObject captcha;
    public GameObject confirmationContainer;
    public GameObject mainContainer;
    
    public static CashoutPanel Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        //Debug.Log("cashout panel enabled");
       // captcha.SetActive(true);
       mainContainer.SetActive(true);
       confirmationContainer.SetActive(false);
        var cashoutManager = FindObjectOfType<CashoutManager>();
        cashoutManager.mainContainer.gameSocCoinText.text = PlayfabDataManager.Instance.GetGameSocCoins().ToString();
        ShowRewInterAd();
        PlayfabDataManager.Instance.GetCurrentServerTime();
    }

    public void ShowRewInterAd()
    {
        // if (RewardedInterstitialAdController.Instance.isAdLoaded)
        // {
        //     InterstitialAds.Instance.ShowAd();
        // }
        // else
        // {
        //     InterstitialAds.Instance.LoadAd();
        // }
    }
}
