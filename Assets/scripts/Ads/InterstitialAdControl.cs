using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class InterstitialAdControl : MonoBehaviour
{

    private InterstitialAd interstitialAd;
    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;
    //public Text fpsMeter;
    //public Text statusText;


    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);


        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

#if UNITY_IPHONE
        deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
        deviceIds.Add("e4c0ce3589cae808450f963be4428e77");
#endif

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);


        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);

       // RequestAndLoadInterstitialAd();
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



    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        //statusText.text = "Requesting Interstitial Ad.";

        //string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        string adUnitId;
#if UNITY_EDITOR
        adUnitId = "unused";
#elif UNITY_ANDROID
         adUnitId = "ca-app-pub-7784458915951437/4192458464";
#elif UNITY_IPHONE
         adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
         adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        interstitialAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        interstitialAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            //statusText.text = "Interstitial ad is not ready yet";
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }


    /*    public void RequestAndLoadInterstitialAd()
        {
            PrintStatus("Requesting Interstitial ad.");

    #if UNITY_EDITOR
            string adUnitId = "unused";
    #elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";
    #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
            string adUnitId = "unexpected_platform";
    #endif

            // Clean up interstitial before using it
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
            }

            // Load an interstitial ad
            InterstitialAd.Load(adUnitId, CreateAdRequest(),
                (InterstitialAd ad, LoadAdError loadError) =>
                {
                    if (loadError != null)
                    {
                        PrintStatus("Interstitial ad failed to load with error: " +
                            loadError.GetMessage());
                        return;
                    }
                    else if (ad == null)
                    {
                        PrintStatus("Interstitial ad failed to load.");
                        return;
                    }

                    PrintStatus("Interstitial ad loaded.");
                    interstitialAd = ad;

                    ad.OnAdFullScreenContentOpened += () =>
                    {
                        PrintStatus("Interstitial ad opening.");
                        OnAdOpeningEvent.Invoke();
                    };
                    ad.OnAdFullScreenContentClosed += () =>
                    {
                        PrintStatus("Interstitial ad closed.");
                        OnAdClosedEvent.Invoke();
                    };
                    ad.OnAdImpressionRecorded += () =>
                    {
                        PrintStatus("Interstitial ad recorded an impression.");
                    };
                    ad.OnAdClicked += () =>
                    {
                        PrintStatus("Interstitial ad recorded a click.");
                    };
                    ad.OnAdFullScreenContentFailed += (AdError error) =>
                    {
                        PrintStatus("Interstitial ad failed to show with error: " +
                                    error.GetMessage());
                    };
                    ad.OnAdPaid += (AdValue adValue) =>
                    {
                        string msg = string.Format("{0} (currency: {1}, value: {2}",
                                                   "Interstitial ad received a paid event.",
                                                   adValue.CurrencyCode,
                                                   adValue.Value);
                        PrintStatus(msg);
                    };
                });
        }

        public void ShowInterstitialAd()
        {
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                interstitialAd.Show();
            }
            else
            {
                PrintStatus("Interstitial ad is not ready yet.");
            }
        }*/

  

    #endregion



    #region AD INSPECTOR

    /*   public void OpenAdInspector()
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
