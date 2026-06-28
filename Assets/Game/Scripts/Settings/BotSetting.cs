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
        
        public Vector3 Center;
        public float Radius;
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