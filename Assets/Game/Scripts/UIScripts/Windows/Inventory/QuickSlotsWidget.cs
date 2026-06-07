using System.Collections.Generic;
using Game.Scripts.Game.GameplayControllers.Inventory;
using UnityEngine;

namespace Game.Scripts.UIScripts.Windows.Inventory
{
    public class QuickSlotsWidget : MonoBehaviour
    {
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private InventorySlotView slotPrefab;
        
        private QuickInventoryController _quickController;
        private List<InventorySlotView> _slotViews = new();
        
        public void Init(QuickInventoryController quickController)
        {
            _quickController = quickController;
            CreateSlots();
            _quickController.OnSelectedSlotChanged += OnSelectedSlotChanged;
        }
        
        public void DeInit()
        {
            _quickController.OnSelectedSlotChanged -= OnSelectedSlotChanged;
        }
        
        private void CreateSlots()
        {
            for (int i = 0; i < _quickController.SlotCount; i++)
            {
                var view = Instantiate(slotPrefab, slotsContainer);
                view.Initialize(_quickController, i, true);
                _slotViews.Add(view);
            }
            OnSelectedSlotChanged(_quickController.SelectedSlotIndex);
        }
        
        private void OnSelectedSlotChanged(int index)
        {
            for (int i = 0; i < _slotViews.Count; i++)
            {
                _slotViews[i].SetHighlight(i == index);
            }
        }
        
        private void Update()
        {
            foreach (var view in _slotViews)
            {
                view.UpdateView();
            }
        }
    }
}