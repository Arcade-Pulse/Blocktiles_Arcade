// using System.Collections;
// using System.Collections.Generic;
// using AppodealAds.Unity.Api;
// using AppodealAds.Unity.Common;
// using UnityEngine;
//
// public class Banner : MonoBehaviour,IBannerAdListener
// {
//     #region Banner callback handlers
//
//     public bool isAdLoaded;
//     private void OnEnable()
//     {
//       //  Appodeal.setBannerCallbacks(this);
//
//     }
// // Called when a banner is loaded (height arg shows banner's height, precache arg shows if the loaded ad is precache
//     public void onBannerLoaded(int height, bool precache)
//     {
//         Debug.Log("Banner loaded");
//         isAdLoaded = true;
//     }
//
// // Called when banner failed to load
//     public void onBannerFailedToLoad()
//     {
//         Debug.Log("Banner failed to load"); 
//
//        // Appodeal.cache(Appodeal.BANNER_BOTTOM);
//
//     }
//     
//
//
// // Called when banner is shown
//     public void onBannerShown()
//     {
//         Debug.Log("Banner shown");
//     }
//
// // Called when banner failed to show
//     public void onBannerShowFailed()
//     {
//         Debug.Log("Banner show failed");
//     }
//
// // Called when banner is clicked
//     public void onBannerClicked()
//     {
//         Debug.Log("Banner clicked");
//     }
//
// // Called when banner is expired and can not be shown
//     public void onBannerExpired()
//     {
//         Debug.Log("Banner expired");
//     }
//
//     #endregion
// }
