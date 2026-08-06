using System.Collections.Generic;
using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.UIScripts.Game;
using Game.Scripts.UIScripts.MiddleGame;
using Game.Scripts.UIScripts.MainMenu;
using Game.Scripts.UIScripts.Windows;
using UnityEngine;
using VContainer;
namespace Game.Scripts.Base.Services.UIFactory
{
    public sealed class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPauseService _pauseService;

        private Transform _windowsRoot;
        
        [Inject]
        public UIFactory(IAssetProvider assetProvider, IPauseService pauseService)
        {
            _assetProvider = assetProvider;
            _pauseService = pauseService;
        }
        
        public void CreateWindowsRoot()
        {
            _windowsRoot = InstantiateRegistered(AssetsPath.WindowCanvas).transform;
            Object.DontDestroyOnLoad(_windowsRoot);
        }

        public WindowBackground CreateWindowBackground() =>
            InstantiateRegistered(AssetsPath.WindowBackground, _windowsRoot).GetComponent<WindowBackground>();

        public List<BaseWindow> SetupWindows(List<GameObject> windowObjects)
        {
            var result = new List<BaseWindow>();
            
            foreach (var windowObject in windowObjects)
            {
                var window = Object.Instantiate(windowObject, _windowsRoot).GetComponent<BaseWindow>();
                Register(window.gameObject);
                window.Init();
                window.gameObject.SetActive(false);
                result.Add(window);
            }

            return result;
        }

        public Transform CreateUICanvasRoot() =>
            InstantiateRegistered(AssetsPath.UICanvas).transform;

        public MiddleGameUI CreateMiddleGameUI(Transform parent) =>
            InstantiateRegistered(AssetsPath.MiddleGameUI, parent).GetComponent<MiddleGameUI>();
        
        public GameUI CreateGameUI(Transform parent) =>
            InstantiateRegistered(AssetsPath.GameUI, parent).GetComponent<GameUI>();

        public MainMenuUI CreateMainMenuUI(Transform parent) =>
            InstantiateRegistered(AssetsPath.MainMenuUI, parent).GetComponent<MainMenuUI>();

        private GameObject InstantiateRegistered(string address, Vector3 at)
        {
            var gameObject = _assetProvider.Instantiate(address, at);

            Register(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string address, Transform parent)
        {
            var gameObject = _assetProvider.Instantiate(address, parent);

            Register(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string address)
        {
            var gameObject = _assetProvider.Instantiate(address);

            Register(gameObject);
            return gameObject;
        }

        private void Register(GameObject gameObject)
        {
            RegisterPauseHandlers(gameObject);
        }

        private void RegisterPauseHandlers(GameObject gameObject)
        {
            foreach (var pauseHandler in gameObject.GetComponentsInChildren<IPauseHandler>())
            {
                _pauseService.Register(pauseHandler);
            }
        }
    }
}