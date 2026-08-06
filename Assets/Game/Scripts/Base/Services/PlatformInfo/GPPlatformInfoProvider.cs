#if UNITY_WEBGL && GAME_PUSH
using System.Linq;
using GamePush;

namespace Game.Scripts.Base.Services.PlatformInfo
{
    public sealed class GPPlatformInfoProvider : IPlatformInfoProvider
    {
        private const string MORE_GAMES_TAG = "more_games";

        private readonly PlatformType[] _moreGamesSupportedPlatformTypes =
        {
            PlatformType.YANDEX, PlatformType.VK, PlatformType.VK_PLAY, PlatformType.OK, PlatformType.CRAZY_GAMES,
            PlatformType.GAME_MONETIZE, PlatformType.KONGREGATE, PlatformType.PLAYDECK, PlatformType.GOOGLE_PLAY,
            PlatformType.None
        };

        public PlatformType GetPlatformId()
        {
            return (PlatformType)GP_Platform.Type();
        }

        public Language GetLanguage() => (Language)GP_Language.Current();

        public DeviceType GetDeviceType() =>
            GP_Device.IsMobile() ? DeviceType.Mobile : DeviceType.Desktop;

        public bool IsMoreGamesSupported() => _moreGamesSupportedPlatformTypes.Contains(GetPlatformId());

        public void ShowMoreGames() => GP_GamesCollections.Open(MORE_GAMES_TAG);

        public string GetPlayerName() => GP_Player.GetName();
        public int GetPlayerId() => GP_Player.GetID();
    }
}
#endif