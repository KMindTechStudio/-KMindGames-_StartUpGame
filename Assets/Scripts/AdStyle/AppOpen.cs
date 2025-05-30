using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using System;
using GoogleMobileAds.Common;

public class AppOpen : MonoBehaviour
{
    private void Awake()
    {
        // Use the AppStateEventNotifier to listen to application open/close events.
        // This is used to launch the loaded ad when we open the app.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    void Start()
    {
        MobileAds.Initialize(initStatus => { }); 
    }

    public bool IsAdAvailable
    {
        get
        {
            return appOpenAd != null
                && appOpenAd.CanShowAd()
                && DateTime.Now <_expireTime;
        }
    }

    // These ad units are configured to always serve test ads.
    #if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/9257395921";
    #elif UNITY_IPHONE
       string _adUnitId = "ca-app-pub-3940256099942544/5575463023";
    #else
      private string _adUnitId = "unused";
    #endif

        private AppOpenAd appOpenAd;
        private DateTime _expireTime;

    /// <summary>
    /// Loads the app open ad.
    /// </summary>
    public void LoadAppOpenAd()
        {
            // Clean up the old ad before loading a new one.
            if (appOpenAd != null)
            {
                appOpenAd.Destroy();
                appOpenAd = null;
            }

            Debug.Log("Loading the app open ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            AppOpenAd.Load(_adUnitId, adRequest,
                (AppOpenAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("app open ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("App open ad loaded with response : "
                              + ad.GetResponseInfo());

                    appOpenAd = ad;
                    // App open ads can be preloaded for up to 4 hours.
                    _expireTime = DateTime.Now + TimeSpan.FromHours(4);
                    RegisterReloadHandler(ad);
                });
        }
    private void RegisterReloadHandler(AppOpenAd ad)
    {
    // Raised when the ad closed full screen content.
    ad.OnAdFullScreenContentClosed += () =>
    {
            Debug.Log("App open ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        }
        ;
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
    }

    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (IsAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }

    /// <summary>
    /// Shows the app open ad.
    /// </summary>
    public void ShowAppOpenAd()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }
}
