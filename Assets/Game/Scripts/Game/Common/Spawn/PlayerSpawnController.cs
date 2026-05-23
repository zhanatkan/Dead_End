using Game.Scripts.Game.Character.Player;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.Common.Spawn
{
    public class PlayerSpawnController
    {
        private readonly PlayerController _playerController;

        [Inject]
        public PlayerSpawnController(PlayerController playerController)
        {
            
        }
    }
}