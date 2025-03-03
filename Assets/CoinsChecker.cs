using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsChecker : MonoBehaviour
{
    public Button claimButton;
    public int subtractedAmount = 20000;

    public TextMeshProUGUI coinsValue;

    public TextMeshProUGUI textMessage;

    public TextMeshProUGUI buttonText;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (PlayfabDataManager.Instance.GetGameSocCoins()>=subtractedAmount)
        {
            claimButton.interactable = true;
            textMessage.text = "Exchange 20K coins for 3 lives?";
            buttonText.text = "YES";
        }
        else
        {
            claimButton.interactable = false;
            textMessage.text = "Please collect 20000 coins to get reward";
            buttonText.text = "CLAIM REWARD";

        }
    }

    public void ClaimButtonPressed()
    {
        claimButton.interactable = false;
        Debug.Log("lives are "+PlayfabDataManager.Instance.GetTotalPlayerLives());
        Debug.Log("lives 2 are "+PlayfabDataManager.Instance.lives);
        if (PlayfabDataManager.Instance.lives<=7)
        {
            VirtualCurrency.Instance.AddLife(3);
            SubtractCurrency("CN",subtractedAmount);

        }
        else if(PlayfabDataManager.Instance.lives<10)
        {
            VirtualCurrency.Instance.AddLife(1);
            SubtractCurrency("CN",subtractedAmount);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }
    
    public void SubtractCurrency(string currencyCode, int amount)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "subtractCurrency",
            FunctionParameter = new Dictionary<string, object>
            {
                { "currencyCode", currencyCode },
                { "amount", amount }
            },
            GeneratePlayStreamEvent = true
        };
        
        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptSuccess, OnError);
    }

    private void OnCloudScriptSuccess(ExecuteCloudScriptResult result)
    {
        if (result.FunctionResult != null)
        {
            var functionResult = result.FunctionResult as Dictionary<string, object>;
            if (functionResult != null && functionResult.ContainsKey("balance"))
            {
               // Debug.Log($"New balance: {functionResult["balance"]}");
             //   OnChangeVirtualCurrencyAmount?.Invoke(this, new VirtualCurrency.OnAddSubstractAmountEventArgs { currencyValue = currentGameSocCoin });
            }
            Debug.Log("success remove of coins");
            LoadPlayerInventory2(); // Refresh the local balance from the server

        }
        else
        {
            Debug.LogWarning("No result returned from cloud script.");
        }
        gameObject.SetActive(false);
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError($"Error calling Cloud Script: {error.GenerateErrorReport()}");
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
                PlayfabDataManager.Instance.gameSocCoins = cn;
                coinsValue.text = cn.ToString();

            }

            if (result.VirtualCurrency.TryGetValue("LF", out var lf))
            {
                PlayfabDataManager.Instance.lives = lf;
            }
                
        }
        else
        {
            Debug.LogError("Failed to get virtual currency data.");
        }
    }
    
    private void OnErrorGetUserInventory(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }


}
