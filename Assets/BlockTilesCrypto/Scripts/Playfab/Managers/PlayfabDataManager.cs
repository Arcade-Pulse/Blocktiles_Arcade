using System;
using System.Collections;
using System.Globalization;
using CodeStage.AntiCheat.ObscuredTypes;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFabPersonal.Users;
using Newtonsoft.Json;
using UnityEngine;
using CodeStage.AntiCheat.Detectors;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using Global;
using PlayFabPersonal.Economy;
using UnityEngine.Serialization;

namespace PlayFabPersonal.Managers
{
    public class PlayfabDataManager : MonoBehaviour
    {
        public static PlayfabDataManager Instance { get; private set; }

        [SerializeField] public ObscuredInt gameSocCoins;
        [SerializeField] public int lives;
        [SerializeField] public string currentPlayerID;
        [SerializeField] public bool isGuestAccountLinked;

        public int timesEnteredWithNoLives;
        public int isPlayerChallenged;

        [SerializeField] private int totalPlayerLives;
        private int lifeRewardPerAd;

        public float min;
        public float max;

        private int challengeMaxRange;
        [FormerlySerializedAs("usdConversionRate")] public string btcConversionRate;

        public string lastTransactionEmail;
        public double lastTransactionAmountInUSD;
        public DateTime lastTransactionDate;
        public string lastTransactionStatus;
        public string lastTransactionMethod;
        public bool gotTitleData;

        [SerializeField] private bool isTransactionAllowed;
        [SerializeField] private bool isTransactionInPending;
        [SerializeField] private bool isTransactionCompleted;
        [SerializeField] private bool isTransactionFailed;
        private bool isFirstTimeTransaction = false;

        [SerializeField] private ObscuredInt transactionLimit;
        [SerializeField] private DateTime timeLimit_PlayFab;
        [SerializeField] private DateTime serverTime;

        public TimeSpan hoursLeftUntilCashout;
        public static event EventHandler OnLoadPlayerData;
        public class OnFirstTransactionEventArgs : EventArgs
        {
            public bool isFirstTransaction { get; set; }
            public TimeSpan timeUntilNextCashOut { get; set; }
            public DateTime lastTransactionDateTime { get; set; }
        }
        public static event EventHandler<OnFirstTransactionEventArgs> OnFirstTimeTransaction;
        public static event EventHandler OnTransactionAllowed;
        public static event EventHandler OnTransactionNotAllowed;
        public static event Action<string> OnSuccessStartTransactionException;
        public static event Action<string> OnSuccessStartingTransaction;
        public static event Action<string> OnErrorStartingTransaction;
        public static event EventHandler<VirtualCurrency.OnAddSubstractAmountEventArgs> OnChangeVirtualCurrencyAmount;

        public class OnConverstionSuccessEventArgs : EventArgs
        {
            public ObscuredDecimal conversionRate;
        }
        public static event EventHandler<OnConverstionSuccessEventArgs> OnReceivedConversionResult;

        private TransactionError transactionError;
        private TransactionStatus transactionStatus;
        private CloudScriptRevisionOption cloudScriptRevision = CloudScriptRevisionOption.Live;
        private ScoreData scoreData;
        //private ChallengeDataRequest challengeData;

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

        private void Start()
        {
            UserAccount.OnEverythingSuccessLoadGameScene.AddListener(LoadPlayerData);
            ObscuredCheatingDetector.StartDetection(OnDetectedCheat);
        }

        private void LoadPlayerData()
        {
            GetTitleData();
            LoadPlayerInventory();
        }

