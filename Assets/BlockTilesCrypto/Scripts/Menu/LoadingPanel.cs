using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using Facebook.Unity;
using Global;
using GooglePlayGames;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPanel : MonoBehaviour
{
    public float targetTime;

    // public GameObject loginScreen;
        
    public static LoadingPanel Instance;
        
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        targetTime = 30f;
    }

    //         public void OnDisable()
//         {
// //            Debug.Log("disabled connection status object");
//         }

    public void Update()
    {
        targetTime -= Time.deltaTime;
        
        if (targetTime <= 0.0f)
        {
            TimerEnded();
        }

        // if (PlayFabClientAPI.IsClientLoggedIn())
        // {
        //     this.gameObject.SetActive(false);
        // }
    }
        
    private void TimerEnded()
    {
        //  loginScreen = FindObjectOfType<LoginMenu>().gameObject;
        //loginScreen.SetActive(true);
        this.gameObject.SetActive(false);
        LogOut();

        //  PlayerPrefs.DeleteAll();
        //  SceneManager.LoadScene(0);

    }
    
    public void LogOut(){
      //  Debug.Log("ForgetAllCredentials calls");

        if (PlayGamesPlatform.Instance != null)
        {
            
        }

        if (FB.IsLoggedIn)
        {
            FB.LogOut();
        }
        PlayFabClientAPI.ForgetAllCredentials();
        // GoogleSignIn.DefaultInstance.Disconnect();
        // GoogleSignIn.DefaultInstance.SignOut();
      //  Debug.Log("ForgetAllCredentials calls 2");
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.LAST_LOGIN);
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.FACEBOOK);
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.GOOGLE);
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.GUEST);
        //ObscuredPrefs.DeleteAll();
        

}
}
