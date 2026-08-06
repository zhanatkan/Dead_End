using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Managers.GameField;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.Common.Spawn
{
    public class PlayerSpawnController
    {
        private readonly PlayerController _player;
        private readonly MainGameField _gameField;

        [Inject]
        public PlayerSpawnController(PlayerController player, MainGameField gameField)
        {
            _player = player;
            _gameField = gameField;
        }

        public void StartGameplay()
        {
            _player.transform.position = _gameField.Map.PlayerSpawnRange.SpawnPosition();
        }
    }
}