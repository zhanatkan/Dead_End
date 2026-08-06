using Leopotam.Ecs;
using UnityEngine;
using Game.Scripts.Game.AIScripts.Common;
using Game.Scripts.Game.Character.Player;

namespace Game.Scripts.Game.AIScripts.Player
{
    public class PlayerNoiseSystem : IEcsRunSystem
    {
        private EcsFilter<PlayerTag, NoiseEmitterComponent> _playerFilter = null;
        private readonly PlayerController _playerController;

        public PlayerNoiseSystem(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Run()
        {
            foreach (var i in _playerFilter)
            {
                ref var noise = ref _playerFilter.Get2(i);
                noise.NoiseRadius = _playerController.GetCurrentNoiseRadius();
            }
        }
    }
}