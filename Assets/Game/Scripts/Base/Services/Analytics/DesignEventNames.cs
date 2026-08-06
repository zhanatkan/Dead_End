namespace Game.Scripts.Base.Services.Analytics
{
    public static class DesignEventNames
    {
        public const string InterstitialAd = "interstitial_ad";
        public const string RewardedStart = "rewarded_start";
        public const string RewardedReward = "rewarded_reward";
        public const string RewardedFinish = "rewarded_finish";
        public const string Buy = "buy";
        public const string BuyFailed = "buy_failed";

        //FTUE
        public const string FirstLaunchFTUE = "first_game_launch";
        public const string TutorialStep = "tutorial_step";
        public const string TutorialStart = "tutorial_start";
        public const string TutorialFinish = "tutorial_finish";

        public const string StartGame = "start_game";
        
        public const string WatchSoftAd = "watch_soft_ad";
        public const string TakeDailyReward = "take_daily_reward";
        public const string TakeDailyQuestReward = "take_daily_quest_reward";
        public const string TakePlaytimeGift = "take_playtime_gift";
    }
}