using System;
using System.Collections;
using System.Collections.Generic;
using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] public TMP_Text gameSocCoinText;
    private PlayfabDataManager playfabDataManager;
    public static GameUIManager Instance;

    private void OnEnable()
    {
        playfabDataManager = FindObjectOfType<PlayfabDataManager>();
    //    Debug.Log(playfabDataManager.gameObject.name);
        ShowPlayerStats();
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

    // private void OnEnable()
    // {
    //     VirtualCurrency.OnCoinBalanceUpdated += ShowPlayerStats;  // Update the UI when the coin balance changes
    // }
    //
    // private void OnDisable()
    // {
    //     VirtualCurrency.OnCoinBalanceUpdated -= ShowPlayerStats;  // Clean up the listener
    // }


    public void ShowPlayerStats()
    {
        //Debug.Log("executing Show player stats");
        try
        {
            if (playfabDataManager == null)
            {
                //Debug.Log("playfabDataManager is null in ShowPlayerStats");
                return;
            }

            if (lifeText != null)
            {
                lifeText.text = playfabDataManager.GetLives().ToString() + "/" + playfabDataManager.GetTotalPlayerLives();
               // Debug.Log("updated lives"+playfabDataManager.GetLives()+playfabDataManager.GetTotalPlayerLives());

            }
            else
            {
                //Debug.LogError("lifeText is null in ShowPlayerStats");
            }

            if (gameSocCoinText != null)
            {
                gameSocCoinText.text = playfabDataManager.GetGameSocCoins().ToString();
               // Debug.Log("updated coins"+playfabDataManager.GetGameSocCoins().ToString());

            }
            else
            {
              //  Debug.Log("gameSocCoinText is null in ShowPlayerStats");
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
          //  Debug.LogError("Error in showPlayerStats: " + ex.Message);
        }
        catch (Exception ex)
        {
         //   Debug.LogError("Unexpected error in showPlayerStats: " + ex.Message);
        }
    }
}
