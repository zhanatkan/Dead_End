using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Base.Services.UIFactory;
using Game.Scripts.UIScripts.Windows;
using VContainer;

namespace Game.Scripts.Base.Services.WindowHolder
{
    public sealed class WindowHolder : IWindowHolder
    {
        private readonly IUIFactory _uiFactory;
        private readonly IBundleProvider _bundleProvider;

        private WindowBackground _windowBackground;

        [Inject]
        public WindowHolder(IUIFactory uiFactory, IBundleProvider bundleProvider)
        {
            _uiFactory = uiFactory;
            _bundleProvider = bundleProvider;
        }

        public void CreateWindowsRoot()
        {
            _uiFactory.CreateWindowsRoot();
        }

        public WindowBackground GetWindowBackground()
        {
            if ( _windowBackground )
            {
                return _windowBackground;
            }

            _windowBackground = _uiFactory.CreateWindowBackground();
            return _windowBackground;
        }

        public T GetWindow<T>() where T : BaseWindow
        {
            return _bundleProvider.GetWindowFromBundle<T>() as T;
        }
    }
}