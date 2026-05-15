using Game.Scripts.Settings.Sprites;

namespace Game.Scripts.Base.Services.SpriteSetup
{
    public interface ISpriteSetupProvider
    {
        T GetSpriteSetup<T>() where T : BaseSpriteSetup;
    }
}