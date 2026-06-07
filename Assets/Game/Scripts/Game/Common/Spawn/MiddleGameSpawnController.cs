using Game.Scripts.Base.Services.GameFactory;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Managers.MiddleManager;
using Game.Scripts.Settings.Inventory;
using VContainer;

namespace Game.Scripts.Game.Common.Spawn
{
    public class MiddleGameSpawnController
    {
        private readonly PlayerController _player;
        private readonly MiddleGameField _middleGameField;
        //Temporary params
        private readonly IGameFactory _gameFactory;
        private readonly ISettingsProvider _settingsProvider;

        [Inject]
        public MiddleGameSpawnController(PlayerController player,
            MiddleGameField middleGameField, IGameFactory gameFactory, ISettingsProvider settingsProvider)
        {
            _player = player;
            _middleGameField = middleGameField;
            _gameFactory = gameFactory;
            _settingsProvider = settingsProvider;
        }

        public void StartGameplay()
        {
            _player.transform.position = _middleGameField.PlayerSpawnRange.SpawnPosition();
            var item1 = _gameFactory.CreateItemPickup(ItemType.Heal);
            item1.transform.position = _middleGameField.PlayerSpawnRange.SpawnPosition();
            var item2 = _gameFactory.CreateItemPickup(ItemType.Heal);
            item2.transform.position = _middleGameField.PlayerSpawnRange.SpawnPosition();
        }
    }
}