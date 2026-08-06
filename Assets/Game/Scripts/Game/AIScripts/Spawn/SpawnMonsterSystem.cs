using Leopotam.Ecs;
using UnityEngine;
using Game.Scripts.Settings;
using Game.Scripts.Game.GameField;
using Game.Scripts.Game.AIScripts.Common;

namespace Game.Scripts.Game.AIScripts.Spawn
{
    public class SpawnMonsterSystem : IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<MapLoadedEvent> _filter = null;
        private BotsSetting _botsSetting = null; 

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var mapEvent = ref _filter.Get1(i);
                Map map = mapEvent.MapInstance;

                if (map.BotsSpawnRange == null || map.BotsSpawnRange.Count == 0)
                {
                    continue;
                }

                EcsEntity mapEntity = _world.NewEntity();
                mapEntity.Get<MapComponent>().MapInstance = map;

                if (_botsSetting == null || _botsSetting.Monsters == null)
                {
                    continue;
                }

                foreach (var botSetting in _botsSetting.Monsters)
                {
                    int spawnCount = Random.Range(botSetting.MinCount, botSetting.MaxCount + 1);

                    for (int c = 0; c < spawnCount; c++)
                    {
                        int randomRangeIndex = Random.Range(0, map.BotsSpawnRange.Count);
                        SpawnRange spawnRange = map.BotsSpawnRange[randomRangeIndex];
                        Vector3 spawnPosition = spawnRange.SpawnPosition();

                        if (botSetting.Prefab == null || botSetting.Prefab.Count == 0)
                        {
                            continue;
                        }
                        int randomPrefabIndex = Random.Range(0, botSetting.Prefab.Count);
                        GameObject prefab = botSetting.Prefab[randomPrefabIndex];

                        GameObject monsterGO = Object.Instantiate(prefab, spawnPosition, Quaternion.identity);

                        EcsEntity monsterEntity = _world.NewEntity();
                        
                        ref var bot = ref monsterEntity.Get<BotComponent>();
                        bot.Transform = monsterGO.transform;
                        bot.Rigidbody = monsterGO.GetComponent<Rigidbody>();
                        bot.Setting = botSetting;

                        ref var state = ref monsterEntity.Get<BotStateComponent>();
                        state.CurrentState = BotState.Patrol;
                        
                        ref var health = ref monsterEntity.Get<HealthComponent>();
                        health.MaxHealth = botSetting.MaxHealth;
                        health.CurrentHealth = botSetting.MaxHealth;
                        
                        int initialRangeIndex = Random.Range(0, map.BotsSpawnRange.Count);
                        state.TargetPosition = map.BotsSpawnRange[initialRangeIndex].SpawnPosition();
                        state.Timer = 0f;
                    }
                }
            }
        }
    }
}