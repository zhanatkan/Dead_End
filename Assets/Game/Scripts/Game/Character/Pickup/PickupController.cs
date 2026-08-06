using System;
using Game.Scripts.Game.GameplayControllers.Inventory;
using UnityEngine;

namespace Game.Scripts.Game.Character.Pickup
{
    public class PickupController : MonoBehaviour
    {
        [SerializeField] private float MaxDistance;
        [SerializeField] private LayerMask LayerMask;
        
        private InventoryController _inventoryController;
        private Transform _camera;

        public void Construct(InventoryController inventoryController,
            Transform camera)
        {
            _inventoryController = inventoryController;
            _camera = camera;
        }

        public void TryPickup()
        {
            Ray ray = new Ray(_camera.position, _camera.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, MaxDistance, LayerMask))
            {
                if (hit.collider.TryGetComponent<ItemPickup>(out var pickup))
                {
                    _inventoryController.AddItem(pickup.ItemData, pickup.Amount);
                    pickup.PickedUp();
                }
            }
        }
    }
}