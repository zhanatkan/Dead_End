using Leopotam.Ecs;
using UnityEngine;
using Game.Scripts.Game.AIScripts.Common;
using Game.Scripts.Game.GameField;

namespace Game.Scripts.Game.AIScripts.Behaviour
{
    public class BotBehaviorSystem : IEcsRunSystem
    {
        private EcsFilter<BotComponent, BotStateComponent> _botFilter = null;
        private EcsFilter<PlayerTag> _playerFilter = null;
        
        private EcsFilter<MapComponent> _mapFilter = null; 

        public void Run()
        {
            float deltaTime = Time.deltaTime;
            Transform playerTransform = _playerFilter.IsEmpty() ? null : _playerFilter.Get1(0).Transform;

            if (_mapFilter.IsEmpty())
            {
                return;
            } 
            Map map = _mapFilter.Get1(0).MapInstance;

            foreach (var i in _botFilter)
            {
                ref var bot = ref _botFilter.Get1(i);
                ref var state = ref _botFilter.Get2(i);

                Vector3 botPos = bot.Transform.position;

                switch (state.CurrentState)
                {
                    case BotState.Patrol:
                        if (Vector3.Distance(botPos, state.TargetPosition) < 1.5f)
                        {
                            int randomZoneIndex = Random.Range(0, map.BotsSpawnRange.Count);
                            SpawnRange randomZone = map.BotsSpawnRange[randomZoneIndex];

                            state.TargetPosition = randomZone.SpawnPosition();
                        }
                        MoveTowards(bot, state.TargetPosition, bot.Setting.PatrolSpeed, deltaTime);
                        break;

                    case BotState.Chase:
                        if (playerTransform != null)
                        {
                            state.TargetPosition = playerTransform.position;
                        }
                        MoveTowards(bot, state.TargetPosition, bot.Setting.ChaseSpeed, deltaTime);
                        break;

                    case BotState.Alert:
                        state.Timer -= deltaTime;
                        
                        if (state.Timer <= 0f)
                        {
                            state.CurrentState = BotState.Patrol;
                            
                            int randomZoneIndex = Random.Range(0, map.BotsSpawnRange.Count);
                            state.TargetPosition = map.BotsSpawnRange[randomZoneIndex].SpawnPosition();
                        }
                        else
                        {
                            if (Vector3.Distance(botPos, state.TargetPosition) < 1.5f)
                            {
                                state.TargetPosition = botPos + Random.insideUnitSphere * 5f;
                                state.TargetPosition.y = botPos.y;
                            }
                            MoveTowards(bot, state.TargetPosition, bot.Setting.ChaseSpeed, deltaTime);
                        }
                        break;
                }
            }
        }

        private void MoveTowards(BotComponent bot, Vector3 target, float speed, float deltaTime)
        {
            if (bot.Rigidbody == null)
            {
                return;
            }

            Vector3 direction = (target - bot.Transform.position);
            direction.y = 0; 

            if (direction.magnitude > 0.1f)
            {
                Vector3 normDir = direction.normalized;
                Quaternion targetRotation = Quaternion.LookRotation(normDir);
                bot.Transform.rotation = Quaternion.Slerp(bot.Transform.rotation, targetRotation, bot.Setting.RotationSpeed * deltaTime);

                Vector3 newVelocity = normDir * speed;
                newVelocity.y = bot.Rigidbody.linearVelocity.y; 

                bot.Rigidbody.linearVelocity = newVelocity;
            }
            else
            {
                bot.Rigidbody.linearVelocity = new Vector3(0, bot.Rigidbody.linearVelocity.y, 0);
            }
        }
    }
}