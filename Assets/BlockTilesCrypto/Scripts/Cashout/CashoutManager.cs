using System;
using System.Collections.Generic;
using System.Globalization;
using CodeStage.AntiCheat.ObscuredTypes;
using Newtonsoft.Json;
using PlayFabPersonal.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Cashout
{
    public class CashoutManager : MonoBehaviour
    {
        public CashoutMainContainer mainContainer;
        public CashoutConfirmationContainer confirmationContainer;
        public CashoutTransactionContainer transactionContainer;
        public CashoutTransactionDetailsContainer transactionDetailsContainer;
        [SerializeField] private GameObject cashoutPanel;
        public GameObject mainMenuPanel;

        private ObscuredDecimal totalDollars;
        private ObscuredDecimal coinInBTC;
        private ObscuredDecimal currencyConversionRate;
        private string selectedCashoutMethod;


        private void Start()
        {
            PlayfabDataManager.OnTransactionAllowed += ProceedWithTransaction;

            PlayfabDataManager.OnReceivedConversionResult += GetDollarRate;
            PlayfabDataManager.OnTransactionNotAllowed += DoNotProceedWithTransaction;
           // Debug.Log("proceds with transaction");

            mainContainer.gameSocCoinText.text = PlayfabDataManager.Instance.GetGameSocCoins().ToString();
            mainContainer.proceedButton.onClick.AddListener(Proceed);
            mainContainer.cancelButton.onClick.AddListener(closeCashout);
            confirmationContainer.cancelButton.onClick.AddListener(() =>
            {
                confirmationContainer.confirmationPanel.SetActive(false);
                mainContainer.mainContainerPanel.SetActive(true);
            });
            confirmationContainer.confirmButton.onClick.AddListener(ConfirmTransaction);
            mainContainer.lastTransactionStatusButton.onClick.AddListener(LastTransactionDetails);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void DoNotProceedWithTransaction(object sender, EventArgs e)
        {
           // Debug.Log("do not proceed with transaction");
            var cooldownTimer = FindObjectOfType<TransactionCooldownTimer>();
            MainMenuManager.Instance.HideLoadingPanel();
            cooldownTimer.StartTransactionCooldown(PlayfabDataManager.Instance.hoursLeftUntilCashout,PlayfabDataManager.Instance.lastTransactionDate);
            PlayfabDataManager.Instance.GetConversionRate();
        }

        public void closeCashout()
        {
            cashoutPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }

        private void LastTransactionDetails()
        {
            //Debug.Log("last transaction details");
            transactionDetailsContainer.transactionDetailPanel.SetActive(true);
            transactionDetailsContainer.coinInUSDCurrency.text =
                PlayfabDataManager.Instance.lastTransactionAmountInUSD.ToString() + " USD";
            //Debug.Log("last transaction amount in usd "+PlayfabDataManager.Instance.lastTransactionAmountInUSD);
            transactionDetailsContainer.transactionDateText.text =
                PlayfabDataManager.Instance.lastTransactionDate.ToString();
            transactionDetailsContainer.transactionStatusText.text =
                SetColorBasedOnTransactionStatus(PlayfabDataManager.Instance.lastTransactionStatus);
            transactionDetailsContainer.transferingToEmailText.text = PlayfabDataManager.Instance.lastTransactionEmail;

            if (PlayfabDataManager.Instance.lastTransactionMethod == "binance")
            {
                var binanceTexture = transactionDetailsContainer.paymentMethodTextures[1];
                transactionDetailsContainer.paymentMethodImage.GetComponent<Image>().sprite = Sprite.Create(
                    binanceTexture, new Rect(0.0f, 0.0f, binanceTexture.width, binanceTexture.height), new Vector2(),
                    100.0f);
            }
            else
            {
                var coinbaseTexture = transactionDetailsContainer.paymentMethodTextures[0];
                transactionDetailsContainer.paymentMethodImage.GetComponent<Image>().sprite = Sprite.Create(
                    coinbaseTexture, new Rect(0.0f, 0.0f, coinbaseTexture.width, coinbaseTexture.height), new Vector2(),
                    100.0f);
            }
        }

        private string SetColorBasedOnTransactionStatus(string lastTransactionStatus)
        {
            Dictionary<string, string> statusColorDictionary = new()
            {
                { "SUCCESS", "#64FF32" },
                { "FAILED", "#FF6464" },
                { "PENDING", "#FAB43C" },
                { "AWAITING", "#28B4E6" },
                { "REFUNDED", "#32C832" }
            };

            return $"<color={statusColorDictionary[lastTransactionStatus]}>{lastTransactionStatus}</color>";
        }

        public void ConfirmTransaction()
        {
            confirmationContainer.confirmButton.gameObject.SetActive(false);
            cashoutPanel.SetActive(false);
            mainMenuPanel.SetActive(true);

            PlayfabDataManager.Instance.lastTransactionEmail = mainContainer.cashoutEmailField.text;
            var coinsInUSDC = coinInBTC / currencyConversionRate;
            // PlayfabDataManager.Instance.StartTransaction(selectedCashoutMethod, coinsInUSDC,
            //     mainContainer.cashoutEmailField.text);
        }

        private void Proceed()
        {
            decimal currentGameSocCoins = PlayfabDataManager.Instance.GetGameSocCoins();
            decimal maxUSD = (decimal)PlayfabDataManager.Instance.max;
            decimal minUSD = (decimal)PlayfabDataManager.Instance.min;
            decimal maxUSDinBTC = maxUSD * currencyConversionRate;
            decimal minUSDinBTC = minUSD * currencyConversionRate;
           // decimal conversionRateDecimal = (decimal)currencyConversionRate;

            //Debug.Log("Proceed: Current GameSoc Coins: " + currentGameSocCoins);
            //Debug.Log("Proceed: Conversion Rate: " + currencyConversionRate);

            // Calculate totalDollers based on current GameSoc coins and conversion rate
            //Debug.Log("conver gamesoc coins "+PlayfabDataManager.Instance.btcConversionRate);
            var gameSocToUsd = Decimal.Parse(PlayfabDataManager.Instance.btcConversionRate,CultureInfo.InvariantCulture);
            //Debug.Log("gamesoc to usd "+gameSocToUsd);
            totalDollars =  currentGameSocCoins*gameSocToUsd;
            //Debug.Log("Proceed: Total Dollars: " + totalDollars);
            //Debug.Log("Proceed: Min USD: " + minUSD);
            //Debug.Log("Proceed: Max USD: " + maxUSD);

            // Calculate coinInUSDC
//            Debug.Log("Proceed: Coin in BTC: " + coinInBTC);

            if (coinInBTC < minUSDinBTC)
            {
                // Show Message
              //  Debug.Log("Proceed: Coin in BTC is less than the minimum BTC required.");
                MessageManager.OnErrorShowMessage?.Invoke("Your account balance is too low to proceed.");
            }
            else if (coinInBTC > maxUSDinBTC)
            {
               // Debug.Log("Proceed: Coin in BTC is greater than the maximum BTC allowed.");

                // Calculate the maximum GameSoc coins to cash out based on maxUSD
                decimal maxGameSocCoinsToCashOut = maxUSDinBTC / currencyConversionRate;
               // Debug.Log("Proceed: Max GameSoc coins to cash out: " + maxGameSocCoinsToCashOut);

                // Round to nearest integer
                int maxGameSocCoinsToCashOutInt = (int)Math.Round(maxGameSocCoinsToCashOut);
               // Debug.Log("Proceed: Max GameSoc coins to cash out (rounded): " + maxGameSocCoinsToCashOutInt);

                // Calculate the remaining GameSoc coins after cashing out the maximum allowed
                int remainingGameSocCoins = (int)currentGameSocCoins - maxGameSocCoinsToCashOutInt;
                //Debug.Log("Proceed: Remaining GameSoc coins after max cash out: " + remainingGameSocCoins);

                // Ensure no negative value is assigned
                remainingGameSocCoins = Math.Max(remainingGameSocCoins, 0);

                // Update the UI accordingly
                confirmationContainer.coinInCryptoCurrency.text = maxUSDinBTC.ToString("N8") + " BTC";
               // Debug.Log("MAX AMOUNT IS "+PlayfabDataManager.Instance.max);
                confirmationContainer.usageMessageText.text =
                    $"Are you sure you want to proceed with the below transaction? Your amount exceeded our Max Transaction Limit so we have updated it to our max limit {PlayfabDataManager.Instance.max} Dollars";
                coinInBTC = maxUSDinBTC;

                if (GeneralFunctions.ValidateEmail(mainContainer.cashoutEmailField.text) &&
                    !GeneralFunctions.IsEmpty(mainContainer.cashoutEmailField.text))
                {
                    confirmationContainer.transferingToEmailText.text = mainContainer.cashoutEmailField.text;
                    confirmationContainer.confirmationPanel.SetActive(true);
                    mainContainer.mainContainerPanel.SetActive(false);

                    // Update the player's GameSoc coins
                   // Debug.Log("Proceed: Valid email provided, updating GameSoc coins.");
                    PlayfabDataManager.Instance.SetGameSocCoins(remainingGameSocCoins);
                }
                else
                {
                   // Debug.Log("Proceed: Invalid email format.");
                    MessageManager.OnErrorShowMessage?.Invoke("Please check your email format");
                    return;
                }
            }
            else
            {
                //Debug.Log("Proceed: Coin in USDC is within the allowed range.");

                // Round to nearest integer
                int coinsToCashOutInt = (int)Math.Round(currentGameSocCoins);
               // Debug.Log("Proceed: Coins to cash out (rounded): " + coinsToCashOutInt);

                // Calculate the remaining GameSoc coins after cashing out
                int remainingGameSocCoins = (int)currentGameSocCoins - coinsToCashOutInt;
                //Debug.Log("Proceed: Remaining GameSoc coins after cash out: " + remainingGameSocCoins);

                // Ensure no negative value is assigned
                remainingGameSocCoins = Math.Max(remainingGameSocCoins, 0);

                if (GeneralFunctions.ValidateEmail(mainContainer.cashoutEmailField.text) &&
                    !GeneralFunctions.IsEmpty(mainContainer.cashoutEmailField.text))
                {
                    confirmationContainer.transferingToEmailText.text = mainContainer.cashoutEmailField.text;
                    confirmationContainer.confirmationPanel.SetActive(true);
                    mainContainer.mainContainerPanel.SetActive(false);

                    // Update the player's GameSoc coins
                   // Debug.Log("Proceed: Valid email provided, updating GameSoc coins.");
                    PlayfabDataManager.Instance.SetGameSocCoins(remainingGameSocCoins);
                }
                else
                {
                   // Debug.Log("Proceed: Invalid email format.");
                    MessageManager.OnErrorShowMessage?.Invoke("Please check your email format");
                    return;
                }
            }
        }


        private void ConvertGameSocCoinToDollars()
        {
            PlayfabDataManager.Instance.GetConversionRate();
        }

        public double CalculateRequiredAmount(decimal conversionRate)
        {
            // Calculate the raw required amount using Instance.min
            double rawAmount = PlayfabDataManager.Instance.min / (double)conversionRate;

            // Determine the magnitude of the highest significant digit
           /// int magnitude = (int)Math.Pow(10, Math.Floor(Math.Log10((double)rawAmount)));

            // Round to the nearest significant digit
           // int requiredAmount = (int)(Math.Round(rawAmount / magnitude) * magnitude);

            return rawAmount;
        }

        private void GetDollarRate(object sender, PlayfabDataManager.OnConverstionSuccessEventArgs e)
        {
            //Debug.Log("GetDollerRate: Conversion rate received: " + e.conversionRate);

            float currentGameSocCoin = PlayfabDataManager.Instance.GetGameSocCoins();
            var requiredAmount = CalculateRequiredAmount(e.conversionRate);

//    Debug.Log("GetDollerRate: Current GameSoc Coins: " + currentGameSocCoin);
            //  Debug.Log("GetDollerRate: Required Amount: " + requiredAmount);
            // Debug.Log("GetDollerRate: Transaction Allowed: " + PlayfabDataManager.Instance.GetTransactionAllowed());

            if (PlayfabDataManager.Instance.GetTransactionAllowed() &&
                int.Parse(currentGameSocCoin.ToString(CultureInfo.InvariantCulture)) < requiredAmount)
            {
                mainContainer.paymentAllowedContainer.SetActive(false);
                mainContainer.sufficientAmountContainer.SetActive(true);

                if (PlayfabDataManager.Instance.GetTransactionCompleted())
                {
                    mainContainer.lastTransactionEmail.text = PlayfabDataManager.Instance.lastTransactionEmail;
                    mainContainer.lastTransactionAmount.text =
                        PlayfabDataManager.Instance.lastTransactionAmountInUSD.ToString("N18");
                    mainContainer.lastTransactionDate.text =
                        PlayfabDataManager.Instance.lastTransactionDate.ToString("dd/MM/yyyy");
                    mainContainer.lastTransactionContainer.SetActive(true);
                }
                else
                {
                    mainContainer.lastTransactionContainer.SetActive(false);
                }

                SetCurrencyProgressBar(requiredAmount);
            }
            else if (PlayfabDataManager.Instance.GetTransactionAllowed())
            {
                mainContainer.nextCashoutContainer.SetActive(false);
                mainContainer.sufficientAmountContainer.SetActive(false);
                mainContainer.paymentAllowedContainer.SetActive(true);
            }

            totalDollars = e.conversionRate * PlayfabDataManager.Instance.GetGameSocCoins();
//    Debug.Log("GetDollerRate: Total Dollars after conversion: " + totalDollars);

            string url = "https://api.coinbase.com/v2/exchange-rates?currency=USD";
            //  Debug.Log("GetDollerRate: Fetching exchange rate from URL: " + url);
            WebRequests.Get(url, OnSuccess, OnError);
        }

        private void OnSuccess(string result)
        {
//            Debug.Log("OnSuccess: Received exchange rate result: " + result);

            try
            {
                ExchangeRateResponse exchangeRateResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(result);
//                Debug.Log("OnSuccess: Parsed exchange rate response successfully.");

                if (exchangeRateResponse?.data?.rates?.BTC != null)
                {
                    currencyConversionRate =
                        decimal.Parse(exchangeRateResponse.data.rates.BTC, CultureInfo.InvariantCulture);
                    //Debug.Log("OnSuccess: Currency conversion rate from API: " + currencyConversionRate);

                    //Debug.Log("total dollars are "+totalDollars);
                    coinInBTC = totalDollars * currencyConversionRate;
                   //Debug.Log("OnSuccess: Coin in BTC after conversion: " + coinInBTC);

                    mainContainer.coinInCryptoCurrency.text = coinInBTC.ToString("N8") + " BTC";
                    confirmationContainer.coinInCryptoCurrency.text = coinInBTC.ToString("N8") + " BTC";
                }
                else
                {
                   // Debug.LogError("OnSuccess: USD rate not found in exchange rate response.");
                    // Handle the error appropriately
                }
            }
            catch (Exception ex)
            {
                //Debug.LogError("OnSuccess: Exception occurred while parsing exchange rate response: " + ex.Message);
                // Handle the error appropriately
            }

            MainMenuManager.Instance.HideLoadingPanel();
        }


        private void OnError(string error)
        {
            MainMenuManager.Instance.HideLoadingPanel();
            cashoutPanel.SetActive(false);
            //  Debug.Log(error);
        }

        public static int RoundUpToNearestInteger(double value)
        {
            // Round up to the nearest integer
            return (int)Math.Ceiling(value);
        }
        
        private void SetCurrencyProgressBar(ObscuredDouble requiredAmount)
        {
            float minValue = PlayfabDataManager.Instance.min;

            //var convertRate = float.Parse(conversionRate.ToString());
            //minValue /= convertRate;
            // var requiredAmount = float.Parse(conversionRate.ToString());
            var requiredAmountInteger = RoundUpToNearestInteger(requiredAmount);

            mainContainer.requiredAmountText.text =
                $"You need to have at least <br><color=#EAC444>{requiredAmountInteger} GameSoc Coins</color> to cash out!";
            //Debug.Log("gamesoc coins:" + PlayfabDataManager.Instance.GetGameSocCoins());
           // Debug.Log("required amount to cashout:" + requiredAmount);
            var percentage = PlayfabDataManager.Instance.GetGameSocCoins() / requiredAmount;
            //Debug.Log("percentage:" + percentage);
            var Percefloat = float.Parse(percentage.ToString());
            mainContainer.currencyProgressBar.fillAmount = Percefloat;
            //  mainContainer.currencyProgressBar.fillAmount = requiredAmount;
        }

        // private void DoNotProceedWithTransaction(object sender, EventArgs e)
        // {
        //     // NoAddErrorText.text = $"There is only one transaction Allowed in {PlayFabDataManager.Instance.transactionLimit} Days";
        //     // adErrorButtonText.text = "Close";
        //     // NoAddError.SetActive(true);
        //     Debug.Log("Transaction not allowed");
        // }

        private void ProceedWithTransaction(object sender, EventArgs e)
        {
          //   Debug.Log("Proceed With trans");
            // loadingPanel.SetActive(true);
            confirmationContainer.confirmButton.gameObject.SetActive(true);
            cashoutPanel.SetActive(true);
            mainContainer.lastTransactionContainer.SetActive(false);
            ConvertGameSocCoinToDollars();
        }

        public void SetCashoutMethod(string selectedCashoutMethod)
        {
            this.selectedCashoutMethod = selectedCashoutMethod;
        }

        private void OnDisable()
        {
            PlayfabDataManager.OnTransactionAllowed -= ProceedWithTransaction;

            PlayfabDataManager.OnTransactionNotAllowed -= DoNotProceedWithTransaction;
            PlayfabDataManager.OnReceivedConversionResult -= GetDollarRate;
        }
    }
}