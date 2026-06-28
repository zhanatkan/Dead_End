using Game.Scripts.Settings;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.Scripts.Game.AIScripts.Spawn
{
    public struct MonsterTypeSpawner : IComponentData
    {
        public int MinCount;
        public int MaxCount;
        public float3 Center;
        public float Radius;
    }
    
    [InternalBufferCapacity(100)]
    public struct MonsterPrefabElement : IBufferElementData
    {
        public Entity Prefab;
    }
    
    public class SpawnerAuthoring : MonoBehaviour
    {
        public BotsSetting BotsSetting;

        public class Baker : Baker<SpawnerAuthoring>
        {
            public override void Bake(SpawnerAuthoring authoring)
            {
                foreach (var botSetting in authoring.BotsSetting.Monsters)
                {
                    var entity = CreateAdditionalEntity(TransformUsageFlags.None);
                    
                    AddComponent(entity, new MonsterTypeSpawner
                    {
                        MinCount = botSetting.MinCount,
                        MaxCount = botSetting.MaxCount,
                        Center = botSetting.Center,
                        Radius = botSetting.Radius
                    });

                    var buffer = AddBuffer<MonsterPrefabElement>(entity);
                    foreach (var prefab in botSetting.Prefab)
                    {
                        buffer.Add(new MonsterPrefabElement
                        {
                            Prefab = GetEntity(prefab, TransformUsageFlags.Dynamic)
                        });
                    }
                }
            }
        }
    }

    public partial struct MonsterSpawnSystem : ISystem
    {
        private bool _spawned;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MonsterTypeSpawner>();
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (_spawned)
            {
                return;
            }

            _spawned = true;

            var ecbSystem = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
            var random = Random.CreateFromIndex((uint)UnityEngine.Random.Range(1, int.MaxValue));

            foreach (var (spawner, buffer, entity) in 
                     SystemAPI.Query<RefRO<MonsterTypeSpawner>, DynamicBuffer<MonsterPrefabElement>>()
                         .WithEntityAccess())
            {
                Debug.Log(buffer.Length);
                int count = random.NextInt(spawner.ValueRO.MinCount, spawner.ValueRO.MaxCount + 1);

                for (int i = 0; i < count; i++)
                {
                    var prefabIndex = random.NextInt(0, buffer.Length);
                    var prefab = buffer[prefabIndex].Prefab;

                    float angle = random.NextFloat(0f, math.TAU);
                    float radius = random.NextFloat(0f, spawner.ValueRO.Radius);
                    float3 offset = new float3(math.cos(angle), 0f, math.sin(angle)) * radius;
                    float3 position = spawner.ValueRO.Center + offset;
                    
                    var newEnemy = ecb.Instantiate(prefab);
                    ecb.SetComponent(newEnemy, LocalTransform.FromPosition(position));
                    Debug.Log("Spawning " + i + " of " + count);
                }
            }
        }
    }
}