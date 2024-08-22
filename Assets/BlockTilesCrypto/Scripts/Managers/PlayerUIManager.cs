using System;
using CodeStage.AntiCheat.Storage;
using Global;
using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text uidText;
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] public TMP_Text gameSocCoinText;
    [SerializeField] public GameObject menuPanel;
    [SerializeField] private GameObject guestNotAllowedPanel;

    private PlayfabDataManager playfabDataManager;
    private VerificationManager verificationManager;
    public static PlayerUIManager Instance;

    public Button cashoutButton;

    private void OnEnable()
    {
       // checkIfRewardedIntIsLoaded();
    }

    private void checkIfRewardedIntIsLoaded()
    {
        if (RewardedInterstitialAdController.Instance != null && RewardedInterstitialAdController.Instance.isAdLoaded)
        {
            cashoutButton.interactable = true;
        }
        else
        {
            cashoutButton.interactable = false;
        }
    }

    private void Awake()
    {
        playfabDataManager = FindObjectOfType<PlayfabDataManager>();
     //   verificationManager = FindObjectOfType<VerificationManager>();

        if (playfabDataManager == null)
        {
            Debug.LogWarning("PlayfabDataManager is not found in the scene.");
        }
        // if (verificationManager == null)
        // {
        //     Debug.LogError("VerificationManager is not found in the scene.");
        // }

        if (Instance == null)
        {
            Instance = this;
        //    DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ShowPlayerStats();

        VirtualCurrency.OnChangeLife += UpdateLives_OnChangeLife;
        VirtualCurrency.OnChangeVirtualCurrencyAmount += UpdateGameSocCoins_OnOnChangeVirtualCurrencyAmount;
    }
    
    public void ShowPlayerStats()
    {
        try
        {
            if (playfabDataManager == null)
            {
                Debug.LogError("playfabDataManager is null in ShowPlayerStats");
                return;
            }

            if (uidText != null)
            {
                string playerID = playfabDataManager.GetCurrentPlayerID();
                if (playerID.Length >= 5)
                {
                    uidText.text = $"UID: {playerID.Substring(0, 5)}";
                }
                else
                {
                    Debug.Log("Player ID is too short to display the first 5 characters.");
                    uidText.text = $"UID: {playerID}"; // Fallback to displaying the entire ID
                }
            }
            else
            {
                Debug.LogError("uidText is null in ShowPlayerStats");
            }

            if (lifeText != null)
            {
                lifeText.text = playfabDataManager.GetLives().ToString() + "/" + playfabDataManager.GetTotalPlayerLives();
            }
            else
            {
                Debug.LogError("lifeText is null in ShowPlayerStats");
            }

            if (gameSocCoinText != null)
            {
                gameSocCoinText.text = playfabDataManager.GetGameSocCoins().ToString();
            }
            else
            {
                Debug.LogError("gameSocCoinText is null in ShowPlayerStats");
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debug.LogError("Error in showPlayerStats: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Unexpected error in showPlayerStats: " + ex.Message);
        }
    }

    public void AddLife()
    {
        var GO = GameObject.FindObjectOfType<RewardLives>();
        if (GO != null)
        {
            GO.RewardLife();
        }
        else
        {
            Debug.LogError("RewardLives object is not found in the scene.");
        }
    }

    private void UpdateGameSocCoins_OnOnChangeVirtualCurrencyAmount(object sender, VirtualCurrency.OnAddSubstractAmountEventArgs e)
    {
        if (playfabDataManager != null)
        {
            playfabDataManager.SetGameSocCoins(playfabDataManager.GetGameSocCoins() + e.currencyValue);
            if (gameSocCoinText != null)
            {
                gameSocCoinText.text = playfabDataManager.GetGameSocCoins().ToString();
            }
        }
    }

    private void UpdateLives_OnChangeLife(object sender, VirtualCurrency.OnAddSubstractLifeEventArgs e)
    {
        if (playfabDataManager != null)
        {
            playfabDataManager.SetLives(playfabDataManager.GetLives() + e.currencyLife);
            if (lifeText != null)
            {
                lifeText.text = playfabDataManager.GetLives().ToString() + "/" + playfabDataManager.GetTotalPlayerLives();
            }
        }
    }

    public void CashOutButtonPressed()
    {
        ShowLoadingPanel();
        CheckIfTransactionAllowed();
    }

    public void ShowLoadingPanel()
    {
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.ShowLoadingPanel();
        }
        else
        {
            Debug.LogError("MainMenuManager.Instance is null.");
        }
    }

    // public void ShowAdWhenCashoutIsPressed()
    // {
    //     if (InterstitialAdController.Instance != null)
    //     {
    //         if (InterstitialAdController.Instance.isAdLoaded)
    //         {
    //             InterstitialAdController.Instance.ShowAd();
    //         }
    //         else
    //         {
    //             InterstitialAdController.Instance.LoadAd();
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("InterstitialAdController.Instance is null.");
    //     }
    // }

    public void CheckIfTransactionAllowed()
    {
        try
        {
            if (PlayfabDataManager.Instance != null)
            {
                if (PlayfabDataManager.Instance.isGuestAccountLinked)
                {
                    PlayfabDataManager.Instance.GetTransactionDate();
                }
                else if (ObscuredPrefs.HasKey(PlayerPrefNameString.LAST_LOGIN))
                {
                    if (ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) == PlayerPrefNameString.GUEST)
                    {
                        if (MainMenuManager.Instance != null)
                        {
                            MainMenuManager.Instance.HideLoadingPanel();
                        }
                        else
                        {
                            Debug.LogError("MainMenuManager.Instance is null.");
                        }

                        if (guestNotAllowedPanel != null)
                        {
                            guestNotAllowedPanel.SetActive(true);
                        }
                        else
                        {
                            Debug.LogError("guestNotAllowedPanel is null.");
                        }
                    }
                    else
                    {
                        PlayfabDataManager.Instance.GetTransactionDate();
                    }
                }
                else
                {
                    PlayfabDataManager.Instance.GetTransactionDate();
                }
            }
            else
            {
                Debug.LogError("PlayfabDataManager.Instance is null.");
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.LogError("NullReferenceException in CheckIfTransactionAllowed: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Unexpected error in CheckIfTransactionAllowed: " + ex.Message);
        }
    }

    private void OnDisable()
    {
        VirtualCurrency.OnChangeLife -= UpdateLives_OnChangeLife;
        VirtualCurrency.OnChangeVirtualCurrencyAmount -= UpdateGameSocCoins_OnOnChangeVirtualCurrencyAmount;
    }
}
