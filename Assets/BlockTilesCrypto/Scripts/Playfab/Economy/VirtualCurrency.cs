using System;
using CodeStage.AntiCheat.ObscuredTypes;
using Firebase.Analytics;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Managers;
using UnityEngine;

namespace PlayFabPersonal.Economy
{
    public class VirtualCurrency : MonoBehaviour
    {
        public int addedLife;
        public static VirtualCurrency Instance { get; private set; }
        public static event EventHandler<OnAddSubstractAmountEventArgs> OnChangeVirtualCurrencyAmount;
        public class OnAddSubstractAmountEventArgs : EventArgs
        {
            public int currencyValue;
        }

        public static event EventHandler<OnAddSubstractLifeEventArgs> OnChangeLife;
        public class OnAddSubstractLifeEventArgs : EventArgs
        {
            public int currencyLife;
        }

        private ObscuredInt currentGameSocCoin;

        // private ObscuredInt currentLives;

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

            DontDestroyOnLoad(this.gameObject);
        }

        public void AddGameSocCurrency(int currentGameSocCoin)
        {
            this.currentGameSocCoin = currentGameSocCoin;
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "addGameSocCoins1A",
                FunctionParameter = new
                {
                    currencyType = "CN",
                    amount = currentGameSocCoin
                },
                RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
                GeneratePlayStreamEvent = true
            };

            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessAddCurrency, OnErrorAddCurrency);
            }
        }

        public void AddLife(int life)
        {
            addedLife = life;
            // this.currentLives = life;
            Debug.Log("add life"+addedLife);
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "addLife1A",
                FunctionParameter = new
                {
                    currencyType = "LF",
                    amount = life
                },
                RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessAddLife, OnErrorAddLife);
        }

        private void OnSuccessAddLife(ExecuteCloudScriptResult result)
        {
           // PlayfabDataManager.Instance.SetIsLifeRewarded(false);
            Debug.Log("Life added Successfully");
            MessageManager.OnSuccessShowMessage?.Invoke
            (
                // $"You have successfully added \"{PlayfabDataManager.Instance.GetLifeRewardPerAd()}\" life. You now have " + (PlayfabDataManager.Instance.GetLives() + PlayfabDataManager.Instance.GetLifeRewardPerAd()) + "\\" + PlayfabDataManager.Instance.GetTotalPlayerLives() + "."
                $"You have successfully added \"{addedLife}\" lives." 
            );
            OnChangeLife?.Invoke(this, new OnAddSubstractLifeEventArgs { currencyLife = addedLife });
        }

        private void OnErrorAddLife(PlayFabError error)
        {
            Debug.Log(error.ErrorMessage);
        }

        public void SubtractLife(int life)
        {
            // this.currentLives = life;
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "subtractLife1A",
                FunctionParameter = new
                {
                    currencyType = "LF",
                    amount = life
                },
                RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessSubtractLife, OnErrorSubtractLife);
        }

        private void OnSuccessSubtractLife(ExecuteCloudScriptResult result)
        {
            Debug.Log("Life substracted successfully");
            OnChangeLife?.Invoke(this, new OnAddSubstractLifeEventArgs { currencyLife = -1 });
        }

        private void OnErrorSubtractLife(PlayFabError error)
        {
          //  Debug.Log(error.ErrorMessage);
        }

        private void OnSuccessAddCurrency(ExecuteCloudScriptResult result)
        {
            // GetCoin()
            OnChangeVirtualCurrencyAmount?.Invoke(this, new OnAddSubstractAmountEventArgs { currencyValue = currentGameSocCoin });
            PlayfabDataManager.Instance.LoadPlayerInventory2(); // Refresh the local balance from the server

            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, new Parameter[]
            {
                new Parameter(FirebaseAnalytics.ParameterValue, 50), // Amount earned
                new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, "coins") // Currency name
            });
        }

        private void OnErrorAddCurrency(PlayFabError error)
        {
            Debug.Log(error.ErrorMessage);
        }

    }
}