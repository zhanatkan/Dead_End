namespace Game.Scripts.Base.Services.PlatformInfo
{
    public interface IPlatformInfoProvider
    {
        PlatformType GetPlatformId();
        Language GetLanguage();
        DeviceType GetDeviceType();
        bool IsMoreGamesSupported();
        void ShowMoreGames();
        string GetPlayerName();
        int GetPlayerId();
    }
}