using UnityEngine;

namespace Game.Scripts.Settings.Inventory
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        public string ItemName;
        public int ItemID;
        public ItemCategory ItemCategory;
        public ItemType ItemType;
        public ItemUseType ItemUseType;
        
        public Sprite ItemIcon;
        public GameObject ItemPrefab;
        
        [Tooltip("Максимальное количество в одном стаке")]
        public int MaxStack = 64;
        
        [Tooltip("Имя объекта в руке (для Equip типа)")]
        public string InHandName;
    }
}