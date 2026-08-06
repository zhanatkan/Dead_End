using System;
using System.Collections.Generic;

namespace Game.Scripts.Base.Services.Leaderboard
{
    [Serializable]
    public class LeaderboardData
    {
        public readonly int PlayerPosition;
        public readonly List<LeaderboardEntry> Players;

        public LeaderboardData(int playerPosition, List<LeaderboardEntry> players)
        {
            PlayerPosition = playerPosition;
            Players = players;
        }
    }
}