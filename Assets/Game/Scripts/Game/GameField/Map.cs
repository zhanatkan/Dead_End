using UnityEngine;
using System.Collections.Generic;
using Game.Scripts.UIScripts.Windows.LevelChoice;

namespace Game.Scripts.Game.GameField
{
    public class Map : MonoBehaviour
    {
        [SerializeField] public LevelName LevelName { get; private set; }
        [field: SerializeField] public SpawnRange PlayerSpawnRange { get; private set; }
        //[field: SerializeField] public List<SpawnRange> BotsSpawnRange { get; private set; }
        
        [Header("Elements")]
        [SerializeField] private SpawnRange ElementsSpawnRange;
        public IRandomPosition RandomPosition => ElementsSpawnRange;
    }
}