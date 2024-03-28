using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class MyBannerAd : MonoBehaviour
{
    [SerializeField] private AdsConfigurations configs;

    private BannerView _bannerView;


    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }


    /// <summary>
    /// Creates a banner view at top of the screen.
    /// </summary>
    private void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Use the AdSize argument to set a custom size for the ad.
        _bannerView = new BannerView(configs.GetBannerAdId(), configs.BannerAdSize, configs.BannerAdPosition);

        // Listen to events the banner may raise.
        ListenToAdEvents();

        Debug.Log("Banner view created.");
    }


    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }


    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Showing banner view.");
            _bannerView.Show();
        }
        else
        {
            LoadAd();
        }

    }

    /// <summary>
    /// Hides the ad.
    /// </summary>
    public void HideAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Hiding banner view.");
            _bannerView.Hide();
        }
    }

    /// <summary>
    /// Destroys the ad.
    /// When you are finished with a BannerView, make sure to call
    /// the Destroy() method before dropping your reference to it.
    /// </summary>
    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }



    /// <summary>
    /// listen to events the banner view may raise.
    /// </summary>
    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
}
