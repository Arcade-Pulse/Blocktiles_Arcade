using System;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountRecovery : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailAddressField; 
    [SerializeField] private Button recoveryPanelButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject recoveryPanel;
    [SerializeField] private GameObject loginPanel;

    private void Start()
    {
        resetButton.onClick.AddListener(SendRecoveryEmail);
        recoveryPanelButton.onClick.AddListener(() =>
        {
            recoveryPanel.gameObject.SetActive(true);
            loginPanel.gameObject.SetActive(false);
        });
        backButton.onClick.AddListener(() =>
           {
               recoveryPanel.gameObject.SetActive(false);
               loginPanel.gameObject.SetActive(true);
           });
    }

    private void SendRecoveryEmail()
    {
        var request = new SendAccountRecoveryEmailRequest();
        request.Email = emailAddressField.text;
        // request.EmailTemplateId = "D2461FA494AE4601";
        request.TitleId = PlayFabSettings.TitleId;

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnSuccess, OnError);
    }

    private void OnSuccess(SendAccountRecoveryEmailResult result)
    {
     //   Debug.Log("Success");
    }

    private void OnError(PlayFabError error)
    {
      //  Debug.Log(error.ErrorMessage);
    }
}
