using System;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.WindowManager;
using Game.Scripts.Settings.Inventory;
using Game.Scripts.UIScripts.Windows.Inventory;
using VContainer;

namespace Game.Scripts.Game.GameplayControllers.Inventory
{
    public class QuickInventoryController : InventoryController
    {
        public event Action<int> OnSelectedSlotChanged;
        public event Action<InventorySlot> OnItemUsed;
        
        private readonly ISettingsProvider _settingsProvider;
        private readonly IAudioService _audioService;
        private readonly IWindowManager _windowManager;
        
        private int _selectedSlotIndex;
        public int SelectedSlotIndex
        {
            get => _selectedSlotIndex;
            private set
            {
                if (_selectedSlotIndex != value)
                {
                    _selectedSlotIndex = value;
                    OnSelectedSlotChanged?.Invoke(_selectedSlotIndex);
                }
            }
        }
        
        public InventorySlot SelectedSlot => GetSlot(SelectedSlotIndex);
        
        [Inject]
        public QuickInventoryController(IWindowManager windowManager, IAudioService audioService,
            ISettingsProvider settingsProvider) : base(windowManager, audioService, settingsProvider) { }
        
        public void SelectSlot(int index)
        {
            if (IsValidSlot(index))
            {
                SelectedSlotIndex = index;
            }
        }
        
        public void SelectNextSlot()
        {
            int next = SelectedSlotIndex + 1;
            if (next >= SlotCount)
            {
                next = 0;
            }
            SelectSlot(next);
        }
        
        public void SelectPreviousSlot()
        {
            int prev = SelectedSlotIndex - 1;
            if (prev < 0)
            {
                prev = SlotCount - 1;
            }
            SelectSlot(prev);
        }
        
        public bool UseSelectedItem()
        {
            InventorySlot slot = SelectedSlot;
            if (slot == null || slot.IsEmpty)
            {
                return false;
            }
            
            ItemData item = slot.Item;
            bool success = false;
            
            switch (item.ItemUseType)
            {
                case ItemUseType.Consume:
                    success = true;
                    break;
                case ItemUseType.Equip:
                    success = true;
                    break;
                case ItemUseType.Reload:
                    success = true;
                    break;
                default:
                    return false;
            }
            
            if (success)
            {
                OnItemUsed?.Invoke(slot);
                
                if (item.ItemUseType == ItemUseType.Consume)
                {
                    //RemoveItem(slot, 1);
                }
                return true;
            }
            
            return false;
        }
    }
}