using System;
using CodeStage.AntiCheat.ObscuredTypes;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFabPersonal.Managers;
using TMPro;
using UnityEngine;

public class VerificationManager : MonoBehaviour
{
    public GameObject challengeWindow;

    private void OnEnable()
    {
        GetPlayerChallengeStatus();
       // Debug.Log("ver manager enabled");
    }

    public void GetPlayerChallengeStatus()
    {
//        Debug.Log("getting player challenge status");
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnSuccessGetPlayerChallenge, OnErrorGetPlayerChallenge);
        }

    }

    private void OnSuccessGetPlayerChallenge(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("playerChallenged") && result.Data.ContainsKey("playerChancesLeft"))
        {
//            Debug.Log("contains player challenged");
            PlayfabDataManager.Instance.isPlayerChallenged = int.Parse(result.Data["playerChallenged"].Value);
            var numberOfTriesLeft = int.Parse(result.Data["playerChancesLeft"].Value);
            if (PlayfabDataManager.Instance.isPlayerChallenged == 1 && numberOfTriesLeft > 0)
            {
               // Debug.Log("ok player is challenged");
                challengeWindow.SetActive(true);
            }
        }
        else
        {
            Debug.Log("ok player is not challenged");
            if (challengeWindow!=null)
            {
                Debug.Log("windows null");

                challengeWindow.SetActive(false);

            }
            PlayfabDataManager.Instance.isPlayerChallenged = 0;
        }

    }

    private void OnErrorGetPlayerChallenge(PlayFabError error)
    {
        Debug.Log("error getting challenge");
       // Debug.Log(error.GenerateErrorReport());
    }
}
