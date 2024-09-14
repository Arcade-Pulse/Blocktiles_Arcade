using System;
using System.Collections;
using CodeStage.AntiCheat.Storage;
using Global;
using Google.Play.AppUpdate;
using Google.Play.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InAppUpdateManager : MonoBehaviour
{
    [SerializeField] private GameObject appUpdateContainer;
    [SerializeField] private Button updateButton;

    private AppUpdateManager appUpdateManager;
    private AppUpdateInfo appUpdateInfo;
    public static event EventHandler<OnUpdateAvailableEventArgs> OnUpdateAvailable;
    public class OnUpdateAvailableEventArgs : EventArgs
    {
        public bool isUpdateAvailable;
    }

    private void OnEnable()
    {
     //   DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (updateButton!=null)
        {
            updateButton.onClick.AddListener(CheckUpdate);
        }
        appUpdateContainer.SetActive(false);
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.CASHOUTSELECTED);

#if UNITY_ANDROID && !UNITY_EDITOR
        {
        // Initialize the AppUpdateManager.
        appUpdateManager = new AppUpdateManager();

        // Check for an updaate when the game starts.
        StartCoroutine(CheckForUpdate());
        }
#endif
#if UNITY_EDITOR
        {
            SceneManager.LoadScene("AuthenticationScene");
      //     GoogleMobileAdsController.Instance.AdsConfiguration();

          //  OnUpdateAvailable?.Invoke(this, new OnUpdateAvailableEventArgs { isUpdateAvailable = false });
        }
#endif
    }

    public void CheckUpdate()
    {
        StartCoroutine(StartImmediateUpdate());
    }

    private IEnumerator CheckForUpdate()
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            appUpdateInfo = appUpdateInfoOperation.GetResult();

            // Check if an update is available and it's an immediate update.
            if (appUpdateInfo.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                appUpdateContainer.SetActive(true);
                OnUpdateAvailable?.Invoke(this, new OnUpdateAvailableEventArgs { isUpdateAvailable = true });
                      //  SceneManager.LoadScene("AuthenticationScene");

            }
            else
            {
                appUpdateContainer.SetActive(false);
               // OnUpdateAvailable?.Invoke(this, new OnUpdateAvailableEventArgs { isUpdateAvailable = false });
                SceneManager.LoadScene("AuthenticationScene");
            }
        }
        else
        {
            // Handle the error if AppUpdateInfo retrieval fails.
            Debug.Log("Error getting AppUpdateInfo: " + appUpdateInfoOperation.Error);
          //  OnUpdateAvailable?.Invoke(this, new OnUpdateAvailableEventArgs { isUpdateAvailable = false });
                   SceneManager.LoadScene("AuthenticationScene");
                   // SceneManager.LoadScene("AuthenticationScene");
        }
    }

    // Called when the update button is clicked.
    private IEnumerator StartImmediateUpdate()
    {
        if (appUpdateInfo != null)
        {
            
            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

            // Creates an AppUpdateRequest that can be used to monitor the
            // requested in-app update flow.

            var UpdateRequest = appUpdateManager.StartUpdate(
                // The result returned by PlayAsyncOperation.GetResult().
                appUpdateInfo,
                // The AppUpdateOptions created defining the requested in-app update
                // and its parameters.
                appUpdateOptions);
            yield return UpdateRequest;

            // If the update completes successfully, then the app restarts and this line
            // is never reached. If this line is reached, then handle the failure (for
            // example, by logging result.Error or by displaying a message to the user).
            
           RegisterEventHandlers(UpdateRequest);
        }
    }

    private void RegisterEventHandlers(AppUpdateRequest updateRequest)
    {
        AppUpdateErrorCode errorCode = updateRequest.Error;
      //  Debug.Log("Error Code: " + errorCode.ToString());
        switch (errorCode)
        {
            // Handle various error codes as needed.
            case AppUpdateErrorCode.ErrorUserCanceled:
                StartCoroutine(CheckForUpdate());
                break;
        }
    }
}
