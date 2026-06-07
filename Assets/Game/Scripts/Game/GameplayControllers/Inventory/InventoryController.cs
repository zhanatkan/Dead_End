using System;
using System.Collections.Generic;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.WindowManager;
using Game.Scripts.Settings.Inventory;
using Game.Scripts.UIScripts.Windows.Inventory;
using UnityEngine;
using VContainer;
using InventorySlot = Game.Scripts.UIScripts.Windows.Inventory.InventorySlot;

namespace Game.Scripts.Game.GameplayControllers.Inventory
{
    public class InventoryController
    {
        public event Action<int> OnSlotChanged;
        
        private readonly List<InventorySlot> _slots;
        private readonly IWindowManager _windowManager;
        private readonly IAudioService _audioService;
        private readonly ISettingsProvider _settingsProvider;
        
        public int SlotCount => _slots.Count;
        
        private InventoryWindow _inventoryWindow;
        
        [Inject]
        public InventoryController(IWindowManager windowManager, IAudioService audioService,
            ISettingsProvider settingsProvider)
        {
            _windowManager = windowManager;
            _audioService = audioService;
            _settingsProvider = settingsProvider;

            int slotCount = _settingsProvider.PlayerSettings.InventorySettings.MaxSlotsCount;
            _slots = new List<InventorySlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                _slots.Add(new InventorySlot());
            }
        }

        public void Init()
        {
            _inventoryWindow = _windowManager.CreateWindow<InventoryWindow>();
        }
        
        public InventorySlot GetSlot(int index)
        {
            if (!IsValidSlot(index)) return null;
            return _slots[index];
        }

        public void OpenInventoryWindow()
        {
            _inventoryWindow.Init(this, _audioService);
            _inventoryWindow.Show();
        }
        
        public bool AddItem(ItemData item, int amount = 1)
        {
            if (amount <= 0)
            {
                return false;
            }
            
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (!slot.IsEmpty && slot.Item == item && slot.Amount < item.MaxStack)
                {
                    int space = item.MaxStack - slot.Amount;
                    int toAdd = Mathf.Min(space, amount);
                    slot.Amount += toAdd;
                    amount -= toAdd;
                    NotifySlotChanged(i);
                    if (amount <= 0)
                    {
                        return true;
                    }
                }
            }
            
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].IsEmpty)
                {
                    int toAdd = Mathf.Min(item.MaxStack, amount);
                    _slots[i].SetItem(item, toAdd);
                    amount -= toAdd;
                    NotifySlotChanged(i);
                    if (amount <= 0)
                    {
                        return true;
                    }
                }
            }
            
            return amount <= 0;
        }
        
        public void MoveItem(int fromIndex, int toIndex, InventoryController sourceService = null)
        {
            InventoryController fromService = sourceService ?? this;
            InventoryController toService = this;
            
            InventorySlot fromSlot = fromService.GetSlot(fromIndex);
            InventorySlot toSlot = toService.GetSlot(toIndex);

            if (fromSlot == null || fromSlot.IsEmpty || toSlot == null ||
                (fromService == toService && fromIndex == toIndex))
            {
                return;
            }

            if (toSlot.IsEmpty)
            {
                toSlot.SetItem(fromSlot.Item, fromSlot.Amount);
                fromSlot.Clear();
                NotifySourceTarget(fromService, fromIndex, toService, toIndex);
                return;
            }
            
            if (fromSlot.Item == toSlot.Item && toSlot.Amount < toSlot.Item.MaxStack)
            {
                int space = toSlot.Item.MaxStack - toSlot.Amount;
                int moveAmount = Mathf.Min(space, fromSlot.Amount);
                toSlot.Amount += moveAmount;
                fromSlot.Amount -= moveAmount;
                if (fromSlot.Amount <= 0)
                {
                    fromSlot.Clear();
                }
                NotifySourceTarget(fromService, fromIndex, toService, toIndex);
                return;
            }
            
            SwapSlots(fromIndex, toIndex, fromService, toService);
        }
        
        protected void RemoveItem(int slotIndex, int amount = 1)
        {
            if (!IsValidSlot(slotIndex))
            {
                return;
            }
            var slot = _slots[slotIndex];
            if (slot.IsEmpty || amount <= 0)
            {
                return;
            }
            
            slot.Amount -= amount;
            if (slot.Amount <= 0)
            {
                slot.Clear();
            }
            NotifySlotChanged(slotIndex);
        }

        private void NotifySlotChanged(int index)
        {
            OnSlotChanged?.Invoke(index);
        }
        
        private void NotifySourceTarget(InventoryController fromService, int fromIndex, 
            InventoryController toService, int toIndex)
        {
            if (fromService == toService)
            {
                NotifySlotChanged(fromIndex);
                
                if (fromIndex != toIndex)
                {
                    NotifySlotChanged(toIndex);
                }
            }
            else
            {
                fromService.NotifySlotChanged(fromIndex);
                toService.NotifySlotChanged(toIndex);
            }
        }
        
        private bool SwapSlots(int indexA, int indexB, InventoryController serviceA = null, InventoryController serviceB = null)
        {
            InventoryController aService = serviceA ?? this;
            InventoryController bService = serviceB ?? this;
            
            InventorySlot slotA = aService.GetSlot(indexA);
            InventorySlot slotB = bService.GetSlot(indexB);
            if (slotA == null || slotB == null)
            {
                return false;
            }
            
            (slotA.Item, slotB.Item) = (slotB.Item, slotA.Item);
            (slotA.Amount, slotB.Amount) = (slotB.Amount, slotA.Amount);
            
            if (aService == bService)
            {
                NotifySlotChanged(indexA);
                NotifySlotChanged(indexB);
            }
            else
            {
                aService.NotifySlotChanged(indexA);
                bService.NotifySlotChanged(indexB);
            }
            return true;
        }
        
        protected bool IsValidSlot(int index) => index >= 0 && index < _slots.Count;
    }
}