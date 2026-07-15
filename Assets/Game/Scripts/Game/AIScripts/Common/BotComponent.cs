using UnityEngine;
using Game.Scripts.Settings;
using Game.Scripts.Game.GameField;

namespace Game.Scripts.Game.AIScripts.Common
{
    public enum BotState
    {
        Patrol,
        Chase,
        Alert 
    }

    public struct BotComponent
    {
        public Transform Transform;
        public Rigidbody Rigidbody;
        public BotSetting Setting;
    }

    public struct BotStateComponent
    {
        public BotState CurrentState;
        public Vector3 TargetPosition;
        public float Timer;
    }

    public struct PlayerTag
    {
        public Transform Transform;
    }
    
    public struct MapComponent
    {
        public Map MapInstance;
    }
}