// using System.Collections;
// using GoogleMobileAds.Api;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// namespace AdManager
// {
//     public class NativeAdController:MonoBehaviour
//     {
//         private const string _adUnitId = "ca-app-pub-1932984074141788/3788716206";
//         public RawImage imageAd;
//         public TextMeshProUGUI headlineAd;
//         public TextMeshProUGUI descAd;
//         private NativeAd nativeAd;

//         public bool nativeAdLoaded;
   
//         public static NativeAdController Instance { get; private set; }

//         // private void OnEnable()
//         // {
//         //     DontDestroyOnLoad(this);
//         // }

//         private void Awake() 
//         { 
//             // If there is an instance, and it's not me, delete myself.
            
//              if (Instance != null && Instance != this) 
//              { 
//                  Destroy(this); 
//              } 
//              else 
//              { 
//                  Instance = this; 
//              }

//             // StartCoroutine(RequestNativeAd());
//         }
        
        
//         public void RequestNativeAd()
//         {
//             Debug.Log("requesting native ad command");
//             AdLoader adLoader = new AdLoader.Builder(_adUnitId)
//                 .ForNativeAd()
//                 .Build();
//             adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
//             adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;

//             adLoader.LoadAd(new AdRequest.Builder().Build());
//         }
        
//         private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
            
//            Debug.Log("Native ad failed to load: ");
//         }
//         private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args) {
//             Debug.Log("Native ad loaded.");
//             this.nativeAdLoaded = true;
//             this.nativeAd = args.nativeAd;
//             HandleErrors();
//         }

//         public void ShowAdOnScreen()
//         {
//             Debug.Log("SHOWING NATIVE ad .");
//             imageAd.texture = nativeAd.GetIconTexture();
//                 headlineAd.text = nativeAd.GetHeadlineText();
//                 descAd.text = nativeAd.GetBodyText();
                
//         }

//         public void HandleErrors()
//         {
//             if (!nativeAd.RegisterIconImageGameObject(imageAd.gameObject))
//             {
//                 Debug.Log("error registering icon");
//             }
//             if (!nativeAd.RegisterHeadlineTextGameObject(headlineAd.gameObject))
//             {
//                 Debug.Log("error registering headline");
//             }
//             if (!nativeAd.RegisterBodyTextGameObject(descAd.gameObject))
//             {
//                 Debug.Log("error registering description");
//             }
//         }
        
        
//     }
// }