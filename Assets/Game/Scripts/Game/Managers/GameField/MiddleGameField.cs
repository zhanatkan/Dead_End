using System;
using Game.Scripts.Base.Services.Input;
using Game.Scripts.Game.GameField;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.Managers.GameField
{
    public class MiddleGameField : MonoBehaviour
    {
        public event Action OnLevelChoiceTriggerEnterEvent;
        [field: SerializeField] public SpawnRange PlayerSpawnRange { get; set; }
        
        [field: SerializeField] private TriggerPlace LevelChoiceTrigger { get; set; }
        
        private IInputService _inputService; 
        
        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        public void Init()
        {
            LevelChoiceTrigger.Init(OnLevelChoiceTriggerEnter);
        }

        public void StartGameplay()
        {
            
        }

        public void DeInit()
        {
            LevelChoiceTrigger.DeInit();
        }

        private void OnLevelChoiceTriggerEnter()
        {
            if ( _inputService.GetUseInput() )
            {
                OnLevelChoiceTriggerEnterEvent?.Invoke();
                Debug.Log("Clicked");
            }
        }
    }
}