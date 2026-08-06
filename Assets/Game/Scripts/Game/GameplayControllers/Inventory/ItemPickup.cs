using Game.Scripts.Settings.Inventory;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.GameplayControllers.Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        [field: SerializeField] public ItemData ItemData;
        [field: SerializeField] public int Amount = 1;
        
        public void PickedUp()
        {
            Destroy(gameObject);
        }
    }
}