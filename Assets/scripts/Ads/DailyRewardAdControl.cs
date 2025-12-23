using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class DailyRewardAdControl : MonoBehaviour
{
    private RewardedAd rewardedAd;
    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;


    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
       /* MobileAds.SetiOSAppPauseOnBackground(true);
       
        MobileAds.Initialize(HandleInitCompleteAction);*/
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            //statusText.text = "Initialization complete";
            //RequestBannerAd();
        });
    }


    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }


    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        //statusText.text = "Requesting Rewarded Ad.";

        //string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        string adUnitId;
#if UNITY_EDITOR
        adUnitId = "unused";
#elif UNITY_ANDROID
         adUnitId = "ca-app-pub-7784458915951437/7856371308";
#elif UNITY_IPHONE
         adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
         adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
        rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
        rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
        else
        {
            //statusText.text = "Rewarded ad is not ready yet.";
        }
    }



    /*
        public void RequestAndLoadRewardedAd()
        {
            PrintStatus("Requesting Rewarded ad.");
    #if UNITY_EDITOR
            string adUnitId = "unused";
    #elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5224354917";
    #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
    #else
            string adUnitId = "unexpected_platform";
    #endif

            // create new rewarded ad instance
            RewardedAd.Load(adUnitId, CreateAdRequest(),
                (RewardedAd ad, LoadAdError loadError) =>
                {
                    if (loadError != null)
                    {
                        PrintStatus("Rewarded ad failed to load with error: " +
                                    loadError.GetMessage());
                        return;
                    }
                    else if (ad == null)
                    {
                        PrintStatus("Rewarded ad failed to load.");
                        return;
                    }

                    OnAdLoadedEvent.Invoke();

                    PrintStatus("Rewarded ad loaded.");
                    rewardedAd = ad;

                    ad.OnAdFullScreenContentOpened += () =>
                    {
                        PrintStatus("Rewarded ad opening.");
                        OnAdOpeningEvent.Invoke();
                    };
                    ad.OnAdFullScreenContentClosed += () =>
                    {
                        PrintStatus("Rewarded ad closed.");
                        OnAdClosedEvent.Invoke();
                    };
                    ad.OnAdImpressionRecorded += () =>
                    {
                        PrintStatus("Rewarded ad recorded an impression.");
                    };
                    ad.OnAdClicked += () =>
                    {
                        PrintStatus("Rewarded ad recorded a click.");
                    };
                    ad.OnAdFullScreenContentFailed += (AdError error) =>
                    {
                        PrintStatus("Rewarded ad failed to show with error: " +
                                   error.GetMessage());
                    };
                    ad.OnAdPaid += (AdValue adValue) =>
                    {
                        OnUserEarnedRewardEvent.Invoke();
                        string msg = string.Format("{0} (currency: {1}, value: {2}",
                                                   "Rewarded ad received a paid event.",
                                                   adValue.CurrencyCode,
                                                   adValue.Value);
                        PrintStatus(msg);
                    };
                });
        }

        public void ShowRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Show((Reward reward) =>
                {
                    PrintStatus("Rewarded ad granted a reward: " + reward.Amount);
                });
            }
            else
            {
                PrintStatus("Rewarded ad is not ready yet.");
            }
        }*/

    #endregion



    #region AD INSPECTOR
    /*
        public void OpenAdInspector()
        {
            statusText.text = "Open Ad Inspector.";

            MobileAds.OpenAdInspector((error) =>
            {
                if (error != null)
                {
                    string errorMessage = error.GetMessage();
                    MobileAdsEventExecutor.ExecuteInUpdate(() => {
                        statusText.text = "Ad Inspector failed to open, error: " + errorMessage;
                    });
                }
                else
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() => {
                        statusText.text = "Ad Inspector closed.";
                    });
                }
            });
        }*/

    #endregion

    private void PrintStatus(string message)
    {
        Debug.Log(message);
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            //statusText.text = message;
        });
    }
}


