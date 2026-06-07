using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Settings.Inventory
{
    namespace Game.Scripts.Configs.Game
    {
        [CreateAssetMenu(fileName = nameof(ItemsSetting), menuName = "Settings/" + nameof(ItemsSetting), order = 0)]
        public class ItemsSetting : ScriptableObject
        {
            public List<ItemData> ItemSettings;

            public ItemData GetItemDataByType(ItemType itemType)
            {
                foreach (var itemData in ItemSettings)
                {
                    if (itemData.ItemType == itemType)
                    {
                        return itemData;
                    }
                }
                return null;
            }
        }
    }
}