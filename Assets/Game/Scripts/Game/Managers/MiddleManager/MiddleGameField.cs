using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Base.Services.GameFactory;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Game.GameField;
using Game.Scripts.Game.Maps;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.Managers.MiddleManager
{
    public class MiddleGameField : MonoBehaviour
    {
        [field: SerializeField] public SpawnRange PlayerSpawnRange { get; set; }
        
        [Inject]
        public void Construct()
        {
            
        }
        
        public void Init()
        {
            
        }

        public void StartGameplay()
        {
            
        }

        public void DeInit()
        {
            
        }
    }
}