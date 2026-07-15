using Leopotam.Ecs;
using UnityEngine;
using Game.Scripts.Game.AIScripts.Common;

namespace Game.Scripts.Game.AIScripts.Behaviour
{
    public class BotVisionSystem : IEcsRunSystem
    {
        private EcsFilter<BotComponent, BotStateComponent> _botFilter = null;
        private EcsFilter<PlayerTag> _playerFilter = null;

        public void Run()
        {
            if (_playerFilter.IsEmpty())
            {
                return;
            }
            
            Transform playerTransform = _playerFilter.Get1(0).Transform;
            Vector3 playerPos = playerTransform.position;

            foreach (var i in _botFilter)
            {
                ref var bot = ref _botFilter.Get1(i);
                ref var state = ref _botFilter.Get2(i);

                Vector3 botPos = bot.Transform.position;
                Vector3 dirToPlayer = (playerPos - botPos);
                float distanceToPlayer = dirToPlayer.magnitude;

                bool canSeePlayer = false;

                if (distanceToPlayer <= bot.Setting.VisionRadius)
                {
                    float angle = Vector3.Angle(bot.Transform.forward, dirToPlayer.normalized);
                    if (angle <= bot.Setting.VisionAngle * 0.5f)
                    {
                        if (Physics.Raycast(botPos + Vector3.up * 1.5f, dirToPlayer.normalized, out RaycastHit hit, bot.Setting.VisionRadius, bot.Setting.VisionMask))
                        {
                            if (hit.transform == playerTransform)
                            {
                                canSeePlayer = true;
                            }
                        }
                    }
                }

                if (canSeePlayer)
                {
                    state.CurrentState = BotState.Chase;
                }
                else if (state.CurrentState == BotState.Chase)
                {
                    state.CurrentState = BotState.Alert;
                    state.Timer = Random.Range(bot.Setting.MinAlertTime, bot.Setting.MaxAlertTime);
                    state.TargetPosition = playerPos + Random.insideUnitSphere * 3f;
                    state.TargetPosition.y = botPos.y;
                }
            }
        }
    }
}