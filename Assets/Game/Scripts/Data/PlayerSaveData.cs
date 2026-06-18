using System;
using System.Collections.Generic;
using Game.Scripts.UIScripts.Windows.LevelChoice;

namespace Game.Scripts.Data
{
    [Serializable]
    public class PlayerSaveData
    {
        public List<ItemSlotSave> MainInventorySlots = new();
        public List<ItemSlotSave> QuickInventorySlots = new();
        public int MaxSlotsCount = 16;
        public int Records = 0;

        public LevelName LevelName;
    }

    [Serializable]
    public class ItemSlotSave
    {
        public int ItemID;
        public int Amount;
        
        public ItemSlotSave(int itemID, int amount)
        {
            ItemID = itemID;
            Amount = amount;
        }
    }
}