using Cysharp.Threading.Tasks;
using Game.Scripts.Base.Services.GameFactory;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Data;
using Game.Scripts.Game.GameField;
using Game.Scripts.UIScripts.Windows.LevelChoice;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.Managers.GameField
{
    public class MainGameField : MonoBehaviour, ISaveReader
    {
        private IGameFactory _gameFactory;

        private LevelName _currentLevel;
        public Map Map { get; private set; }
        
        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public async UniTask LoadMap()
        {
            Map = await _gameFactory.CreateMap(_currentLevel.ToString());
        }

        public void ReadSave(SaveData saveData)
        {
            _currentLevel = saveData.PlayerSaveData.LevelName;
        }
    }
}