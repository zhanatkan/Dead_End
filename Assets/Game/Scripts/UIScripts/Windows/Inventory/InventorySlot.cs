using System;
using Game.Scripts.Settings.Inventory;

namespace Game.Scripts.UIScripts.Windows.Inventory
{
    [Serializable]
    public class InventorySlot
    {
        public ItemData Item;
        public int Amount;
        public bool IsEmpty => Item == null || Amount <= 0;
        
        public void Clear()
        {
            Item = null;
            Amount = 0;
        }
        
        public void SetItem(ItemData item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}