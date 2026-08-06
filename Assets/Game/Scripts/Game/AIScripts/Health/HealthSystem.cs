using Leopotam.Ecs;
using UnityEngine;
using Game.Scripts.Game.AIScripts.Common;
using Game.Scripts.Game.Character.Player;

namespace Game.Scripts.Game.AIScripts.Health
{
    public class HealthSystem : IEcsRunSystem
    {
        private EcsFilter<HealthComponent, TakeDamageEvent> _damageFilter = null;
        private EcsFilter<HealthComponent> _healthFilter = null;

        public void Run()
        {
            foreach (var i in _damageFilter)
            {
                ref var health = ref _damageFilter.Get1(i);
                ref var damageEvent = ref _damageFilter.Get2(i);

                health.CurrentHealth -= damageEvent.Damage;
                health.CurrentHealth = Mathf.Max(0f, health.CurrentHealth);
            }

            foreach (var i in _healthFilter)
            {
                ref var health = ref _healthFilter.Get1(i);

                if (health.CurrentHealth <= 0f)
                {
                    EcsEntity entity = _healthFilter.GetEntity(i);

                    if (entity.Has<PlayerTag>())
                    {
                        ref var playerTag = ref entity.Get<PlayerTag>();
                        if (playerTag.Transform.TryGetComponent<PlayerController>(out var player))
                        {
                            player.GameOver();
                        }
                    }
                    else if (entity.Has<BotComponent>())
                    {
                        ref var bot = ref entity.Get<BotComponent>();

                        if (bot.Transform != null)
                        {
                            Object.Destroy(bot.Transform.gameObject);
                        }

                        entity.Destroy();
                    }
                }
            }
        }
    }
}