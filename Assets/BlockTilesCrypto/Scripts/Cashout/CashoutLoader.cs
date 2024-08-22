using System.Threading.Tasks;
using PlayFabPersonal.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CashoutLoader : MonoBehaviour
{
    private static CashoutLoader _instance;

    public static CashoutLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singleton = new("CashoutLoaderSingleton");
                _instance = singleton.AddComponent<CashoutLoader>();
                DontDestroyOnLoad(singleton);
            }
            return _instance;
        }
    }

    VerificationManager verificationManager;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        verificationManager = FindAnyObjectByType<VerificationManager>();
    }

    private void OnEnable()
    {
      //  CashoutEventSystem.OnCashoutButtonClicked += LoadCashoutPanel;
    }

    private void OnDisable()
    {
      //  CashoutEventSystem.OnCashoutButtonClicked -= LoadCashoutPanel;
    }

    public async void LoadCashoutPanel()
    {
        //Debug.Log("entered here 2");
    
        while (!IsDesiredSceneLoaded())
        {
            await Task.Yield();
        }
        
        // if (InterstitialAdController.Instance.isAdLoaded)
        // {
        //     InterstitialAdController.Instance.ShowAd();
        // }
        // else
        // {
        //     InterstitialAdController.Instance.LoadAd();
        // }

        // Perform further actions
       // Debug.Log("Desired scene loaded. Proceeding with cashout panel loading.");
        // BannerViewController.Instance.DestroyAd();
       // RewardedInterstitialAdController.Instance.ShowAd();
        MainMenuManager.Instance.ShowLoadingPanel();
        
        PlayfabDataManager.Instance.GetTransactionDate();
        //
        // if (PlayfabDataManager.Instance.GetChallengeData() == null)
        // {
        //     MessageManager.OnErrorShowMessage?.Invoke("Please haven't verified yet");
        //     if (PlayfabDataManager.Instance.GetGameSocCoins() > 500)
        //         verificationManager.InitiateVerificationProcess();
        //     GameSceneManager.Instance.HideLoadingPanel();
        // }
        // else
        // {
        //     if (PlayfabDataManager.Instance.GetChallengeData().chances >= 0 && PlayfabDataManager.Instance.GetChallengeData().isCompleted)
        //     {
        //         PlayfabDataManager.Instance.GetTransactionDate();
        //     }
        //     else
        //     {
        //         MessageManager.OnErrorShowMessage?.Invoke("You are barred from cashout due to failing verification.");
        //         GameSceneManager.Instance.HideLoadingPanel();
        //     }
        // }
    }

    private bool IsDesiredSceneLoaded()
    {
        // Check if the desired scene is loaded
        Scene desiredScene = SceneManager.GetSceneByName("MainMenuScene");
        return desiredScene.isLoaded;
    }
}
