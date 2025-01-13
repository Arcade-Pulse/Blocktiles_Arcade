using System;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using Global;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;
using PlayFabPersonal.Users;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    [SerializeField] private Button homeButton;
    [SerializeField] private Button tryAgainButton;

    [SerializeField] private Button cashoutButton;
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private bool isPlayed = false;

    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // private void OnEnable()
    // {
    //     // if (InterstitialAdController.Instance.isAdLoaded)
    //     // {
    //     //     InterstitialAdController.Instance.ShowAd();
    //     // }
    //     // else
    //     // {
    //     //     InterstitialAdController.Instance.LoadAd();
    //     // }
    //   
    // }

    private void OnEnable()
    {
  
    }


    private void Start()
    {
        PlayfabDataManager.OnLoadPlayerData += LoadMainMenuScene;

        if (tryAgainButton != null)
        {
            tryAgainButton.onClick.AddListener(TryAgainButton);
        }
        if (homeButton != null)
        {
            homeButton.onClick.AddListener(LoadMainMenu);
        }
        if (cashoutButton != null)
        {
            cashoutButton.onClick.AddListener(InGameCashOutButton);
        }
    }

    public void InGameCashOutButton()
    {
        SetButtonInteractable(false);
        ObscuredPrefs.Set(PlayerPrefNameString.CASHOUTSELECTED,true);
        SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Single);

        //  CashoutEventSystem.InvokeCashoutButtonClicked();
    }

    private void LoadMainMenuScene(object sender, EventArgs e)
    {
      //  HideLoadingPanel();
     //   Debug.Log("reached here !");
        SceneManager.LoadSceneAsync("MainMenuScene");
        //StartCoroutine(PlayfabDataManager.Instance.GetTransactionData());
       // PlayfabDataManager.Instance.ChallengeData();
        //HideLoadingPanel();
     //   Debug.Log("reached here 2 !");
    }
    



    public void ShowLoadingPanel()
    {
        loadingScreen.loadingPanel.SetActive(true);
    }

    public void HideLoadingPanel()
    {
        loadingScreen.loadingPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SetButtonInteractable(false);
        SceneManager.LoadScene("MainMenuScene");
    }

    private void OnDisable()
    {
        PlayfabDataManager.OnLoadPlayerData -= LoadMainMenuScene;
    }

    internal void SetButtonInteractable(bool value)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
           // playButton.interactable = value;
            homeButton.interactable = value;
            cashoutButton.interactable = value;
            tryAgainButton.interactable = value;
        }
    }
    
    public void TryAgainButton()
    {
        if (isPlayed) return;
        isPlayed = true;
        Debug.Log("try again pressed");

        SetButtonInteractable(false);

        if (PlayfabDataManager.Instance.GetLives() <= 0)
        {
            isPlayed = false;
            Debug.Log("you have zero lives");
            // Show Message
           // MessageManager.OnErrorShowMessage?.Invoke("You have 0 life now");
           // if (RewardedAdController.Instance.isAdLoaded)
           // {
           //     RewardedAdController.Instance.inGameRequest = true;
           //     RewardedAdController.Instance.ShowAd();
           // }
           // else
           // {
           //     if (RewardedInterstitialAdController.Instance.isAdLoaded)
           //     {
           //         RewardedInterstitialAdController.Instance.ShowAd();
           //     }
           //     RewardedAdController.Instance.LoadAd();
           // }
           if ( cashoutButton != null)
           {
                 
               cashoutButton.interactable = true;
           }
            SceneManager.LoadSceneAsync("GameScene");
            return;
        }
        VirtualCurrency.Instance.SubtractLife(1);
        GameUIManager.Instance.ShowPlayerStats();
        SceneManager.LoadSceneAsync("GameScene");
    }


    // public void SetIsPlayed(bool isPlayed)
    // {
    //     this.isPlayed = isPlayed;
    // }
}
