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
                float activeChaseSpeed = bot.Setting.CanRun ? bot.Setting.ChaseSpeed : bot.Setting.PatrolSpeed;

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
                            float distanceToPlayer = Vector3.Distance(botPos, playerTransform.position);

                            if (distanceToPlayer <= bot.Setting.AttackRange)
                            {
                                state.CurrentState = BotState.Attack;
                                break;
                            }
                        }
                        MoveTowards(bot, state.TargetPosition, activeChaseSpeed, deltaTime);
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
                            MoveTowards(bot, state.TargetPosition, activeChaseSpeed, deltaTime);
                        }
                        break;

                    case BotState.Attack:
                        if (playerTransform == null)
                        {
                            state.CurrentState = BotState.Patrol;
                            break;
                        }

                        float distToPlayer = Vector3.Distance(botPos, playerTransform.position);

                        if (distToPlayer > bot.Setting.AttackRange * 1.2f)
                        {
                            state.CurrentState = BotState.Chase;
                            break;
                        }

                        RotateTowards(bot, playerTransform.position, deltaTime);

                        if (bot.Setting.AttackType == Settings.AttackType.Melee || bot.Setting.AttackType == Settings.AttackType.SelfDestruct)
                        {
                            MoveTowards(bot, playerTransform.position, bot.Setting.PatrolSpeed, deltaTime);
                        }
                        else
                        {
                            StopMovement(bot);
                        }
                        break;
                }
            }
        }

        private void MoveTowards(BotComponent bot, Vector3 target, float speed, float deltaTime)
        {
            Vector3 direction = (target - bot.Transform.position);
            direction.y = 0; 

            if (direction.magnitude > 0.1f)
            {
                RotateTowards(bot, target, deltaTime);

                Vector3 newVelocity = direction.normalized * speed;
                newVelocity.y = bot.Rigidbody.linearVelocity.y; 
                bot.Rigidbody.linearVelocity = newVelocity;
            }
            else
            {
                StopMovement(bot);
            }
        }

        private void RotateTowards(BotComponent bot, Vector3 target, float deltaTime)
        {
            Vector3 direction = (target - bot.Transform.position);
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                bot.Transform.rotation = Quaternion.Slerp(bot.Transform.rotation, targetRotation, bot.Setting.RotationSpeed * deltaTime);
            }
        }

        private void StopMovement(BotComponent bot)
        {
            if (bot.Rigidbody != null)
            {
                bot.Rigidbody.linearVelocity = new Vector3(0, bot.Rigidbody.linearVelocity.y, 0);
            }
        }
    }
}