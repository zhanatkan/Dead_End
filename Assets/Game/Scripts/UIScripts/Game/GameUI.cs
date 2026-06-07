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
        [Header("Inventory")]
        [SerializeField] private QuickSlotsWidget QuickSlotsWidget;
        
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
        }

        public void DeInit()
        {
            QuickSlotsWidget.DeInit();
        }
    }
}