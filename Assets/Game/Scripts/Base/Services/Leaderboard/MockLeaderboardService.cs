using System;
using System.Collections.Generic;

namespace Game.Scripts.Base.Services.Leaderboard
{
    public sealed class MockLeaderboardService : ILeaderboardService
    {
        private readonly List<LeaderboardEntry> _players = new()
        {
            new LeaderboardEntry
            {
                name = "TimOn",
                position = 1,
                score = 10,
            },
            new LeaderboardEntry
            {
                name = "**Fess**",
                position = 2,
                score = 9,
            },
            new LeaderboardEntry
            {
                name = "serverSTEAM",
                position = 3,
                score = 8,
            },
            new LeaderboardEntry
            {
                name = "Niceboy",
                position = 4,
                score = 7,
            },
            new LeaderboardEntry
            {
                name = "Sheff816",
                position = 5,
                score = 6,
            },
            new LeaderboardEntry
            {
                name = "TASSAY.TTK",
                position = 6,
                score = 5,
            },
            new LeaderboardEntry
            {
                name = "Ba3|2109",
                position = 7,
                score = 4,
            },
            new LeaderboardEntry
            {
                name = "xuligan",
                position = 8,
                score = 3,
            },
            new LeaderboardEntry
            {
                name = "Gogogo",
                position = 9,
                score = 2,
            },
            new LeaderboardEntry
            {
                name = "Aboba boba",
                position = 10,
                score = 1,
            },
        };

        private readonly int _playerPosition = 15;

        public bool IsLeaderboardAvailable => true;

        public void Init()
        {
        }

        public void Open(LeaderboardOptions leaderboardOptions)
        {
        }

        public void Fetch(Action<LeaderboardData> onFetch)
        {
            var leaderboardData = new LeaderboardData(_playerPosition, _players);
            onFetch?.Invoke(leaderboardData);
        }
    }
}