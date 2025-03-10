using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public GameObject UIBG; // renamed
    public GameObject crosshair;
    public Transform inventoryPanel;
    public Transform quickslotPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;
    public float reachDistance = 20f;
    private Camera mainCamera;

    private Outline lastOutlineObject;
    // Start is called before the first frame update
    private void Awake()
    {
        UIBG.SetActive(true);
    }
    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        for (int i = 0; i < quickslotPanel.childCount; i++)
        {
            if (quickslotPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(quickslotPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        UIBG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);//new line
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpened = !isOpened;
            if (isOpened)
            {
                UIBG.SetActive(true);
                inventoryPanel.gameObject.SetActive(true); // new line
                crosshair.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                // и делаем его невидимым
                Cursor.visible = true;

            }
            else
            {
                UIBG.SetActive(false);
                inventoryPanel.gameObject.SetActive(false); // new line
                crosshair.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                // и делаем его невидимым
                Cursor.visible = false;
            }
        }
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            if (hit.transform.gameObject.CompareTag("Item"))
            {
                if (lastOutlineObject != null)
                    lastOutlineObject.enabled = false;
                lastOutlineObject = hit.transform.gameObject.GetComponent<Outline>();
                lastOutlineObject.enabled = true;
            }
            else if (lastOutlineObject != null)
            {
                lastOutlineObject.enabled = false;
                lastOutlineObject = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out hit, reachDistance))
            {
                if (hit.collider.gameObject.GetComponent<Item>() != null)
                {
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item, hit.collider.gameObject.GetComponent<Item>().amount);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
    public InventorySlot FindItemSlot(ItemType itemType)
    {
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.itemType == itemType && !slot.isEmpty)
            {
                return slot;
            }
        }
        return null;
    }
    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            // Стакаем предметы вместе
            // В слоте уже имеется этот предмет
            if (slot.item == _item)
            {
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
                break;
            }
        }
        foreach (InventorySlot slot in slots)
        {
            // добавляем предметы в свободные ячейки
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                if (slot.item.maximumAmount != 1) // added this if statement for single items
                {
                    slot.itemAmountText.text = _amount.ToString();
                }
                break;
            }
        }
    }
}
