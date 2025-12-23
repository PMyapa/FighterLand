using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class EventRewardAdControl : MonoBehaviour
{
    
    private RewardedInterstitialAd rewardedInterstitialAd;
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


    public void RequestAndLoadRewardedInterstitialAd()
    {
        //statusText.text = "Requesting Rewarded Interstitial Ad.";

        // These ad units are configured to always serve test ads.

        //string adUnitId = "ca-app-pub-3940256099942544/5354046379";
        string adUnitId;
#if UNITY_EDITOR
        adUnitId = "unused";
#elif UNITY_ANDROID
       adUnitId = "ca-app-pub-7784458915951437/8858036756";
#elif UNITY_IPHONE
       adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
       adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
        {
            if (error != null)
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    //statusText.text = "RewardedInterstitialAd load failed, error: " + error;
                });
                return;
            }
            this.rewardedInterstitialAd = rewardedInterstitialAd;
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                //statusText.text = "RewardedInterstitialAd loaded";
                OnAdLoadedEvent.Invoke();
            });
            // Register for ad events.
            this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    //statusText.text = "Rewarded Interstitial presented.";
                });
            };
            this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    //statusText.text = "Rewarded Interstitial dismissed.";
                });
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    //statusText.text = "Rewarded Interstitial failed to present.";
                });
                this.rewardedInterstitialAd = null;
            };
        });
    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show((reward) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    //statusText.text = "User Rewarded: " + reward.Amount;
                    OnUserEarnedRewardEvent.Invoke();
                });
            });
        }
        else
        {
            //statusText.text = "Rewarded ad is not ready yet.";
        }
    }

    /*    public void RequestAndLoadRewardedInterstitialAd()
        {
            PrintStatus("Requesting Rewarded Interstitial ad.");

            // These ad units are configured to always serve test ads.
    #if UNITY_EDITOR
            string adUnitId = "unused";
    #elif UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/5354046379";
    #elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/6978759866";
    #else
                string adUnitId = "unexpected_platform";
    #endif

            // Create a rewarded interstitial.
            RewardedInterstitialAd.Load(adUnitId, CreateAdRequest(),
                (RewardedInterstitialAd ad, LoadAdError loadError) =>
                {
                    if (loadError != null)
                    {
                        PrintStatus("Rewarded intersitial ad failed to load with error: " +
                                    loadError.GetMessage());
                        return;
                    }
                    else if (ad == null)
                    {
                        PrintStatus("Rewarded intersitial ad failed to load.");
                        return;
                    }

                    PrintStatus("Rewarded interstitial ad loaded.");
                    rewardedInterstitialAd = ad;
                    OnAdLoadedEvent.Invoke();

                    ad.OnAdFullScreenContentOpened += () =>
                    {
                        PrintStatus("Rewarded intersitial ad opening.");
                        OnAdOpeningEvent.Invoke();
                    };
                    ad.OnAdFullScreenContentClosed += () =>
                    {
                        PrintStatus("Rewarded intersitial ad closed.");
                        OnAdClosedEvent.Invoke();
                    };
                    ad.OnAdImpressionRecorded += () =>
                    {
                        PrintStatus("Rewarded intersitial ad recorded an impression.");
                    };
                    ad.OnAdClicked += () =>
                    {
                        PrintStatus("Rewarded intersitial ad recorded a click.");
                    };
                    ad.OnAdFullScreenContentFailed += (AdError error) =>
                    {
                        PrintStatus("Rewarded intersitial ad failed to show with error: " +
                                    error.GetMessage());
                    };
                    ad.OnAdPaid += (AdValue adValue) =>
                    {
                        OnUserEarnedRewardEvent.Invoke();

                        string msg = string.Format("{0} (currency: {1}, value: {2}",
                                                    "Rewarded intersitial ad received a paid event.",
                                                    adValue.CurrencyCode,
                                                    adValue.Value);
                        PrintStatus(msg);
                    };
                });
        }

        public void ShowRewardedInterstitialAd()
        {
            if (rewardedInterstitialAd != null)
            {
                rewardedInterstitialAd.Show((Reward reward) =>
                {
                    PrintStatus("Rewarded interstitial granded a reward: " + reward.Amount);
                });
            }
            else
            {
                PrintStatus("Rewarded Interstitial ad is not ready yet.");
            }
        }
    */

    #endregion


    #region AD INSPECTOR
    /*
        public void OpenAdInspector()
        {
            //statusText.text = "Open Ad Inspector.";

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

