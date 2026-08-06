using System;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Game.GameplayControllers.Inventory;
using Game.Scripts.UIScripts.Windows.Inventory;
using UnityEngine;
using VContainer;

namespace Game.Scripts.UIScripts.Game
{
    public class GameUI : MonoBehaviour
    {
        public event Action OnPauseButtonClick;
        
        [Header("Inventory")]
        [SerializeField] private QuickSlotsWidget QuickSlotsWidget;
        [SerializeField] private ButtonWithClickSound PauseButton;
        
        private IAudioService _audioService;
        private ISaveLoadService _saveLoadService;
        private QuickInventoryController _quickInventoryController;

        [Inject]
        public void Construct(IAudioService audioService, ISaveLoadService saveLoadService,
            QuickInventoryController quickInventoryController)
        {
            _audioService = audioService;
            _saveLoadService = saveLoadService;
            _quickInventoryController = quickInventoryController;
        }

        public void Init()
        {
            QuickSlotsWidget.Init(_quickInventoryController);
            PauseButton.Init(_audioService, OnPauseButtonClicked);
        }

        public void DeInit()
        {
            QuickSlotsWidget.DeInit();
        }

        public void SetQuickWidgetOverrideCanvas(bool isActive)
        {
            QuickSlotsWidget.SetOverrideCanvas(isActive);
        }

        private void OnPauseButtonClicked()
        {
            OnPauseButtonClick?.Invoke();
        }
    }
}