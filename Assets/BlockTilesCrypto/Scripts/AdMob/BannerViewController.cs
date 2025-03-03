// using System;
// using UnityEngine;
// using GoogleMobileAds.Api;
//
// [AddComponentMenu("GoogleMobileAds/BannerViewController")]
// public class BannerViewController : MonoBehaviour
// {
//     public static BannerViewController Instance { get; private set; }
//    // public static bool DevelopmentMode { get; set; } = false;
//    private string adBannerID = AdsID.adBannerID;
//
//     private BannerView _bannerView;
//     public bool isBannerLoaded = false;
//     public bool collapsible;
//
//     public static event EventHandler OnLoadFailed;
//
//     private void Awake()
//     {
//         // If there is an instance, and it's not me, delete myself.
//
//         if (Instance != null && Instance != this)
//         {
//             Destroy(this);
//         }
//         else
//         {
//             Instance = this;
//         }
//
//         DontDestroyOnLoad(this);
//     }
//
//     public void CreateBannerView()
//     {
//         
//       //  Debug.Log("Creating banner view.");
//
//         // If we already have a banner, destroy the old one.
//         if (_bannerView != null)
//         {
//             DestroyAd();
//         }
//        // AdSize bannerSize = AdSize.IABBanner; // For a 300x250 banner
//        // AdSize customSize = new AdSize(330, 80);
//          AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
//
//         // Create a 320x50 banner at top of the screen.
//         _bannerView = new BannerView(adBannerID,adaptiveSize, AdPosition.Bottom);
//       //  Debug.Log("Current loaded bannerView ad serving adapter " + _bannerView.GetResponseInfo().GetMediationAdapterClassName());
//         // OnLoaded?.Invoke(this, EventArgs.Empty);
//         // Listen to events the banner may raise.
//         ListenToAdEvents();
//
//       //  Debug.Log("Banner view created.");
//         isBannerLoaded = true;
//     }
//
//     public void LoadAd()
//     {
//         
//         // Create an instance of a banner view first.
//         if (_bannerView == null)
//         {
//             CreateBannerView();
//         }
//
//         // Create our request used to load the ad.
//         var adRequest = new AdRequest();
//
//         if (collapsible)
//         {
//             adRequest.Extras.Add("collapsible", "bottom");
//         }
//
//         // Send the request to load the ad.
//         //Debug.Log("Loading banner ad.");
//         _bannerView.LoadAd(adRequest);
//     }
//
//     public void ShowAd()
//     {
//  
//         if (_bannerView != null)
//         {
//           //  Debug.Log("Showing banner view.");
//             _bannerView.Show();
//         }
//     }
//
//     public void DestroyAd()
//     {
//         if (_bannerView != null)
//         {
//          //   Debug.Log("Destroying banner view.");
//             _bannerView.Destroy();
//             _bannerView = null;
//             isBannerLoaded = false;
//         }
//     }
//
//     private void ListenToAdEvents()
//     {
//         // Raised when an ad is loaded into the banner view.
//         _bannerView.OnBannerAdLoaded += () =>
//         {
//             // Debug.Log("Banner view loaded an ad with response : "
//             //     + _bannerView.GetResponseInfo());
//             isBannerLoaded = true;
//             Debug.Log("Banner view loaded an ad with response : "
//                       + _bannerView.GetResponseInfo());
//             Debug.Log("Ad Height: {0}, width: {1}"+ _bannerView.GetHeightInPixels()+"and "
//                 +_bannerView.GetWidthInPixels());
//         };
//         // Raised when an ad fails to load into the banner view.
//         _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
//         {
//             Debug.Log("Banner view failed to load an ad with error : " + error);
//             OnLoadFailed?.Invoke(this, EventArgs.Empty);
//         };
//         // Raised when the ad is estimated to have earned money.
//         _bannerView.OnAdPaid += (AdValue adValue) =>
//         {
//             // Debug.Log(String.Format("Banner view paid {0} {1}.",
//             //     adValue.Value,
//             //     adValue.CurrencyCode));
//         };
//         // Raised when an impression is recorded for an ad.
//         _bannerView.OnAdImpressionRecorded += () =>
//         {
//           //  Debug.Log("Banner view recorded an impression.");
//         };
//         // Raised when a click is recorded for an ad.
//         _bannerView.OnAdClicked += () =>
//         {
//            // Debug.Log("Banner view was clicked.");
//         };
//         // Raised when an ad opened full screen content.
//         _bannerView.OnAdFullScreenContentOpened += () =>
//         {
//           //  Debug.Log("Banner view full screen content opened.");
//         };
//         // Raised when the ad closed full screen content.
//         _bannerView.OnAdFullScreenContentClosed += () =>
//         {
//           //  Debug.Log("Banner view full screen content closed.");
//             // LoadAd();
//         };
//     }
// }
