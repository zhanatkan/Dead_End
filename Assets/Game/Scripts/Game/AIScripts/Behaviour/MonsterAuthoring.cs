using Unity.Entities;
using UnityEngine;

namespace Game.Scripts.Game.AIScripts.Behaviour
{
    public struct MonsterTag : IComponentData { }

    public class MonsterAuthoring : MonoBehaviour
    {
        private class Baker : Baker<MonsterAuthoring>
        {
            public override void Bake(MonsterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<MonsterTag>(entity);
            }
        }
    }
}