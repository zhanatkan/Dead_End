using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class OpenDoorController : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private QuickSlotInventory quickslotInventory;
    private DoorManager manager;
    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        quickslotInventory = FindObjectOfType<QuickSlotInventory>();
        manager = FindObjectOfType<DoorManager>();
    }
    private void Update()
    {
        if (manager.isInside == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (quickslotInventory.activeSlot != null)
                {
                    if (quickslotInventory.activeSlot.item != null)
                    {
                        if (quickslotInventory.activeSlot.item.itemType == ItemType.Key)
                        {
                            if (inventoryManager.isOpened == false)
                            {
                                manager.door.GetComponent<Animator>().SetBool("IsOpen", true);
                            }
                            else return;
                        }
                        else return;
                    }
                    else return;
                }
                else return;
            }
            else return;
        }
    }
}
