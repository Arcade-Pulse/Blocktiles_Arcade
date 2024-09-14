using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;
using UnityEditor.Minesweeper.Scripts.UnityAds;
using UnityEngine;
using UnityEngine.UI;

public class RewardLives : MonoBehaviour
{
    public Button rewardButton;
    public Button rewardButton2;
    
    public void RewardLife()
    {
       // Debug.Log("reward life button pressed"+RewardedAdController.Instance.inGameRequest);
// #if UNITY_EDITOR
//         if (PlayfabDataManager.Instance.GetLives() >= PlayfabDataManager.Instance.GetTotalPlayerLives())
//         {
//             MessageManager.OnErrorShowMessage?.Invoke("You have full life now");
//             rewardButton.interactable = true;
//             rewardButton2.interactable = true;
//             return;
//         }
//         else
//         {
//             VirtualCurrency.Instance.AddLife(PlayfabDataManager.Instance.GetLifeRewardPerAd());
//            return;
//         }
//
// #endif
        rewardButton.interactable = false;
        rewardButton2.interactable = false;
        if (PlayfabDataManager.Instance.GetLives() >= PlayfabDataManager.Instance.GetTotalPlayerLives())
        {
            MessageManager.OnErrorShowMessage?.Invoke("You have full life now");
            rewardButton.interactable = true;
            rewardButton2.interactable = true;
            return;
        }

        if (Rewarded.Instance.isAdLoaded)
        {
            Debug.Log("rewarded is loaded");
           // RewardedAdController.Instance.ShowAd();
           Rewarded.Instance.ShowAd();
           VirtualCurrency.Instance.AddLife(PlayfabDataManager.Instance.GetLifeRewardPerAd());

        }
        else
        {            Debug.Log("rewarded is not loaded");

            Rewarded.Instance.LoadRewardedAd();
        }
        
        rewardButton.interactable = true;
        rewardButton2.interactable = true;

        // if (!PlayfabDataManager.Instance.GetIsLifeRewarded())
        // {
        //   //  PlayfabDataManager.Instance.SetIsLifeRewarded(true);
        //
        // }
    }
}
