using System;

namespace Game.Scripts.Base.Services.Leaderboard
{
    public interface ILeaderboardService
    {
        bool IsLeaderboardAvailable { get; }

        void Init();

        void Open(LeaderboardOptions leaderboardOptions);
        void Fetch(Action<LeaderboardData> onFetch);
    }
}