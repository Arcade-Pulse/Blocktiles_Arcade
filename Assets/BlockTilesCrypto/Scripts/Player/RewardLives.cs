using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;
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

        if (RewardedAdController.Instance.isAdLoaded)
        {
            Debug.Log("rewarded is loaded");
           // RewardedAdController.Instance.ShowAd();
           RewardedAdController.Instance.ShowAd();
           Debug.Log("added lives "+PlayfabDataManager.Instance.GetLifeRewardPerAd());

        }
        else
        {            Debug.Log("rewarded is not loaded");

            RewardedAdController.Instance.LoadAd();
            RewardedInterstitialAdController.Instance.ShowAd();
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
