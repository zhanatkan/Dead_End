using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
        public bool CanRun = true;
        public float PatrolSpeed = 3f;
        public float ChaseSpeed = 6f;
        public float RotationSpeed = 10f;
        
        [Header("Health")]
        public float MaxHealth = 100f;

        [Header("Vision")]
        public float VisionRadius = 15f;
        public float VisionAngle = 90f;
        public LayerMask VisionMask;
        public LayerMask BotLayerMask;

        [Header("Alert State")]
        public float MinAlertTime = 3f;
        public float MaxAlertTime = 8f;
        
        [Header("Hearing")]
        public float BaseHearingRadius = 10f;
        public float AlertHearingMultiplier = 1.5f;
        [Range(0f, 1f)]
        public float WallNoiseOcclusion = 0.5f; 
        
        [Header("Attack Settings")]
        public AttackType AttackType = AttackType.Melee;
        public float AttackRange = 2f;
        public float AttackCooldown = 1.5f;
        public float Damage = 10f;

        [Header("Ranged Attack Settings")]
        public GameObject ProjectilePrefab;
        public float ProjectileSpeed = 15f;

        [Header("Self Destruct Settings")]
        public float ExplosionRadius = 4f;
        public GameObject ExplosionEffectPrefab;
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
    
    public enum AttackType
    {
        Melee,
        Ranged,
        SelfDestruct
    }
}