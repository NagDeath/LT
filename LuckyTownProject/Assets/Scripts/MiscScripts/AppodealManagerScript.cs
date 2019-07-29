using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using System;

public class AppodealManagerScript : MonoBehaviour, IRewardedVideoAdListener, IInterstitialAdListener
{
    private event Action onFinishedRewardedVideoEvent;

    [Header("Settable fields")]
    [SerializeField] private string appKeyAndroid;
    [SerializeField] private string appKeyIOS;

    public static AppodealManagerScript instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Appodeal.setTesting(false);

        Appodeal.disableLocationPermissionCheck();
        Appodeal.disableWriteExternalStoragePermissionCheck();
#if UNITY_ANDROID
        Appodeal.initialize(appKeyAndroid, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO);
#elif UNITY_IOS
		Appodeal.initialize(appKeyIOS, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO);
#endif

        Appodeal.setInterstitialCallbacks(this);
        Appodeal.setRewardedVideoCallbacks(this);
    }

    public bool CanShowRewardedVideo()
    {
        return Appodeal.canShow(Appodeal.REWARDED_VIDEO);
    }

    public bool CanShowInterstitial()
    {
        return Appodeal.canShow(Appodeal.INTERSTITIAL);
    }

    public void ShowRewardedVideo(Action action)
    {
        if (CanShowRewardedVideo())
        {
            onFinishedRewardedVideoEvent = action;
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }
    }

    public void ShowInterstitial()
    {
        if (CanShowInterstitial())
            Appodeal.show(Appodeal.INTERSTITIAL);
    }
 
    #region Rewarded Video callback handlers
    public void onRewardedVideoLoaded(bool isPrecache) { print("Video loaded"); } //Called when rewarded video was loaded (precache flag shows if the loaded ad is precache).
    public void onRewardedVideoFailedToLoad() { print("Video failed"); } // Called when rewarded video failed to load
    public void onRewardedVideoShown() { print("Video shown"); } // Called when rewarded video is shown
    public void onRewardedVideoClicked() { print("Video clicked"); } // Called when reward video is clicked
    public void onRewardedVideoClosed(bool finished) { print("Video closed"); } // Called when rewarded video is closed
    public void onRewardedVideoFinished(double amount, string name)
    {
        MainThread.Call(() =>
        {
            onFinishedRewardedVideoEvent?.Invoke();
        });
        print("Reward: " + amount + " " + name); } // Called when rewarded video is viewed until the end
    public void onRewardedVideoExpired() { print("Video expired"); } //Called when rewarded video is expired and can not be shown

    public void onInterstitialLoaded(bool isPrecache) { print("Interstitial loaded"); } // Called when interstitial was loaded (precache flag shows if the loaded ad is precache)
    public void onInterstitialFailedToLoad() { print("Interstitial failed"); } // Called when interstitial failed to load
    public void onInterstitialShown() { print("Interstitial opened"); } // Called when interstitial is shown
    public void onInterstitialClosed() { print("Interstitial closed"); } // Called when interstitial is closed
    public void onInterstitialClicked() { print("Interstitial clicked"); } // Called when interstitial is clicked
    public void onInterstitialExpired() { print("Interstitial expired"); } // Called when interstitial is expired and can not be shown

    #endregion
}
