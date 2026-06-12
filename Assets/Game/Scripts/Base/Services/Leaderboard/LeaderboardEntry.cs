using System;

namespace Game.Scripts.Base.Services.Leaderboard
{
    [Serializable]
    public class LeaderboardEntry
    {
        public string avatar;
        public int id;
        public int score;
        public string name;
        public int position;
    }
}