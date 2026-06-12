using UnityEngine;

namespace Game.Scripts.Base.Services.PlatformInfo
{
    public class MockPlatformInfoProvider : IPlatformInfoProvider
    {
        public PlatformType GetPlatformId() => PlatformType.None;
        public Language GetLanguage() => Language.English;

        public DeviceType GetDeviceType()
        {
#if UNITY_EDITOR
            return DeviceType.Desktop;
#elif UNITY_ANDROID
            return DeviceType.Mobile;
#else
            return DeviceType.Desktop;
#endif
        }
        
        public bool IsMoreGamesSupported() => true;
        public void ShowMoreGames() => Debug.Log("Show more games");
        public string GetPlayerName() => string.Empty;
        public int GetPlayerId() => 870438573;
    }
}