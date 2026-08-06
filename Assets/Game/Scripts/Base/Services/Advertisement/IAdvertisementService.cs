using System;

namespace Game.Scripts.Base.Services.Advertisement
{
    public interface IAdvertisementService
    {
        bool IsBannerSupported { get; }
        bool IsBannerShowing { get; }
        bool IsRewardedAvailable { get; }

        void Init();
        void DeInit();

        void ShowBanner();

        void ShowPreloader(Action onPreloaderEnd);
        void ShowInterstitial(Action onInterstitialEnd);
        void ShowRewarded(Action<bool> onRewardedEnd, Action<bool> onRewardedReward);
    }
}