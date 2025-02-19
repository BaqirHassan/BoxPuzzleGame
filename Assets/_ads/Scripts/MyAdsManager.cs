using UnityEngine;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public class MyAdsManager : MonoSingleton<MyAdsManager>
{
    [SerializeField] private AdsConfigurations configs;
    [SerializeField] private MyBannerAd bannerAd;
    [SerializeField] private MyInterstitialAd interstitailAd;
    [SerializeField] private MyRewardedAd rewardedAd;
    [SerializeField] private MyAppOpenAd appOpenAd;
    public void Start()
    {
        //Todo: Make the ads classes so that they dont need to be a monobehaviour

        if (bannerAd == null)
        {
            if (!TryGetComponent(out bannerAd))
            {
                bannerAd = gameObject.AddComponent<MyBannerAd>();
                bannerAd.configs = configs;
            }
        }

        if (interstitailAd == null)
        {
            if (!TryGetComponent(out interstitailAd))
            {
                interstitailAd = gameObject.AddComponent<MyInterstitialAd>();
                interstitailAd.configs = configs;
            }
        }

        if (rewardedAd == null)
        {
            if (!TryGetComponent(out rewardedAd))
            {
                rewardedAd = gameObject.AddComponent<MyRewardedAd>();
                rewardedAd.configs = configs;
            }
        }

        if (appOpenAd == null)
        {
            if (!TryGetComponent(out appOpenAd))
            {
                appOpenAd = gameObject.AddComponent<MyAppOpenAd>();
                appOpenAd.configs = configs;
            }
        }

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
                        appOpenAd.LoadAd();
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

    public void ShowInterstitialAd()
    {
        interstitailAd.ShowAd();
    }

    public void ShowRewardedAd()
    {
        rewardedAd.ShowAd(() => Debug.LogError("Delete me: Rewarded Was Successfull. Give user Reward."));
    }
































}
