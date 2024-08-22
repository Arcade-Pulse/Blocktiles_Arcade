using PlayFabPersonal.Managers;
using UnityEngine;

public class DevelopmentManager : MonoBehaviour
{
    [SerializeField] private bool developmentMode;
    [SerializeField] private bool isAdAllowed;

    private void Start()
    {
        //PlayfabRequests.DevelopmentMode = developmentMode;
      //   if (!isAdAllowed)
      //   {
      //       GoogleMobileAdsController.DevelopmentMode = developmentMode;
      //       AppOpenAdController.DevelopmentMode = developmentMode;
      //       BannerViewController.DevelopmentMode = developmentMode;
      //       InterstitialAdController.DevelopmentMode = developmentMode;
      //       RewardedAdController.DevelopmentMode = developmentMode;
      //       RewardedInterstitialAdController.DevelopmentMode = developmentMode;
      //   }
      // //  PlayfabRequests.UpdateRevisionOption();
      //
      //   DontDestroyOnLoad(this);
    }


}
