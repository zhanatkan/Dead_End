#if UNITY_WEBGL && GAME_PUSH
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Base.Services.PlatformInfo;
using GamePush;
using UnityEngine;

namespace Game.Scripts.Base.Services.Leaderboard
{
    public sealed class GPLeaderboardService : ILeaderboardService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPlatformInfoProvider _platformInfoProvider;
        
        private readonly PlatformType[] _unsupportedPlatforms = { PlatformType.GAME_DISTRIBUTION };

        private List<LeaderboardEntry> _players;
        private bool _isPlayersFetched, _isPlayerRatingFetched;
        private int _position;

        public bool IsLeaderboardAvailable => !_unsupportedPlatforms.Contains(_platformInfoProvider.GetPlatformId());
        
        public GPLeaderboardService(ICoroutineRunner coroutineRunner, IPlatformInfoProvider platformInfoProvider)
        {
            _coroutineRunner = coroutineRunner;
            _platformInfoProvider = platformInfoProvider;
        }

        public void Init()
        {
            GP_Leaderboard.OnFetchPlayerRatingSuccess += OnFetchPlayerRatingSuccess;
            GP_Leaderboard.OnFetchSuccess += OnFetchSuccess;
        }

        public void Open(LeaderboardOptions leaderboardOptions)
        {
            try
            {
                GPLeaderboardOptions gpOpts = (GPLeaderboardOptions)leaderboardOptions;
                GP_Leaderboard.Open(gpOpts.OrderBy, gpOpts.Order, gpOpts.Limit, gpOpts.ShowNearest,
                    gpOpts.WithMe, gpOpts.IncludeFields, gpOpts.DisplayFields);

            }
            catch ( InvalidCastException e )
            {
                Debug.LogError(e);
            }
        }

        public void Fetch(Action<LeaderboardData> onFetch)
        {
            _coroutineRunner.StartCoroutine(FetchRoutine(onFetch));
        }

        private IEnumerator FetchRoutine(Action<LeaderboardData> onFetch)
        {
            GP_Leaderboard.Fetch(showNearest: 0);
            GP_Leaderboard.FetchPlayerRating();

            yield return new WaitUntil(() => _isPlayerRatingFetched && _isPlayersFetched);

            var leaderboardData = new LeaderboardData(_position, _players);
            onFetch?.Invoke(leaderboardData);
        }

        private void OnFetchSuccess(string fetchTag, GP_Data data)
        {
            _players = data.GetList<LeaderboardEntry>();
            foreach (var player in _players)
            {
                if ( string.IsNullOrEmpty(player.name) )
                {
                    player.name = "Player";
                }
            }
            
            _isPlayersFetched = true;
        }

        private void OnFetchPlayerRatingSuccess(string fetchTag, int position)
        {
            _position = position;
            _isPlayerRatingFetched = true;
        }
    }
}
#endif