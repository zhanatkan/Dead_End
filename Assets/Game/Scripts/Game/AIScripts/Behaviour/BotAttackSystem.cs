using Leopotam.Ecs;
using UnityEngine;
using Game.Scripts.Game.AIScripts.Common;
using Game.Scripts.Settings;

namespace Game.Scripts.Game.AIScripts.Behaviour
{
    public class BotAttackSystem : IEcsRunSystem
    {
        private EcsFilter<BotComponent, BotStateComponent> _botFilter = null;
        private EcsFilter<PlayerTag> _playerFilter = null;

        public void Run()
        {
            if (_playerFilter.IsEmpty()) return;

            Transform playerTransform = _playerFilter.Get1(0).Transform;
            float deltaTime = Time.deltaTime;

            foreach (var i in _botFilter)
            {
                ref var bot = ref _botFilter.Get1(i);
                ref var state = ref _botFilter.Get2(i);

                if (state.CurrentState != BotState.Attack)
                {
                    continue;
                }

                state.AttackCooldownTimer -= deltaTime;

                if (state.AttackCooldownTimer <= 0f)
                {
                    ExecuteAttack(ref bot, ref state, playerTransform, i);
                    state.AttackCooldownTimer = bot.Setting.AttackCooldown;
                }
            }
        }

        private void ExecuteAttack(ref BotComponent bot, ref BotStateComponent state, Transform playerTransform, int entityIndex)
        {
            Vector3 botPos = bot.Transform.position;
            Vector3 playerPos = playerTransform.position;

            EcsEntity playerEntity = _playerFilter.GetEntity(0);

            switch (bot.Setting.AttackType)
            {
                case AttackType.Melee:
                    if (Vector3.Distance(botPos, playerPos) <= bot.Setting.AttackRange * 1.1f)
                    {
                        ApplyDamage(playerEntity, bot.Setting.Damage);
                    }
                    break;

                case AttackType.Ranged:
                    if (bot.Setting.ProjectilePrefab != null)
                    {
                        Vector3 spawnPos = botPos + Vector3.up * 1.2f + bot.Transform.forward * 0.5f;
                        GameObject projGO = Object.Instantiate(bot.Setting.ProjectilePrefab, spawnPos, Quaternion.identity);

                        Vector3 targetDir = (playerPos + Vector3.up * 1f - spawnPos).normalized;
                        projGO.transform.forward = targetDir;

                        if (projGO.TryGetComponent<Rigidbody>(out var projRb))
                        {
                            projRb.linearVelocity = targetDir * bot.Setting.ProjectileSpeed;
                        }

                        if (projGO.GetComponent<BotProjectile>() == null)
                        {
                            var projScript = projGO.AddComponent<BotProjectile>();
                            projScript.Init(bot.Setting.Damage, playerEntity, playerTransform, bot.Setting.BotLayerMask); 
                        }
                    }
                    break;

                case AttackType.SelfDestruct:
                    float distToPlayer = Vector3.Distance(botPos, playerPos);
                    if (distToPlayer <= bot.Setting.ExplosionRadius)
                    {
                        ApplyDamage(playerEntity, bot.Setting.Damage);
                    }

                    EcsEntity botEntity = _botFilter.GetEntity(entityIndex);
                    ApplyDamage(botEntity, bot.Setting.MaxHealth * 10f);
                    break;
            }
        }

        private void ApplyDamage(EcsEntity targetEntity, float damage)
        {
            if (!targetEntity.IsAlive())
            {
                return;
            }

            ref var damageEvent = ref targetEntity.Get<TakeDamageEvent>();
            damageEvent.Damage += damage;
        }
    }
}