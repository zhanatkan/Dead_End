using Game.Scripts.Base.Services.PlatformInfo;
using UnityEngine;

namespace Game.Scripts.Base.Services.Analytics
{
    public sealed class MockAnalyticsService : IAnalyticsService
    {
        public MockAnalyticsService(IPlatformInfoProvider platformInfoProvider)
        {
            var platform = platformInfoProvider.GetPlatformId();
            Debug.Log($"Platform: {platform}");
        }

        public void SendDesignEvent(string eventName)
        {
            Debug.Log($"SendDesign event: {eventName}");
        }

        public void SendDesignEvent(string eventName, int eventValue)
        {
            Debug.Log($"SendDesign event: {eventName}_{eventValue}");
        }

        public void SendDesignEvent(string eventName, string eventValue)
        {
            Debug.Log($"SendDesign event: {eventName}_{eventValue}");
        }
    }
}