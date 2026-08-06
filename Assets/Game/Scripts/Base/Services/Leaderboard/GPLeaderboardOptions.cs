#if UNITY_WEBGL && GAME_PUSH
using GamePush;

namespace Game.Scripts.Base.Services.Leaderboard
{
    public class GPLeaderboardOptions : LeaderboardOptions
    {
        public string OrderBy;
        public Order Order;
        public int Limit;
        public int ShowNearest;
        public WithMe WithMe;
        public string IncludeFields;
        public string DisplayFields;
        
        public GPLeaderboardOptions(string orderBy, Order order, int limit, int showNearest, WithMe withMe, string includeFields,
            string displayFields)
        {
            OrderBy = orderBy;
            Order = order;
            Limit = limit;
            ShowNearest = showNearest;
            WithMe = withMe;
            IncludeFields = includeFields;
            DisplayFields = displayFields;
        }
    }
}
#endif