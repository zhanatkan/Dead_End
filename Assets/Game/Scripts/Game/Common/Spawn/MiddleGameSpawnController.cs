using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Managers.MiddleManager;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.Common.Spawn
{
    public class MiddleGameSpawnController
    {
        private readonly PlayerController _player;
        private readonly MiddleGameField _middleGameField;

        [Inject]
        public MiddleGameSpawnController(PlayerController player,
            MiddleGameField middleGameField)
        {
            _player = player;
            _middleGameField = middleGameField;
        }

        public void StartGameplay()
        {
            _player.transform.position = _middleGameField.PlayerSpawnRange.SpawnPosition();
        }
    }
}