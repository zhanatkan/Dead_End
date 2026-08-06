using Leopotam.Ecs;
using UnityEngine;
using Game.Scripts.Game.AIScripts.Common;

namespace Game.Scripts.Game.AIScripts.Behaviour
{
    public class BotHearingSystem : IEcsRunSystem
    {
        private EcsFilter<BotComponent, BotStateComponent> _botFilter = null;
        private EcsFilter<PlayerTag, NoiseEmitterComponent> _playerFilter = null;

        public void Run()
        {
            if (_playerFilter.IsEmpty()) return;

            ref var playerTag = ref _playerFilter.Get1(0);
            ref var playerNoise = ref _playerFilter.Get2(0);

            if (playerNoise.NoiseRadius <= 0f)
            {
                return;
            }

            Vector3 playerPos = playerTag.Transform.position;

            foreach (var i in _botFilter)
            {
                ref var bot = ref _botFilter.Get1(i);
                ref var state = ref _botFilter.Get2(i);

                if (state.CurrentState == BotState.Chase)
                {
                    continue;
                }

                Vector3 botPos = bot.Transform.position;
                float distanceToPlayer = Vector3.Distance(botPos, playerPos);

                float botHearingRadius = bot.Setting.BaseHearingRadius;
                if (state.CurrentState == BotState.Alert)
                {
                    botHearingRadius *= bot.Setting.AlertHearingMultiplier;
                }

                float effectiveNoiseRadius = playerNoise.NoiseRadius;

                Vector3 dirToPlayer = (playerPos - botPos).normalized;
                if (Physics.Raycast(botPos + Vector3.up * 1f, dirToPlayer, distanceToPlayer, bot.Setting.VisionMask))
                {
                    effectiveNoiseRadius *= (1f - bot.Setting.WallNoiseOcclusion);
                }

                if (distanceToPlayer <= (botHearingRadius + effectiveNoiseRadius))
                {
                    state.CurrentState = BotState.Alert;
                    state.Timer = Random.Range(bot.Setting.MinAlertTime, bot.Setting.MaxAlertTime);
                    
                    state.TargetPosition = playerPos;
                }
            }
        }
    }
}