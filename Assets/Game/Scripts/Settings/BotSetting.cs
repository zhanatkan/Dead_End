using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = nameof(BotSetting), menuName = "Settings/" + nameof(BotSetting))]
    public class BotSetting : ScriptableObject
    {
        public MonsterType MonsterType;
        public List<GameObject> Prefab;
        public int MinCount;
        public int MaxCount;
        
        [Header("Movement")]
        public float PatrolSpeed = 3f;
        public float ChaseSpeed = 6f;
        public float RotationSpeed = 10f;

        [Header("Vision")]
        public float VisionRadius = 15f;
        public float VisionAngle = 90f;
        public LayerMask VisionMask;

        [Header("Alert State")]
        public float MinAlertTime = 3f;
        public float MaxAlertTime = 8f;
    }

    public enum MonsterType
    {
        Zombie,
        Cyclops,
        Golem, 
        Spider,
        MoleRat,
        SuperMoleRat,
        SkullEater,
        Bear,
        Cryper,
        Hound,
        Titan,
        Poisoner,
        Ghoul,
    }
}