using Leopotam.Ecs;
using Game.Scripts.Game.GameField;
using Game.Scripts.Settings;
using UnityEngine;

namespace Game.Scripts.Game.AIScripts.Spawn
{
    public struct MonsterTag
    {
        public MonsterType Type;
        public GameObject GameObject;
    }
    
    public struct MapLoadedEvent
    {
        public Map MapInstance;
    }
}