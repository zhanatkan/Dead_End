using Cysharp.Threading.Tasks;
using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Game.AIScripts.Common;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.GameplayControllers.Inventory;
using Game.Scripts.Game.Managers.GameField;
using Game.Scripts.Game.GameField;
using Game.Scripts.Settings.Inventory;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Base.Services.GameFactory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPauseService _pauseService;
        private readonly ISaveDataHandler _saveDataHandler;
        
        private MainGameField _gameField;
        private MiddleGameField _middleGameField;
        private EcsCreator _ecsCreator;
        private FirstPersonCamera _camera;
        private PlayerController _player;
        
        [Inject]
        public GameFactory(IAssetProvider assetProvider, IPauseService pauseService,
            ISaveDataHandler saveDataHandler)
        {
            _assetProvider = assetProvider;
            _pauseService = pauseService;
            _saveDataHandler = saveDataHandler;
        }

        public MainGameField CreateGameField()
        {
            if (_gameField)
            {
                return _gameField;
            }
            
            _gameField = InstantiateRegistered(AssetsPath.MainGameField).GetComponent<MainGameField>();
            return _gameField;
        }

        public MiddleGameField CreateMiddleGameField()
        {
            if ( _middleGameField )
            {
                return _middleGameField;
            }
            _middleGameField = InstantiateRegistered(AssetsPath.MiddleGameField).GetComponent<MiddleGameField>();
            return _middleGameField;
        }
        
        public EcsCreator CreateEcsCreator()
        {
            if (_ecsCreator)
            {
                return _ecsCreator;
            }
            
            _ecsCreator = InstantiateRegistered(AssetsPath.EcsCreator).GetComponent<EcsCreator>();
            return _ecsCreator;
        }

        public FirstPersonCamera CreateCamera(Transform parent)
        {
            return _camera ? _camera : InstantiateRegistered(AssetsPath.Camera, parent).GetComponent<FirstPersonCamera>();
        }

        public PlayerController CreatePlayer()
        {
            return _player ? _player : InstantiateRegistered(AssetsPath.Player).GetComponentInChildren<PlayerController>();
        }
        
        public Transform CreateWorldCanvas()
        {
            return InstantiateRegistered(AssetsPath.WorldCanvas).transform;
        }

        public async UniTask<Map> CreateMap(string mapName)
        {
            var mapPath = string.Format(AssetsPath.MapsFormat, mapName);
            var mapPrefab = await _assetProvider.LoadAsync<GameObject>(mapPath);
            
            return Object.Instantiate(mapPrefab).GetComponent<Map>();
        }
        
        public ItemPickup CreateItemPickup(ItemType itemType) =>
            InstantiateRegistered(AssetsPath.Items + itemType).GetComponent<ItemPickup>();
        
        private GameObject InstantiateRegistered(string address, Transform parent, Vector3 at)
        {
            var gameObject = _assetProvider.Instantiate(address, parent, at);

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