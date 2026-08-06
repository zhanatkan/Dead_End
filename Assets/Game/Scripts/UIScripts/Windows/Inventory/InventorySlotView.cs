using UnityEngine;
using UnityEngine.EventSystems;
using Game.Scripts.Game.GameplayControllers.Inventory;
using UnityEngine.UI;
using TMPro;

namespace Game.Scripts.UIScripts.Windows.Inventory
{
    public class InventorySlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image IconImage;
        [SerializeField] private TextMeshProUGUI AmountText;
        [SerializeField] private GameObject SelectionHighlight;
        
        private InventoryController _inventoryController;
        private int _slotIndex;
        
        private class DragData
        {
            public InventoryController Controller;
            public int Index;
            public GameObject Ghost;
            public Canvas RootCanvas;
        }
        private DragData _dragData;
        
        public void Initialize(InventoryController inventoryController, int index, bool isQuickSlot = false)
        {
            _inventoryController = inventoryController;
            _slotIndex = index;
            UpdateView();
        }
        
        public void UpdateView()
        {
            InventorySlot slot = _inventoryController.GetSlot(_slotIndex);
            if (slot == null || slot.IsEmpty)
            {
                IconImage.sprite = null;
                IconImage.color = new Color(1, 1, 1, 0);
                AmountText.text = "";
            }
            else
            {
                IconImage.sprite = slot.Item.ItemIcon;
                IconImage.color = Color.white;
                AmountText.text = slot.Amount > 1 ? slot.Amount.ToString() : "";
            }
        }
        
        public void SetHighlight(bool active) => SelectionHighlight.SetActive(active);
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            InventorySlot slot = _inventoryController.GetSlot(_slotIndex);
            if (slot == null || slot.IsEmpty)
            {
                eventData.pointerDrag = null;
                return;
            }
            
            _dragData = new DragData
            {
                Controller = _inventoryController,
                Index = _slotIndex,
            };
            
            Canvas rootCanvas = GetComponentInParent<Canvas>()?.rootCanvas;
            if (rootCanvas == null)
            {
                return;
            }
            _dragData.RootCanvas = rootCanvas;
            
            GameObject ghost = new GameObject("DragGhost");
            ghost.transform.SetParent(rootCanvas.transform, false);
            ghost.transform.SetAsLastSibling();
            Image ghostImage = ghost.AddComponent<Image>();
            ghostImage.sprite = slot.Item.ItemIcon;
            ghostImage.raycastTarget = false;
            ghostImage.color = new Color(1, 1, 1, 0.8f);
            RectTransform ghostRect = ghost.GetComponent<RectTransform>();
            RectTransform originalRect = IconImage.GetComponent<RectTransform>();
            ghostRect.sizeDelta = originalRect.sizeDelta;
            ghostRect.pivot = originalRect.pivot;
            
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootCanvas.transform as RectTransform,
                Input.mousePosition, rootCanvas.worldCamera, out pos);
            ghostRect.anchoredPosition = pos;
            
            _dragData.Ghost = ghost;
            
            IconImage.color = new Color(1, 1, 1, 0.5f);
            IconImage.raycastTarget = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_dragData?.Ghost == null)
            {
                return;
            }
            RectTransform ghostRect = _dragData.Ghost.GetComponent<RectTransform>();
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _dragData.RootCanvas.transform as RectTransform,
                eventData.position, _dragData.RootCanvas.worldCamera, out localPoint);
            ghostRect.anchoredPosition = localPoint;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragData == null)
            {
                return;
            }
            
            IconImage.color = Color.white;
            IconImage.raycastTarget = true;

            if (_dragData.Ghost != null)
            {
                Destroy(_dragData.Ghost);
            }

            InventorySlotView targetSlotView = null;
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                targetSlotView = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlotView>();
                if (targetSlotView == null)
                {
                    targetSlotView =
                        eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<InventorySlotView>();
                }
            }
            
            if (targetSlotView != null && targetSlotView != this)
            {
                InventoryController targetController = targetSlotView._inventoryController;
                int targetIndex = targetSlotView._slotIndex;
                
                targetController.MoveItem(_dragData.Index, targetIndex, _dragData.Controller);
            }
            
            _dragData = null;
        }
    }
}