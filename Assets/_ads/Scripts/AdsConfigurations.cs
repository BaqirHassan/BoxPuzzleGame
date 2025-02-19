using GoogleMobileAds.Api;
using UnityEngine;

[CreateAssetMenu(fileName = "AdsConfigurations", menuName = "ScriptableObjects/AdmobAdsConfigrations", order = 1)]
public class AdsConfigurations : ScriptableObject
{
    [Header("Ads Keys Android")]
    [SerializeField] private string BannerAdId_GP = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] private string InterstitialAdId_GP = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] private string RewardedInterstitialAdId_GP = "ca-app-pub-3940256099942544/5354046379";
    [SerializeField] private string RewardedAdId_GP = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] private string AppOpenId_GP = "ca-app-pub-3940256099942544/9257395921";
    [SerializeField] private string MrecAdId_GP = "ca-app-pub-3940256099942544/6300978111";
    
    [Space, Space, Space, Header("Configurations")]
    [SerializeField] private AdSize BannerAdSize_GP = new AdSize(320, 50);
    [SerializeField] private AdPosition BannerAdPosition_GP = AdPosition.Bottom;



    [Space, Space, Space, Header("Ads Keys iOS")]
    [SerializeField] private string BannerAdId_iOS = "ca-app-pub-3940256099942544/2934735716";
    [SerializeField] private string InterstitialAdId_iOS = "ca-app-pub-3940256099942544/4411468910";
    [SerializeField] private string RewardedInterstitialAdId_iOS = "ca-app-pub-3940256099942544/6978759866";
    [SerializeField] private string RewardedAdId_iOS = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] private string AppOpenId_iOS = "ca-app-pub-3940256099942544/5575463023";
    [SerializeField] private string MrecAdId_iOS = "ca-app-pub-3940256099942544/6300978111";

    [Space, Space, Space, Header("Configurations")]
    [SerializeField] private AdSize BannerAdSize_iOS = new AdSize(320, 50);
    [SerializeField] private AdPosition BannerAdPosition_iOS = AdPosition.Bottom;


    public string GetBannerAdId()
    {
#if UNITY_IOS
        return BannerAdId_iOS;
#elif UNITY_ANDROID
        return BannerAdId_GP;
#endif
    }

    public AdSize GetBannerSize()
    {
#if UNITY_IOS
        return BannerAdSize_iOS;
#elif UNITY_ANDROID
        return BannerAdSize_GP;
#endif
    }

    public AdPosition GetBannerPosition()
    {
#if UNITY_IOS
        return BannerAdPosition_iOS;
#elif UNITY_ANDROID
        return BannerAdPosition_GP;
#endif
    }

    public string GetInterstitialAdId()
    {
#if UNITY_IOS
        return InterstitialAdId_iOS;
#elif UNITY_ANDROID
        return InterstitialAdId_GP;
#endif
    }

    public string GetMrecAdId()
    {
#if UNITY_IOS
        return MrecAdId_iOS;
#elif UNITY_ANDROID
        return MrecAdId_GP;
#endif
    }

    public string GetRewardedAdId()
    {
#if UNITY_IOS
        return RewardedAdId_iOS;
#elif UNITY_ANDROID
        return RewardedAdId_GP;
#endif
    }

    public string GetAppOpenAdId()
    {
#if UNITY_IOS
        return AppOpenId_iOS;
#elif UNITY_ANDROID
        return AppOpenId_GP;
#endif
    }

    public string GetRewardedInterstitialAdId()
    {
#if UNITY_IOS
        return RewardedAdId_iOS;
#elif UNITY_ANDROID
        return RewardedAdId_GP;
#endif
    }
}
