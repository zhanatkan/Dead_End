#if UNITY_WEBGL && GAME_PUSH
using System.Linq;
using Game.Scripts.Base.Services.PlatformInfo;
using GamePush;
using UnityEngine;

namespace Game.Scripts.Base.Services.Analytics
{
    public sealed class GPAnalyticsService : IAnalyticsService
    {
        readonly PlatformType _platformType;
        readonly PlatformType[] _supportedPlatforms = new[]
        {
            PlatformType.OK, PlatformType.VK, PlatformType.SMARTMARKET, PlatformType.YANDEX, PlatformType.VK_PLAY,
            PlatformType.PLAYDECK, PlatformType.GOOGLE_PLAY, PlatformType.None,
        };

        public GPAnalyticsService(IPlatformInfoProvider platformInfoProvider)
        {
            _platformType = platformInfoProvider.GetPlatformId();
        }

        public void SendDesignEvent(string eventName)
        {
            if ( !_supportedPlatforms.Contains(_platformType) )
            {
                return;
            }

            Debug.Log($"send event - {eventName}");
            GP_Analytics.Goal(eventName, string.Empty);
        }

        public void SendDesignEvent(string eventName, int eventValue)
        {
            if ( !_supportedPlatforms.Contains(_platformType) )
            {
                return;
            }

            Debug.Log($"send event - {eventName}_{eventValue}");
            GP_Analytics.Goal(eventName, eventValue);
        }

        public void SendDesignEvent(string eventName, string eventValue)
        {
            if ( !_supportedPlatforms.Contains(_platformType) )
            {
                return;
            }

            Debug.Log($"send event - {eventName}_{eventValue}");
            GP_Analytics.Goal(eventName, eventValue);
        }
    }
}
#endif