using UnityEngine;
using UnityEngine.UI;

public class QuickSlotInventory : MonoBehaviour
{
    public Transform quickSlotParent;
    public InventoryManager inventoryManager;
    public int currentQuickSlotID = 0;
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    public Text healthText;
    public Transform itemContainer;
    public InventorySlot activeSlot = null;
    private Transform allWeapons;
    private PlayerHealth indicators;

    public GameObject GunUiPanel;
    public GameObject ShotGunUiPanel;

    private Door nearbyDoor;
    public void Start()
    {
        GameObject handObject = GameObject.FindGameObjectWithTag("Hand");

        if (handObject != null)
        {
            allWeapons = handObject.transform;
        }
        indicators = FindObjectOfType<PlayerHealth>();
        GunUiPanel.SetActive(false);
        ShotGunUiPanel.SetActive(false);
    }

    public void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0.1)
        {
            quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = notSelectedSprite;
            
            if (currentQuickSlotID >= quickSlotParent.childCount - 1)
            {
                currentQuickSlotID = 0;
            }
            else
            {
                currentQuickSlotID++;
            }

            quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = selectedSprite;
            activeSlot = quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>();
            ShowItemInHand();

        }
        if (mw < -0.1)
        {
            quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = notSelectedSprite;
            
            if (currentQuickSlotID <= 0)
            {
                currentQuickSlotID = quickSlotParent.childCount - 1;
            }
            else
            {
                currentQuickSlotID--;
            }

            quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = selectedSprite;
            activeSlot = quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>();
            ShowItemInHand();
        }
        
        for (int i = 0; i < quickSlotParent.childCount; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                if (currentQuickSlotID == i)
                {
                    if (quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite == notSelectedSprite)
                    {
                        quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = selectedSprite;
                        activeSlot = quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>();
                        ShowItemInHand();
                    }
                    else
                    {
                        quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = notSelectedSprite;
                        activeSlot = null;
                        HideItemsInHand();
                    }
                }
                
                else
                {
                    quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = notSelectedSprite;

                    currentQuickSlotID = i;

                    quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = selectedSprite;
                    activeSlot = quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>();
                    ShowItemInHand();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().item != null)
            {
                if (quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().item.isConsumeable && !inventoryManager.isOpened && quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite == selectedSprite)
                {
                    ChangeCharacteristics();

                    if (quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().amount <= 1)
                    {
                        quickSlotParent.GetChild(currentQuickSlotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                    }
                    else
                    {
                        quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().amount--;
                        quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().itemAmountText.text = quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().amount.ToString();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && activeSlot != null && activeSlot.item != null)
        {
            if (activeSlot.item.itemType == ItemType.Key && nearbyDoor != null) 
            {
                if (nearbyDoor.CanOpenDoorWithKey(activeSlot.item.keyID)) 
                {
                    nearbyDoor.OpenDoor(); 
                    RemoveKeyFromInventory(); 
                }
            }
        }
    }
    public void SetNearbyDoors(Door door)
    {
        nearbyDoor = door; 
    }

    private void RemoveKeyFromInventory()
    {
        activeSlot.item = null;
        activeSlot.itemAmountText.text = "";
        activeSlot.GetComponent<Image>().sprite = notSelectedSprite;
        activeSlot = null;
        quickSlotParent.GetChild(currentQuickSlotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
        HideItemsInHand(); 
    }
    public void CheckItemInHand()
    {
        if (activeSlot != null)
        {
            ShowItemInHand();
        }
        else
        {
            HideItemsInHand();
        }
    }

    private void ChangeCharacteristics()
    {
        indicators.ChangeHealthAmount(quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().item.changeHealth);
    }

    private void ShowItemInHand()
    {
        HideItemsInHand();
        if (activeSlot.item == null)
        {
            return;
        }
        for (int i = 0; i < allWeapons.childCount; i++)
        {
            if (activeSlot.item.inHandName == allWeapons.GetChild(i).name)
            {
                allWeapons.GetChild(i).gameObject.SetActive(true);
            }
        }
        if(activeSlot.item.itemType == ItemType.Weapon)
        {
            GunUiPanel.SetActive(true);
        }
        else
        {
            GunUiPanel.SetActive(false);
        }
        if (activeSlot.item.itemType == ItemType.Gun)
        {
            ShotGunUiPanel.SetActive(true);
        }
        else
        {
            ShotGunUiPanel.SetActive(false);
        }
    }
    
    private void HideItemsInHand()
    {
        for (int i = 0; i < allWeapons.childCount; i++)
        {
            allWeapons.GetChild(i).gameObject.SetActive(false);
        }
        ShotGunUiPanel.SetActive(false);
        GunUiPanel.SetActive(false);
    }
}