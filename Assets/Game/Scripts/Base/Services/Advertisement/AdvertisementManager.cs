using System;
using Game.Scripts.Settings;
using Game.Scripts.Base.Services.Analytics;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.Timer;
using Game.Scripts.EventBus;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Base.Services.Advertisement
{
    public sealed class AdvertisementManager : IAdvertisementManager
    {
        public bool IsRewardedAvailable => _advertisementService.IsRewardedAvailable;

        private readonly IAdvertisementService _advertisementService;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly ISettingsProvider _settingsProvider;
        private readonly IAnalyticsService _analyticsService;

        private Action<bool> _onRewardedEnd;
        private Action<bool> _onRewardedReward;
        private AdvertisementSetting _advertisementSetting;

        [Inject]
        public AdvertisementManager(IAdvertisementService advertisementService, ISaveDataHandler saveDataHandler,
            ISettingsProvider settingsProvider, IAnalyticsService analyticsService)
        {
            _advertisementService = advertisementService;
            _saveDataHandler = saveDataHandler;
            _settingsProvider = settingsProvider;
            _analyticsService = analyticsService;
        }

        public void Init()
        {
            _advertisementSetting = _settingsProvider.AdvertisementSetting;
            _advertisementService.Init();
        }

        public void ShowBanner()
        {
            if ( _advertisementService.IsBannerSupported && !_advertisementService.IsBannerShowing )
            {
                _advertisementService.ShowBanner();
            }
        }

        public void ShowPreloader(Action onPreloaderEnd)
        {
            _advertisementService.ShowPreloader(onPreloaderEnd);
        }

        public void ShowInterstitial(Action onInterstitialEnd)
        {
            _analyticsService.SendDesignEvent(DesignEventNames.InterstitialAd);
            _advertisementService.ShowInterstitial(() =>
            {
                EventBus<OnAdWatched>.Raise(new OnAdWatched(true));
                onInterstitialEnd?.Invoke();
            });
        }

        public void ShowRewarded(Action<bool> onRewardedEnd, Action<bool> onRewardedReward)
        {
            _onRewardedEnd = onRewardedEnd;
            _onRewardedReward = onRewardedReward;
            _analyticsService.SendDesignEvent(DesignEventNames.RewardedStart);
            _advertisementService.ShowRewarded(OnRewardedEnd, OnRewardedReward);
        }

        private void OnRewardedEnd(bool success)
        {
            _onRewardedEnd?.Invoke(success);
            _analyticsService.SendDesignEvent(DesignEventNames.RewardedFinish);
            EventBus<OnAdWatched>.Raise(new OnAdWatched(success));
        }

        private void OnRewardedReward(bool success)
        {
            _onRewardedReward?.Invoke(success);
            _analyticsService.SendDesignEvent(DesignEventNames.RewardedReward);
        }
    }
}