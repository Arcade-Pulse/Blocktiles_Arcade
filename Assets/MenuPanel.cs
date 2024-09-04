using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using Global;
//using GoogleMobileAds.Api;
using PlayFabPersonal.Managers;
using UnityEditor.Minesweeper.Scripts.UnityAds;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Button cashoutButton;
    public static MenuPanel Instance { get; private set; }

    private void OnEnable()
    {
     //   Debug.Log("menu panel enabled");
        ChangeStateCashout();
        UpdatePlayerStatus();
        if (!Rewarded.Instance.isAdLoaded)
        {
            Rewarded.Instance.LoadRewardedAd();
        }

        if (!Interstitial.Instance.isAdLoaded)
        {
            Interstitial.Instance.LoadInterstitialAd();
        }
      //  RewardedInterstitialAdController.Instance.adRewarded = false;
        // if (InterstitialAdController.Instance.isAdLoaded)
        // {
        //     InterstitialAdController.Instance.ShowAd();
        // }
        // else
        // {
        //     InterstitialAdController.Instance.LoadAd();
        // }
    }
    
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

    public void ChangeStateCashout()
    {
      //  Debug.Log("change cashout state");

      cashoutButton.interactable = true;
      // if (RewardedInterstitialAdController.Instance.isAdLoaded)
      // {
      //     cashoutButton.interactable = true;
      //    // Debug.Log("change cashout to true");
      //
      // }
      // else
      // {
      //     cashoutButton.interactable = false;
      //    // RewardedInterstitialAdController.Instance.LoadAd();
      // }
    }

    public void UpdatePlayerStatus()
    {
        try
        {
            if (PlayfabDataManager.Instance != null)
            {
                PlayfabDataManager.Instance.LoadPlayerInventory();
            }
            else
            {
                //Debug.Log("PlayfabDataManager.Instance is null.");
            }

            if (PlayerUIManager.Instance != null)
            {
                PlayerUIManager.Instance.ShowPlayerStats();
            }
            else
            {
                // Debug.Log("PlayerUIManager.Instance is null.");
            }

            if (ObscuredPrefs.HasKey(PlayerPrefNameString.CASHOUTSELECTED))
            {
                var bool1 = ObscuredPrefs.Get(PlayerPrefNameString.CASHOUTSELECTED, true);
                if (bool1)
                {
                    ObscuredPrefs.Set(PlayerPrefNameString.CASHOUTSELECTED, false);

                    if (PlayerUIManager.Instance != null)
                    {
                        PlayerUIManager.Instance.CheckIfTransactionAllowed();
                        PlayerUIManager.Instance.ShowPlayerStats();
                    }
                    else
                    {
                        Debug.Log("PlayerUIManager.Instance is null.");
                    }

                    if (PlayfabDataManager.Instance != null)
                    {
                        PlayfabDataManager.Instance.LoadPlayerInventory();
                    }
                    else
                    {
                        //Debug.Log("PlayfabDataManager.Instance is null.");
                    }
                }
                // ObscuredPrefs.DeleteKey(PlayerPrefNameString.CASHOUTSELECTED);
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("NullReferenceException in MenuPanel.OnEnable: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("Unexpected error in MenuPanel.OnEnable: " + ex.Message);
        }
    }
}

