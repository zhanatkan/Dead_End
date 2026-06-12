namespace Game.Scripts.Base.Services.Analytics
{
    public interface IAnalyticsService
    {
        void SendDesignEvent(string eventName);
        void SendDesignEvent(string eventName, int eventValue);
        void SendDesignEvent(string eventName, string eventValue);
    }
}