using Cysharp.Threading.Tasks;
using Game.Scripts.Settings.Sprites;
using Game.Scripts.UIScripts.Windows;

namespace Game.Scripts.Base.Services.Bundles
{
    public interface IBundleProvider
    {
        UniTask LoadBundle(string bundleName, bool saveInCache = true);
        void ReleaseBundle(string bundleName);
        BaseWindow GetWindowFromBundle<T>() where T : BaseWindow;
        BaseSpriteSetup GetSpriteSetupFromBundle<T>() where T : BaseSpriteSetup;
    }
}