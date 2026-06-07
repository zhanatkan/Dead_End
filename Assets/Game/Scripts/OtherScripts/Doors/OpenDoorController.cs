using UnityEngine;

namespace Game.Scripts.OtherScripts.Doors
{
    public class OpenDoorController : MonoBehaviour
    {
        //private InventoryManager inventoryManager;
        //private QuickSlotInventory quickSlotInventory;
        private DoorManager manager;

        private void Start()
        {
            //inventoryManager = FindObjectOfType<InventoryManager>();
            //quickSlotInventory = FindObjectOfType<QuickSlotInventory>();
            manager = FindObjectOfType<DoorManager>();
        }

        private void Update()
        {
            if (manager.isInside)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    /*if (quickSlotInventory.activeSlot != null)
                    {
                        if (quickSlotInventory.activeSlot.item != null)
                        {
                            if (quickSlotInventory.activeSlot.item.itemType == ItemType.Key)
                            {
                                if (inventoryManager.isOpened == false)
                                {
                                    manager.door.GetComponent<Animator>().SetBool("IsOpen", true);
                                }
                            }
                        }
                    }*/
                }
            }
        }
    }
}