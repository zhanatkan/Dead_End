using Cysharp.Threading.Tasks;
using Game.Scripts.Base.Services.GameFactory;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Data;
using Game.Scripts.Game.AIScripts.Common;
using Game.Scripts.Game.AIScripts.Spawn;
using Game.Scripts.Game.GameField;
using Game.Scripts.UIScripts.Windows.LevelChoice;
using Leopotam.Ecs;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.Managers.GameField
{
    public class MainGameField : MonoBehaviour, ISaveReader
    {
        private IGameFactory _gameFactory;
        private EcsCreator _ecsCreator;

        private LevelName _currentLevel;
        public Map Map { get; private set; }
        
        [Inject]
        public void Construct(IGameFactory gameFactory, EcsCreator ecsCreator)
        {
            _gameFactory = gameFactory;
            _ecsCreator = ecsCreator;
        }

        public async UniTask LoadMap()
        {
            Map = await _gameFactory.CreateMap(_currentLevel.ToString());
            
            var eventEntity = _ecsCreator.EcsWorld.NewEntity();
            eventEntity.Get<MapLoadedEvent>().MapInstance = Map;
        }

        public void ReadSave(SaveData saveData)
        {
            _currentLevel = saveData.PlayerSaveData.LevelName;
        }
    }
}