        public IEnumerator GetTransactionData()
        {
            yield return new WaitUntil(()=>PlayfabDataManager.Instance.gotTitleData);
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                ObscuredString functionNane = "getTransactionData1A";
                var request = new ExecuteCloudScriptRequest
                {
                    FunctionName = functionNane,
                    RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
                    GeneratePlayStreamEvent = true
                };
                PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessGetTransactionData, OnErrorGetTransactionData);
            }
        }

        private void OnSuccessGetTransactionData(ExecuteCloudScriptResult result)
        {
            //Debug.Log("got transaction Data");
            if (result == null || result.FunctionResult == null) 
            {
                //Debug.LogError("ExecuteCloudScriptResult or FunctionResult is null.");
                return;
            }
            
            // ObscuredString functionName = "testFunction";
            // var request = new ExecuteCloudScriptRequest
            // {
            //     FunctionName = functionName,
            //     RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision()
            // };

            // if (PlayFabClientAPI.IsClientLoggedIn())
            // {
            //     PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessGetTransactionDate2, OnErrorGetTransactionDate2);
            // }
            //Debug.Log("got last transaction data");
            

            CheckLastTransaction(result);
            CheckTransactionStatus(result);
            UserAccount.Instance.CheckLinkedAccounts();

        }
        
        private void OnSuccessGetTransactionDate2(ExecuteCloudScriptResult result)
        {
            //Debug.Log("test function executed");
        }

        private void OnErrorGetTransactionDate2(PlayFabError error)
        {
        
        }
        

        private void CheckLastTransaction(ExecuteCloudScriptResult result)
        {
            if (result == null)
            {
               // Debug.LogError("ExecuteCloudScriptResult result is null.");
                return;
            }

            if (result.FunctionResult == null)
            {
                //Debug.LogError("FunctionResult in ExecuteCloudScriptResult is null.");
                return;
            }

            TransactionDataRequest transactionData;
            try
            {
                transactionData = JsonConvert.DeserializeObject<TransactionDataRequest>(result.FunctionResult.ToString());
            }
            catch (Exception ex)
            {
                //Debug.LogError($"Failed to deserialize FunctionResult to TransactionDataRequest: {ex.Message}");
                return;
            }

            if (transactionData == null)
            {
             //   Debug.LogError("TransactionDataRequest deserialization returned null.");
                return;
            }

            TransactionData lastBinanceTransaction = transactionData.binance?.LastOrDefault();
            TransactionData lastCoinbaseTransaction = transactionData.coinbase?.LastOrDefault();

            if (lastBinanceTransaction == null && lastCoinbaseTransaction == null)
            {
                //OnFirstTimeTransaction?.Invoke(this, new OnFirstTransactionEventArgs { isFirstTransaction = true });
            }
            else
            {
                var latestTransactionData = CompareLatestTransactions(lastBinanceTransaction, lastCoinbaseTransaction);

                lastTransactionEmail = latestTransactionData.email;
                lastTransactionDate = latestTransactionData.transactionDateTime;
                //Debug.Log("last transaction date time "+lastTransactionDate);
                lastTransactionAmountInUSD = latestTransactionData.amountInUSD;
                lastTransactionStatus = latestTransactionData.transactionQueryStatus;
                lastTransactionMethod = latestTransactionData.cashoutMethod;
               // OnFirstTimeTransaction?.Invoke(this, new OnFirstTransactionEventArgs { isFirstTransaction = false, lastTransactionDateTime = lastTransactionDate, timeUntilNextCashOut = hoursLeftUntilCashout});

            }
            isFirstTimeTransaction = true;
        }

        private TransactionData CompareLatestTransactions(TransactionData lastBinanceTransaction, TransactionData lastCoinbaseTransaction)
        {
            if (lastBinanceTransaction == null)
            {
                return lastCoinbaseTransaction;
            }
            else if (lastCoinbaseTransaction == null)
            {
                return lastBinanceTransaction;
            }
            else
            {
                if (lastBinanceTransaction.transactionDateTime > lastCoinbaseTransaction.transactionDateTime)
                {
                    return lastBinanceTransaction;
                }
                else
                {
                    return lastCoinbaseTransaction;
                }
            }
        }

        private void CheckTransactionStatus(ExecuteCloudScriptResult result)
{
    try
    {
        if (result != null && result.FunctionResult != null)
        {
            TransactionDataRequest transactionData = JsonConvert.DeserializeObject<TransactionDataRequest>(result.FunctionResult.ToString());

            if (transactionData == null)
            {
                //Debug.Log("TransactionDataRequest deserialization returned null.");
                return;
            }
          //  Debug.Log("transaction data "+transactionData.binance);

            List<TransactionData> binanceDatas = transactionData.binance;
            List<TransactionData> coinbaseDatas = transactionData.coinbase;

            if (binanceDatas == null)
            {
              //  Debug.Log("binanceDatas is null.");
                return;
            }

            if (coinbaseDatas == null)
            {
                //Debug.Log("coinbaseDatas is null.");
                return;
            }

            var request = new ExecuteCloudScriptRequest
            {
                RevisionSelection = cloudScriptRevision,
                GeneratePlayStreamEvent = true
            };

            ObscuredString binanceQueryFunctionName = "binancePayoutQuery1A";

            foreach (var binanceData in binanceDatas)
            {
                if (binanceData != null &&
                    (binanceData.transactionRequestStatus == "SUCCESS" || binanceData.transactionRequestStatus == "TIMEOUT") &&
                    binanceData.reserveCoinsStatus == "PENDING")
                {
                   Debug.Log(binanceData.transactionId);
                    request.FunctionName = binanceQueryFunctionName;
                    request.FunctionParameter = binanceData;

                    PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessCheckTransactionStatus, OnErrorCheckTransactionStatus);
                }
                else if (binanceData == null)
                {
                //    Debug.Log("binanceData is null in binanceDatas list.");
                }
            }
        }
        else
        {
           // Debug.Log("result or result.FunctionResult is null.");
        }
    }
    catch (NullReferenceException ex)
    {
        Debug.Log("NullReferenceException in CheckTransactionStatus: " + ex.Message);
    }
    catch (Exception ex)
    {
        //Debug.Log("Unexpected error in CheckTransactionStatus: " + ex.Message);
    }
}


        private void OnSuccessCheckTransactionStatus(ExecuteCloudScriptResult result)
        {
            if (result == null || result.FunctionResult == null)
            {
              //  Debug.Log("ExecuteCloudScriptResult or FunctionResult is null.");
                return;
            }

            TransactionDataRequest transactionData = JsonConvert.DeserializeObject<TransactionDataRequest>(result.FunctionResult.ToString());
            if (transactionData == null)
            {
             //   Debug.Log("TransactionDataRequest deserialization returned null.");
                return;
            }

           // Debug.Log(transactionData.message);
            if (transactionData.message == "BATCH PAYOUT REFUNDED")
            {
                // Update Coin UI
              // Debug.Log("Update Coin UI");
            }
        }

        private void OnErrorCheckTransactionStatus(PlayFabError error)
        {
            Debug.Log(error);
        }

        private void ShowTransactionDetailToUser(ExecuteCloudScriptResult result)
        {
            // var transactionData = result.FunctionResult;
            // Debug.Log(transactionData);
        }

        private void OnErrorGetTransactionData(PlayFabError error)
        {
            Debug.Log(error);
        }

        public void LoadPlayerInventory()
        {
            var request = new GetUserInventoryRequest();
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.GetUserInventory(request, OnSuccessGetUserInventory, OnErrorGetUserInventory);
            }
        }
        
        public void LoadPlayerInventory2()
        {
            var request = new GetUserInventoryRequest();
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.GetUserInventory(request, OnSuccessGetUserInventory2, OnErrorGetUserInventory);
            }
        }

        private void OnSuccessGetUserInventory2(GetUserInventoryResult result)
        {
            if (result?.VirtualCurrency != null)
            {
                if (result.VirtualCurrency.TryGetValue("CN", out var cn))
                {
                    gameSocCoins = cn;

                }

                if (result.VirtualCurrency.TryGetValue("LF", out var lf))
                {
                    lives = lf;
                }
                
            }
            else
            {
                Debug.LogError("Failed to get virtual currency data.");
            }
        }

        private void OnSuccessGetUserInventory(GetUserInventoryResult result)
        {
            if (result?.VirtualCurrency != null)
            {
                if (result.VirtualCurrency.TryGetValue("CN", out var cn))
                {
                    gameSocCoins = cn;
                }

                if (result.VirtualCurrency.TryGetValue("LF", out var lf))
                {
                    lives = lf;
                }

                // if (lives < 0)
                // {
                //     VirtualCurrency.Instance.AddLife(Math.Abs(lives));
                // }

                LoadMainMenuScene();
            }
            else
            {
                Debug.LogError("Failed to get virtual currency data.");
            }
        }
        
        private void LoadMainMenuScene()
        {
            //  HideLoadingPanel();
           // Debug.Log("reached here !");
           if (SceneManager.GetActiveScene().name!="MainMenuScene")
           {
               SceneManager.LoadSceneAsync("MainMenuScene");
              
           }

           else
           {
               PlayerUIManager.Instance.ShowPlayerStats();
           }
           //StartCoroutine(PlayfabDataManager.Instance.GetTransactionData());
           // PlayfabDataManager.Instance.ChallengeData();
           //HideLoadingPanel();
           // Debug.Log("reached here 2 !");
        }

        private void OnErrorGetUserInventory(PlayFabError error)
        {
          //  Debug.Log(error.ErrorMessage);
        }

        private void OnDetectedCheat()
        {
            ObscuredString functionName = "cheatFound";
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = functionName,
                RevisionSelection = cloudScriptRevision,
                GeneratePlayStreamEvent = true
            };

            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessObscuredCheatingDetector, OnErrorObscuredCheatingDetector);
            }
        }

        private void OnSuccessObscuredCheatingDetector(ExecuteCloudScriptResult result)
        {
            PlayFabClientAPI.ForgetAllCredentials();
            SceneManager.LoadScene("AuthenticationScene");
        }

        private void OnErrorObscuredCheatingDetector(PlayFabError error)
        {
            Debug.Log(error.ErrorMessage);
        }

        #region Get

        public string GetCurrentPlayerID()
        {
            return currentPlayerID;
        }

        // public void StartTransaction(string selectedCashoutMethod, decimal amount, string email)
        // {
        //     
        //     ObscuredString functionName = "startTransaction1B";
        //     var request = new ExecuteCloudScriptRequest()
        //     {
        //         FunctionName = functionName,
        //         FunctionParameter = new
        //         {
        //             email = email,
        //             method = selectedCashoutMethod,
        //             amountInUSD = amount.ToString(CultureInfo.InvariantCulture)
        //         },
        //         GeneratePlayStreamEvent = true,
        //         RevisionSelection = cloudScriptRevision
        //     };
        //
        //     lastTransactionEmail = email;
        //     lastTransactionAmountInUSD = (double)amount;
        //
        //     if (PlayFabClientAPI.IsClientLoggedIn())
        //         PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessStartTransaction, OnErrorStartTransaction);
        // }

        public void ShowBannerAd()
        {
            // BannerViewController.Instance.collapsible = true;
            // BannerViewController.Instance.LoadAd();
            //
        }

        private void OnSuccessStartTransaction(ExecuteCloudScriptResult result)
        {
          //  InterstitialAdController.Instance.ShowAd();
         //   ShowBannerAd();
            //Debug.Log("Transaction Started");
            if (result.FunctionResult == null)
            {
                MessageManager.OnErrorShowMessage?.Invoke("Transaction failed execution");
                SetTransactionData();
                return;
            }

            transactionError = JsonUtility.FromJson<TransactionError>(result.FunctionResult.ToString());
           // transactionError = JsonConvert.DeserializeObject<TransactionError>(result.FunctionResult.ToString());
            if (transactionError.error)
            {
                MessageManager.OnErrorShowMessage?.Invoke(transactionError.message);
              //  Debug.Log("Error is: " + transactionError.message);
            }
            else
            {
                MessageManager.OnSuccessShowMessage?.Invoke("Your request has been successfully submitted.");
                LoadPlayerInventory();
                SetTransactionData();
               // Debug.Log("Data is " + transactionError.message);
            }
            //BannerViewController.Instance.collapsible = false;
        }

        private void SetTransactionData()
        {
            ObscuredString functionName = "newTransactionSave1A";
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = functionName,
                FunctionParameter = new
                {
                    email = lastTransactionEmail,
                    playerFabId = currentPlayerID,
                    transactionDate = serverTime.ToString("dd/MM/yyyy HH:mm"),
                    numberOfCoins = lastTransactionAmountInUSD.ToString(CultureInfo.InvariantCulture)
                },
                RevisionSelection = cloudScriptRevision,
                GeneratePlayStreamEvent = true
            };

            if (PlayFabClientAPI.IsClientLoggedIn())
                PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessSetTransactionData, onErrorSetTransactionData);
        }

        private void OnSuccessSetTransactionData(ExecuteCloudScriptResult result) { }
        private void onErrorSetTransactionData(PlayFabError error) { }

        private void OnErrorStartTransaction(PlayFabError error)
        {
          //  Debug.Log("Transaction Started Failed");
            OnErrorStartingTransaction?.Invoke(error.ErrorMessage);
        }

        private void GetTitleData()
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
                PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnSuccessGetPlayTimeLimitations, OnErrorGetPlayTimeLimitations);
        }

        private void OnSuccessGetPlayTimeLimitations(GetTitleDataResult result)
        {
            if (result.Data != null)
            {
                gotTitleData = true;
                cloudScriptRevision = (CloudScriptRevisionOption)int.Parse(result.Data["cloudScriptRevision"]);
                min = float.Parse(result.Data["minWithdrawableAmount"], CultureInfo.InvariantCulture);
                max = float.Parse(result.Data["maxWithdrawableAmount"], CultureInfo.InvariantCulture);
                transactionLimit = int.Parse(result.Data["transactionLimitPerDay"]);
                totalPlayerLives = int.Parse(result.Data["totalPlayerLives"]);
                lifeRewardPerAd = int.Parse(result.Data["lifeRewardPerAd"]);
                scoreData = JsonConvert.DeserializeObject<ScoreData>(result.Data["gameScoreData"]);
                challengeMaxRange = int.Parse(result.Data["challengeMaxRandRange"]);
                
              //  Debug.Log("cloudscript revision is "+cloudScriptRevision);

                var debugListJson = result.Data["debugList"];
                List<string> debugList = JsonConvert.DeserializeObject<List<string>>(debugListJson);

                CheckIfDeveloperAccount(debugList);
                GetCurrentServerTime();
            }
            else
            {
                GetTitleData();
            }
        }

        private void CheckIfDeveloperAccount(List<string> debugList)
        {
            bool isDeveloper = false;
            if (debugList != null && debugList.Count > 0 && debugList.Contains(GetCurrentPlayerID()))
            {
                isDeveloper = true;
            }
            PlayerPrefs.SetInt(PlayerPrefNameString.DEVELOPER, isDeveloper ? 1 : 0);
        }

        public void GetCurrentServerTime()
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.GetTime(new GetTimeRequest(), OnSuccessGetCurrentServerTime, OnErrorGetCurrentServerTime);
            }
        }

        private void OnSuccessGetCurrentServerTime(GetTimeResult result)
        {
            if (result != null) { serverTime = result.Time; }
        }

        private void OnErrorGetCurrentServerTime(PlayFabError error) { }

        private void GetDate()
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnSuccessGetDate, OnErrorGetDate);
            }
        }

        private void OnSuccessGetDate(GetUserDataResult result)
        {
            if (result.Data != null && result.Data.ContainsKey("Date"))
            {
                try
                {
                    var parsed = DateTime.TryParseExact(
                        result.Data["Date"].Value,
                        "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime dateTime
                    );

                    if (parsed)
                        lastTransactionDate = dateTime;

                }
                catch (Exception e)
                {
                 //   Debug.Log(e);
                    throw;
                }

                if (serverTime > lastTransactionDate.AddDays(1.0f))
                {
                    SetDate();
                }
            }
            else if (result.Data != null)
            {
                SetDate();
            }
            else
            {
                GetTitleData();
            }
        }

        private void SetDate()
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { "Date", serverTime.ToString("dd/MM/yyyy HH:mm") }
                }
            };

            lastTransactionDate = serverTime;
            if (PlayFabClientAPI.IsClientLoggedIn())
                PlayFabClientAPI.UpdateUserData(request, OnSuccessSetDate, OnErrorSetDate);
        }

        private void OnSuccessSetDate(UpdateUserDataResult result) { }
        private void OnErrorSetDate(PlayFabError error) { }
        private void OnErrorGetDate(PlayFabError error) { }
        private void OnErrorGetPlayTimeLimitations(PlayFabError error) { }

        public void GetConversionRate()
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "GetConversionRateBasedOnECPM",
                RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
                GeneratePlayStreamEvent = true
            }, OnSuccessGetConversionRate, OnErrorGetConversionRate);
        }

        private void OnSuccessGetConversionRate(ExecuteCloudScriptResult result)
        {
            if (result.FunctionResult != null && result.FunctionResult is IDictionary<string, object> functionResult)
            {
                btcConversionRate = functionResult["usdConversionRate"].ToString();
              //  Debug.Log("conversion rate is "+btcConversionRate);
                if (decimal.TryParse(btcConversionRate, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal conversionRate))
                {
                    OnReceivedConversionResult?.Invoke(this, new OnConverstionSuccessEventArgs { conversionRate = conversionRate });
                }
                else
                {
                    // If parsing fails, use a default conversion rate
                    var defaultConversionRate = 1e-9m;
                    OnReceivedConversionResult?.Invoke(this, new OnConverstionSuccessEventArgs { conversionRate = defaultConversionRate });
                }
            }
        }

        private void OnErrorGetConversionRate(PlayFabError error) { }

        public void GetTransactionDate()
        {
            ObscuredString functionName = "getTransactionDate1A";
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = functionName,
                FunctionParameter = new
                {
                    playerID = currentPlayerID
                },
                RevisionSelection = cloudScriptRevision,
                GeneratePlayStreamEvent = true
            };

            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessGetTransactionDate, OnErrorGetTransactionDate);
            }
        }

        private void OnSuccessGetTransactionDate(ExecuteCloudScriptResult result)
        {
          //  Debug.Log("got transaction date");
            JsonObject jsonResult = (JsonObject)result.FunctionResult;
            jsonResult.TryGetValue("messageValue", out object messageValue);
  //          Debug.Log("function result is"+result.FunctionResult);

            if (result.FunctionResult != null && messageValue != null && messageValue.ToString() != "0")
            {
                var transactionDate = messageValue.ToString();
                var parsed = DateTime.TryParseExact(
                    transactionDate,
                    "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date
                );

                timeLimit_PlayFab = date;
       //         Debug.Log("time limit playfab is "+timeLimit_PlayFab);

                var timeUntilCashout = timeLimit_PlayFab.AddHours(transactionLimit);
               // Debug.Log("server time is "+serverTime);
              //  Debug.Log("date until cashout is "+timeUntilCashout); 
                hoursLeftUntilCashout = timeUntilCashout - serverTime;
              //  Debug.Log("hours left "+hoursLeftUntilCashout);
               // Debug.Log("time limits is "+transactionLimit);

                if (serverTime >= timeUntilCashout)
                {
                  //  Debug.Log("transaction based on time is allowed");
                    isTransactionAllowed = true;
                    var hoursDifference = timeLimit_PlayFab.AddHours(transactionLimit).Subtract(serverTime);
                    OnTransactionAllowed?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                 //   Debug.Log("transaction based on time is NOT allowed");
                //   GetTransactionStatus(currentPlayerID);
                isTransactionAllowed = false;
                OnTransactionNotAllowed?.Invoke(this, EventArgs.Empty);

                }
            }
            else if (messageValue != null && messageValue.ToString() == "0")
            {
                isTransactionAllowed = true;
                OnTransactionAllowed?.Invoke(this, EventArgs.Empty);
            }
            else if (result.FunctionResult == null)
            {
                OnTransactionNotAllowed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void GetTransactionStatus(string currentPlayerID)
        {
            ObscuredString functionName = "getTransactionStatus";
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = functionName,
                FunctionParameter = new
                {
                    playerID = currentPlayerID
                },
                RevisionSelection = cloudScriptRevision,
                GeneratePlayStreamEvent = true
            };

            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.ExecuteCloudScript(request, OnSuccessGetTransactionStatus, OnErrorGetTransactionStatus);
            }
        }

        private void OnSuccessGetTransactionStatus(ExecuteCloudScriptResult result)
        {
            if (this == null && result.FunctionResult == null) return;

            transactionStatus = JsonConvert.DeserializeObject<TransactionStatus>((string)result.FunctionResult);
            if (transactionStatus == null) return;
            isTransactionInPending = false;
            isTransactionFailed = false;
            isTransactionCompleted = false;

            if (transactionStatus.completed)
            {
                isTransactionCompleted = true;
            }
            else if (transactionStatus.error)
            {
                isTransactionFailed = true;
            }

            if (!transactionStatus.error && transactionStatus.pending)
            {
                isTransactionInPending = true;
                isTransactionAllowed = false;

                OnTransactionAllowed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                isTransactionAllowed = true;
                OnTransactionAllowed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnErrorGetTransactionStatus(PlayFabError error)
        {
            OnTransactionNotAllowed?.Invoke(this, EventArgs.Empty);
        }

        private void OnErrorGetTransactionDate(PlayFabError error) { }

        public DateTime GetServerTime()
        {
            return serverTime;
        }

        public ObscuredInt GetGameSocCoins()
        {
            return gameSocCoins;
        }

        public int GetLives()
        {
            if (lives <= 0)
            {
                return 0;
            }
            return lives;
        }

        public bool GetTransactionCompleted()
        {
            return isTransactionCompleted;
        }

        public bool GetTransactionAllowed()
        {
            return isTransactionAllowed;
        }

        public bool GetFirstTransaction()
        {
            return isFirstTimeTransaction;
        }

        public ObscuredInt GetTransactionLimitPerDay()
        {
            return transactionLimit;
        }

        public CloudScriptRevisionOption GetCloudRevision()
        {
            return cloudScriptRevision;
        }

        public int GetTotalPlayerLives()
        {
            return totalPlayerLives;
        }

        public int GetLifeRewardPerAd()
        {
            Debug.Log("life reward per ad"+lifeRewardPerAd);
            return lifeRewardPerAd;
        }

        public ScoreData GetScoreData()
        {
            return scoreData;
        }

        public int GetChallengeMaxRange()
        {
            return challengeMaxRange;
        }

        // public ChallengeDataRequest GetChallengeData()
        // {
        // //    return challengeData;
        // }

        #endregion

        #region Set

        public void SetPlayerID(string currentPlayerID)
        {
            this.currentPlayerID = currentPlayerID;
        }

        public void SetGameSocCoins(int gameSocCoins)
        {
            this.gameSocCoins = gameSocCoins;
        }

        public void SetLives(int lives)
        {
            this.lives = lives;
        }

        #endregion
    }
}

public class ScoreData
{
    [JsonPropertyAttribute("0")]
    public int _0 { get; set; }

    [JsonPropertyAttribute("1")]
    public int _1 { get; set; }

    [JsonPropertyAttribute("2")]
    public int _2 { get; set; }

    [JsonPropertyAttribute("3")]
    public int _3 { get; set; }

    [JsonPropertyAttribute("4")]
    public int _4 { get; set; }

    [JsonPropertyAttribute("5")]
    public int _5 { get; set; }

    [JsonPropertyAttribute("6")]
    public int _6 { get; set; }

    [JsonPropertyAttribute("7")]
    public int _7 { get; set; }
    public int _ { get; set; }
}
