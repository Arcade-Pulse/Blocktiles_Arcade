using AppodealAds.Unity.Common;

using UnityEngine;
using AppodealAds.Unity.Api;

    public class Rewarded:MonoBehaviour,IRewardedVideoAdListener
    {
        public static Rewarded Instance { get; private set; }
        public bool isAdLoaded;

        public void ShowAd()
        {
            
            if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO)) {
                Appodeal.show(Appodeal.REWARDED_VIDEO);
            }
            else
            {
                LoadRewardedAd();
            }
        }
        
        public void LoadRewardedAd()
        {
            if (!AppodealAds.Unity.Api.Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
            {
                // Ad is not loaded, so we need to load it
                AppodealAds.Unity.Api.Appodeal.cache(Appodeal.REWARDED_VIDEO);
                Debug.Log("Interstitial Ad is being loaded.");
            }
            else
            {
                // Ad is already loaded
                Debug.Log("Interstitial Ad is already loaded.");
            }
        }
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(this);


        }
        public void onRewardedVideoLoaded(bool precache)
        {
            Debug.Log("rewarded video loaded");

        }

        public void onRewardedVideoFailedToLoad()
        {
            Debug.Log("rewarded video failed to load");
        }

        public void onRewardedVideoShowFailed()
        {
            Debug.Log("rewarded video show failed");
        }

        public void onRewardedVideoShown()
        {
            Debug.Log("rewarded video shown ");
        }

        public void onRewardedVideoFinished(double amount, string name)
        {
            Debug.Log("rewarded video show finished");
        }

        public void onRewardedVideoClosed(bool finished)
        {
            Debug.Log("rewarded video show closed");
        }

        public void onRewardedVideoExpired()
        {
            Debug.Log("rewarded video expired");
        }

        public void onRewardedVideoClicked()
        {
            Debug.Log("rewarded video clicked");
        }
    }