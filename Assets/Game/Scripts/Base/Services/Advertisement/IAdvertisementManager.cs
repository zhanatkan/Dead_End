using System;

namespace Game.Scripts.Base.Services.Advertisement
{
    public interface IAdvertisementManager
    {
        bool IsRewardedAvailable { get; }
        
        void Init();

        void ShowPreloader(Action onPreloaderEnd);
        void ShowInterstitial(Action onInterstitialEnd);
        void ShowRewarded(Action<bool> onRewardedEnd, Action<bool> onRewardedReward);
        void ShowBanner();
    }
}