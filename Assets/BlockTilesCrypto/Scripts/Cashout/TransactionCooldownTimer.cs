using System;
using System.Collections;
using System.Globalization;
using Cashout;
using PlayFabPersonal.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransactionCooldownTimer : MonoBehaviour
{
  //  private TimeSpan remainingTime;
    private bool isCooldownActive = false;
    private CashoutManager cashoutManager;

    private void OnEnable()
    {
     //   Debug.Log("object enabled");
       // SceneManager.sceneLoaded += OnSceneLoaded;
       GetLastTransaction();
    }



    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void Start()
    {
       Debug.Log("TransactionCooldownTimer started");

        //PlayfabDataManager.OnFirstTimeTransaction += LoadData_OnFirstTimeTransaction;
    }

    private void OnDisable()
    {
       // PlayfabDataManager.OnFirstTimeTransaction -= LoadData_OnFirstTimeTransaction;
    }

    // private void LoadData_OnFirstTimeTransaction(object sender, PlayfabDataManager.OnFirstTransactionEventArgs e)
    // {
    //     Debug.Log("LoadData_OnFirstTimeTransaction");
    //     if (!e.isFirstTransaction)
    //     {
    //         StartTransactionCooldown(e.timeUntilNextCashOut,e.lastTransactionDateTime);
    //     }
    // }

    public void StartTransactionCooldown(TimeSpan remainingTime, DateTime lastCashoutDate)
    {
       
        isCooldownActive = true;

      //  StopCoroutine(UpdateRemainingTimeToDisplay());
      FindAndSetCashoutManager();
     UpdateRemainingTimeToDisplay(remainingTime);
    }

    public void UpdateRemainingTimeToDisplay(TimeSpan remainingTime)
    {
        //Debug.Log("updating remaining time");

            cashoutManager.mainContainer.nextCashoutContainer.SetActive(true);
            cashoutManager.mainContainer.lastTransactionContainer.SetActive(true);
            cashoutManager.mainContainer.paymentAllowedContainer.SetActive(false);
            cashoutManager.mainContainer.sufficientAmountContainer.SetActive(false);
           // Debug.Log("amount is "+PlayfabDataManager.Instance.lastTransactionAmountInUSD);
 //           //Debug.Log("email is "+PlayfabDataManager.Instance.lastTransactionEmail);
            //Debug.Log("date is "+PlayfabDataManager.Instance.lastTransactionDate.ToString());


            cashoutManager.mainContainer.lastTransactionAmount.text = PlayfabDataManager.Instance.lastTransactionAmountInUSD.ToString() + " USD";
            cashoutManager.mainContainer.lastTransactionEmail.text = PlayfabDataManager.Instance.lastTransactionEmail;
            cashoutManager.mainContainer.lastTransactionDate.text = PlayfabDataManager.Instance.lastTransactionDate.ToString();
            cashoutManager.mainContainer.messageText.text = $"was sent to below {PlayfabDataManager.Instance.lastTransactionMethod} account";
            cashoutManager.mainContainer.message1Text.text = $"You will receive an email from {PlayfabDataManager.Instance.lastTransactionMethod} <br>with a link to complete the process.";

            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            var cashoutPascalCase = PlayfabDataManager.Instance.lastTransactionMethod;
            cashoutPascalCase = info.ToTitleCase(cashoutPascalCase);
            cashoutManager.mainContainer.emailLabelText.text = $"{cashoutPascalCase} account:";
            cashoutManager.mainContainer.nextCashoutTimerText.text = $"<color=#DDB125>Next Cash Out In:</color> <b>{remainingTime.Hours:00}:{remainingTime.Minutes:00}:{remainingTime.Seconds:00}</b>";
            
    }

    // public void SetCashoutManager(CashoutManager manager)
    // {
    //     cashoutManager = manager;
    // }

    // Method to find and set the CashoutManager reference
    private void FindAndSetCashoutManager()
    {
        cashoutManager = FindObjectOfType<CashoutManager>();
        if (cashoutManager == null)
        {
            Debug.Log("CashoutManager not found in the scene.");
        }
    }

    // Event handler for scene loaded event
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("new scene loaded");
        FindAndSetCashoutManager();
        StartCoroutine(PlayfabDataManager.Instance.GetTransactionData());
        // Reset the CashoutManager reference when a new scene is loaded
        // if (scene.name == "MainMenuScene")
        // {
        //    
        // }
    }

    public void GetLastTransaction()
    {
        FindAndSetCashoutManager();
        StartCoroutine(PlayfabDataManager.Instance.GetTransactionData());
    }
    
}
