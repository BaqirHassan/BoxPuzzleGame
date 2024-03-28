using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoSingleton<AdsManager>
{
    [SerializeField] private AdsConfigurations configs;
    [SerializeField] private MyBannerAd bannerAd;
    [SerializeField] private MyInterstitialAd interstitailAd;
    [SerializeField] private MyRewardedAd rewardedAd;

    public void Start()
    {
        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");

                        bannerAd.LoadAd();
                        interstitailAd.LoadAd();
                        rewardedAd.LoadAd();
                        break;
                }
            }
        });
    }

    #region Banner ad
    public void ShowBanner()
    {
        bannerAd.ShowAd();
    }

    public void HideBanner()
    {
        bannerAd.HideAd();
    }

    #endregion

































}
