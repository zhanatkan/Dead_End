using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Settings.Sprites;
using VContainer;

namespace Game.Scripts.Base.Services.SpriteSetup
{
    public class SpriteSetupProvider : ISpriteSetupProvider
    {
        private readonly IBundleProvider _bundleProvider;

        [Inject]
        public SpriteSetupProvider(IBundleProvider bundleProvider)
        {
            _bundleProvider = bundleProvider;
        }

        public T GetSpriteSetup<T>() where T : BaseSpriteSetup
        {
            return _bundleProvider.GetSpriteSetupFromBundle<T>() as T;
        }
    }
}