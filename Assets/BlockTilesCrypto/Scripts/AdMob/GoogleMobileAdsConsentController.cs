using GoogleMobileAds.Ump.Api;
using System;
using PlayFabPersonal.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMobileAdsConsentController : MonoBehaviour
{
    PlayfabDataManager playfabDataManager;

    /// <summary>
    /// If true, it is safe to call MobileAds.Initialize() and load Ads.
    /// </summary>
    public bool CanRequestAds =>
        ConsentInformation.ConsentStatus == ConsentStatus.Obtained ||
        ConsentInformation.ConsentStatus == ConsentStatus.NotRequired;

    // [SerializeField, Tooltip("Button to show user consent and privacy settings.")]
    // private Button _privacyButton;
    //
    // [SerializeField, Tooltip("GameObject with the error popup.")]
    // private GameObject _errorPopup;
    //
    // [SerializeField, Tooltip("Error message for the error popup,")]
    // private TextMeshProUGUI _errorText;

    private ConsentForm _consentForm;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        // Disable the privacy settings button.
        // if (_privacyButton != null)
        // {
        //     _privacyButton.interactable = false;
        // }
        // // Disable the error popup,
        // if (_errorPopup != null)
        // {
        //     _errorPopup.SetActive(false);
        // }

    }

    // }

    public void GatherConsent(Action<string> onComplete)
    {
       // Debug.Log("Gathering consent.");
        
        ConsentRequestParameters requestParameters;
        playfabDataManager = FindAnyObjectByType<PlayfabDataManager>();

  //      var playerId = playfabDataManager.currentPlayerID;
       // var listContainsId = PlayFabDataManager.Instance.debuggersList.Contains(playerId);
        requestParameters = new ConsentRequestParameters
        {
            // False means users are not under age.
            TagForUnderAgeOfConsent = false,
        };
        // if (PlayFabDataManager.Instance.debugNewUserConsentGDPR == 1 && listContainsId)
        // {
        //
        //     DebugGeography debugGeography;
        //     if (PlayFabDataManager.Instance.consentCountry == 1)
        //     {
        //         debugGeography = DebugGeography.NotEEA;
        //     }
        //     else
        //     {
        //         debugGeography = DebugGeography.EEA;
        //     }
        //
        //     requestParameters = new ConsentRequestParameters
        //     {
        //         // False means users are not under age.
        //         TagForUnderAgeOfConsent = false,
        //         ConsentDebugSettings = new ConsentDebugSettings
        //         {
        //             // For debugging consent settings by geography.
        //             DebugGeography = debugGeography,
        //             // https://developers.google.com/admob/unity/test-ads
        //             TestDeviceHashedIds = GoogleMobileAdsController.TestDeviceIds,
        //         }
        //     };
        // }
        // else
        // {
        //     requestParameters = new ConsentRequestParameters
        //     {
        //         // False means users are not under age.
        //         TagForUnderAgeOfConsent = false,
        //     };
        // }

        // Combine the callback with an error popup handler.
        onComplete = (onComplete == null)
            ? UpdateErrorPopup
            : onComplete + UpdateErrorPopup;

        ConsentInformation.Update(requestParameters, (FormError updateError) =>
        {
            // UpdatePrivacyButton();

            if (updateError != null)
            {
                onComplete(updateError.Message);
                return;
            }

            // Determine the consent-related action to take based on the ConsentStatus.
            if (CanRequestAds)
            {
                // Consent has already been gathered or not required.
                // Return control back to the user.
                onComplete(null);
                return;
            }

            // Consent not obtained and is required.
            // Load the initial consent request form for the user.
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
               {
                   //    UpdatePrivacyButton();
                   if (showError != null)
                   {
                       // Form showing failed.
                       onComplete?.Invoke(showError.Message);
                   }
                   // Form showing succeeded.
                   else
                   {
                       onComplete?.Invoke(null);
                   }
               });
        });
    }

    void UpdateErrorPopup(string message)
    {
        // if (string.IsNullOrEmpty(message) || _errorPopup == null)
        // {
        //     return;
        // }
        //
        // if (_errorText != null)
        // {
        //     _errorText.text = message;
        // }
        //
        // _errorPopup.SetActive(true);
    }
}