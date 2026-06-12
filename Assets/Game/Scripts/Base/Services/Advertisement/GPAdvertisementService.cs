#if UNITY_WEBGL && GAME_PUSH
using System;
using GamePush;

namespace Game.Scripts.Base.Services.Advertisement
{
    public sealed class GPAdvertisementService : IAdvertisementService
    {
        public bool IsBannerSupported => GP_Ads.IsStickyAvailable();
        public bool IsBannerShowing => GP_Ads.IsStickyPlaying();
        public bool IsRewardedAvailable => GP_Ads.IsRewardedAvailable();

        private Action _onPreloaderEnd;
        private Action _onInterstitialEnd;
        private Action<bool> _onRewardedEnd;
        private Action<bool> _onRewardedReward;
        
        private bool _isRewarded;

        private bool _isPreloaderShowing;
        private bool _isInterstitialShowing;
        private bool _isRewardedShowing;

        public void Init()
        {
            GP_Ads.OnPreloaderClose += OnPreloaderClosed;
            GP_Ads.OnFullscreenClose += OnInterstitialClosed;
            GP_Ads.OnRewardedReward += OnRewardedReward;
            GP_Ads.OnRewardedClose += OnRewardedClose;
        }

        public void DeInit()
        {
            GP_Ads.OnPreloaderClose -= OnPreloaderClosed;
            GP_Ads.OnFullscreenClose -= OnInterstitialClosed;
            GP_Ads.OnRewardedReward -= OnRewardedReward;
            GP_Ads.OnRewardedClose -= OnRewardedClose;
        }

        public void ShowBanner()
        {
            GP_Ads.ShowSticky();
        }

        public void ShowPreloader(Action onPreloaderEnd)
        {
            if ( _isPreloaderShowing )
            {
                return;
            }
            
            _onPreloaderEnd = onPreloaderEnd;
            _isPreloaderShowing = true;

            GP_Ads.ShowPreloader();
        }

        public void ShowInterstitial(Action onInterstitialEnd)
        {
            if ( _isInterstitialShowing )
            {
                return;
            }
            
            _onInterstitialEnd = onInterstitialEnd;
            _isInterstitialShowing = true;

            GP_Ads.ShowFullscreen();
        }

        public void ShowRewarded(Action<bool> onRewardedEnd, Action<bool> onRewardedReward)
        {
            if ( _isRewardedShowing )
            {
                return;
            }
            
            _onRewardedEnd = onRewardedEnd;
            _isRewardedShowing = true;
            _isRewarded = false;

            GP_Ads.ShowRewarded();
        }

        private void OnPreloaderClosed(bool success)
        {
            if ( !_isPreloaderShowing )
            {
                return;
            }

            _isPreloaderShowing = false;
            _onPreloaderEnd?.Invoke();
        }

        private void OnInterstitialClosed(bool success)
        {
            if ( !_isInterstitialShowing )
            {
                return;
            }

            _isInterstitialShowing = false;
            _onInterstitialEnd?.Invoke();
        }

        private void OnRewardedReward(string tag)
        {
            if ( !_isRewardedShowing )
            {
                return;
            }

            _isRewarded = true;
            _onRewardedReward?.Invoke(_isRewarded);
        }

        private void OnRewardedClose(bool success)
        {
            if ( !_isRewardedShowing )
            {
                return;
            }

            _isRewardedShowing = false;
            _onRewardedEnd?.Invoke(success);
        }
    }
}
#endif