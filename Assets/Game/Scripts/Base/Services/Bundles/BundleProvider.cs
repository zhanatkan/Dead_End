using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Settings.Sprites;
using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.Base.Services.UIFactory;
using Game.Scripts.UIScripts.Windows;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Base.Services.Bundles
{
    public class BundleCache
    {
        public List<BaseWindow> Windows = new();
        public List<BaseSpriteSetup> SpriteSetups = new();
    }

    public class BundleProvider : IBundleProvider
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IUIFactory _uiFactory;

        private readonly Dictionary<string, BundleCache> _bundleCaches = new();

        [Inject]
        public BundleProvider(IAssetProvider assetProvider, IUIFactory uiFactory)
        {
            _assetProvider = assetProvider;
            _uiFactory = uiFactory;
        }

        public async UniTask LoadBundle(string bundleName, bool saveInCache = true)
        {
            List<BaseSpriteSetup> spriteSetups;

            if ( saveInCache )
            {
                spriteSetups =
                    await _assetProvider.LoadAllAsync<BaseSpriteSetup>($"{bundleName}/{AssetsPath.SpriteSetups}");
            }
            else
            {
                spriteSetups =
                    await _assetProvider.LoadAllAsyncDontCache<BaseSpriteSetup>($"{bundleName}/{AssetsPath.SpriteSetups}");
            }

            List<GameObject> windowObjects;
            if ( saveInCache )
            {
                windowObjects = await _assetProvider.LoadAllAsync<GameObject>($"{bundleName}/{AssetsPath.Windows}");
            }
            else
            {
                windowObjects = await _assetProvider.LoadAllAsyncDontCache<GameObject>($"{bundleName}/{AssetsPath.Windows}");
            }

            var windows = _uiFactory.SetupWindows(windowObjects);

            var bundle = new BundleCache()
            {
                Windows = windows,
                SpriteSetups = spriteSetups,
            };

            _bundleCaches.Add(bundleName, bundle);
        }

        public void ReleaseBundle(string bundleName)
        {
            if ( !_bundleCaches.TryGetValue(bundleName, out var bundleCache) )
            {
                Debug.LogError($"Bundle {bundleName} is not loaded ");
                return;
            }

            foreach (var window in bundleCache.Windows)
            {
                Object.Destroy(window.gameObject);
            }

            _bundleCaches.Remove(bundleName);
        }

        public BaseWindow GetWindowFromBundle<T>() where T : BaseWindow
        {
            var bundleName = BundlesInfo.GetBundleName(typeof(T));
            if ( string.IsNullOrEmpty(bundleName) )
            {
                Debug.LogError($"There are no bundle which contains {typeof(T)} window type");
                return null;
            }

            if ( _bundleCaches.TryGetValue(bundleName, out var bundleCache) )
            {
                return GetWindow<T>(bundleCache.Windows);
            }
            
            Debug.LogError($"Bundle for {typeof(T)} is not loaded ");
            return null;
        }

        public BaseSpriteSetup GetSpriteSetupFromBundle<T>() where T : BaseSpriteSetup
        {
            var bundleName = BundlesInfo.GetBundleName(typeof(T));
            if ( string.IsNullOrEmpty(bundleName) )
            {
                Debug.LogError($"There are no bundle which contains {typeof(T)} sprite setup type");
                return null;
            }

            if ( _bundleCaches.TryGetValue(bundleName, out var bundleCache) )
            {
                return GetSpriteSetup<T>(bundleCache.SpriteSetups);
            }

            Debug.LogError($"Bundle for {typeof(T)} is not loaded ");
            return null;
        }

        [CanBeNull]
        private BaseWindow GetWindow<T>(List<BaseWindow> windows) where T : BaseWindow
        {
            foreach (var window in windows)
            {
                if ( window is T tWindow )
                {
                    return tWindow;
                }
            }

            return null;
        }

        [CanBeNull]
        private BaseSpriteSetup GetSpriteSetup<T>(List<BaseSpriteSetup> spriteSetups) where T : BaseSpriteSetup
        {
            foreach (var spriteSetup in spriteSetups)
            {
                if ( spriteSetup is T tSpriteSetup )
                {
                    return tSpriteSetup;
                }
            }

            return null;
        }
    }
}