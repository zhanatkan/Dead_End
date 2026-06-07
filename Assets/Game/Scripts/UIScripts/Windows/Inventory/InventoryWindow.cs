using System.Collections.Generic;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Game.GameplayControllers.Inventory;
using UnityEngine;
using TMPro;

namespace Game.Scripts.UIScripts.Windows.Inventory
{
    public class InventoryWindow : BaseWindow
    {
        [SerializeField] private Transform SlotsParent;
        [SerializeField] private ButtonWithClickSound QuitButton;
        [SerializeField] private TextMeshProUGUI SlotsInfoText;
        [SerializeField] private InventorySlotView SlotViewPrefab;
        
        private InventoryController _inventoryController;
        private IAudioService _audioService;
        private readonly List<InventorySlotView> _slotViews = new();
        
        public void Init(InventoryController inventoryController, IAudioService audioService)
        {
            _inventoryController = inventoryController;
            _audioService = audioService;
            _inventoryController.OnSlotChanged += OnSlotChanged;
        }
        
        public override void Show()
        {
            CreateSlots();
            UpdateSlotsInfo();
            QuitButton.Init(_audioService, OnQuit);
            base.Show();
        }
        
        public override void Hide()
        {
            _inventoryController.OnSlotChanged -= OnSlotChanged;
            QuitButton.DeInit();
            base.Hide();
        }
        
        private void OnSlotChanged(int index)
        {
            if (index >= 0 && index < _slotViews.Count)
            {
                _slotViews[index].UpdateView();
            }

            UpdateSlotsInfo();
        }
        
        private void CreateSlots()
        {
            foreach (var view in _slotViews)
            {
                Destroy(view.gameObject);
            }

            _slotViews.Clear();
            
            for (int i = 0; i < _inventoryController.SlotCount; i++)
            {
                var view = Instantiate(SlotViewPrefab, SlotsParent);
                view.Initialize(_inventoryController, i, false);
                _slotViews.Add(view);
            }
            UpdateSlotsInfo();
        }
        
        private void UpdateSlotsInfo()
        {
            int occupied = 0;
            foreach (var view in _slotViews)
            {
                var slot = _inventoryController.GetSlot(_slotViews.IndexOf(view));
                if (!slot.IsEmpty)
                {
                    occupied++;
                }
            }
            SlotsInfoText.text = $"{occupied} / {_inventoryController.SlotCount}";
        }
        
        private void OnQuit() => Hide();
    }
}