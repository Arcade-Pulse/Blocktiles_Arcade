using System;
using CodeStage.AntiCheat.Storage;
using Global;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public void Logout()
    {
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.LAST_LOGIN);
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.FACEBOOK);
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.GOOGLE);
        ObscuredPrefs.DeleteKey(PlayerPrefNameString.GUEST);

        // ObscuredPrefs.DeleteKey("LoginWithFacebook");
        // ObscuredPrefs.DeleteKey("LoginGuest");
        // ObscuredPrefs.DeleteKey("LastLogin");
        SceneManager.LoadScene("AuthenticationScene");
    }

    private void Start()
    {
        // var request = new ExecuteCloudScriptRequest
        // {
        //     FunctionName = "checkBatchPayoutStatus",
        //     GeneratePlayStreamEvent = true,
        //     FunctionParameter = new
        //     {
        //         playerFabId = "9"
        //     },
        //     RevisionSelection = CloudScriptRevisionOption.Latest
        // };

        // PlayFabClientAPI.ExecuteCloudScript(request, OnSuccess, OnError);
    }

    private void OnSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log(result.ToString());
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }
}
