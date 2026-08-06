using System;
using System.Collections;
using Game.Scripts.Base.Services.Audio;
using UnityEngine;

namespace Game.Scripts.Base.Services.Advertisement
{
    public class MockAdvertisementService : IAdvertisementService
    {
        readonly IAudioService _audioService;
        //readonly AdsLoadingScreen _adsLoadingScreen;
        readonly ICoroutineRunner _coroutineRunner;

        public bool IsBannerSupported => true;
        public bool IsBannerShowing => true;
        public bool IsRewardedAvailable => true;

        public MockAdvertisementService(IAudioService audioService, /*AdsLoadingScreen adsLoadingScreen,*/
            ICoroutineRunner coroutineRunner)
        {
            _audioService = audioService;
            //_adsLoadingScreen = adsLoadingScreen;
            _coroutineRunner = coroutineRunner;
        }

        public void Init()
        {
        }

        public void DeInit()
        {
        }

        public void ShowBanner()
        {
        }

        public void ShowPreloader(Action onPreloaderEnd)
        {
            //_adsLoadingScreen.Show();
            _audioService.PauseAudio(true);

            _coroutineRunner.StartCoroutine(MockAdTimer(() =>
            {
                //_adsLoadingScreen.Hide();
                _audioService.PauseAudio(false);

                onPreloaderEnd?.Invoke();
            }));
        }

        public void ShowInterstitial(Action onInterstitialEnd)
        {
            //_adsLoadingScreen.Show();
            _audioService.PauseAudio(true);

            _coroutineRunner.StartCoroutine(MockAdTimer(() =>
            {
                //_adsLoadingScreen.Hide();
                _audioService.PauseAudio(false);

                onInterstitialEnd?.Invoke();
            }));
        }

        public void ShowRewarded(Action<bool> onRewardedEnd, Action<bool> onRewardedReward)
        {
            //_adsLoadingScreen.Show();
            _audioService.PauseAudio(true);

            _coroutineRunner.StartCoroutine(MockAdTimer(() =>
            {
                //_adsLoadingScreen.Hide();
                _audioService.PauseAudio(false);

                onRewardedEnd?.Invoke(true);
                onRewardedReward?.Invoke(true);
            }));
        }

        private IEnumerator MockAdTimer(Action onComplete)
        {
            yield return new WaitForSeconds(2f);
            onComplete?.Invoke();
        }
    }
}