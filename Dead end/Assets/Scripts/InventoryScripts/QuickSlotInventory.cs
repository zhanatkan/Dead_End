using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotInventory : MonoBehaviour
{
    // Объект у которого дети являются слотами
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
        // Используем колесико мышки
        if (mw > 0.1)
        {
            // Берем предыдущий слот и меняем его картинку на обычную

            quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = notSelectedSprite;
            // Здесь добавляем что случится когда мы УБЕРАЕМ ВЫДЕЛЕНИЕ со слота (Выключить нужный нам предмет, поменять аниматор ...)

            // Если крутим колесиком мышки вперед и наше число currentQuickSlotID равно последнему слоту, то выбираем наш первый слот (первый слот считается нулевым)
            if (currentQuickSlotID >= quickSlotParent.childCount - 1)
            {
                currentQuickSlotID = 0;
            }
            else
            {
                // Прибавляем к числу currentQuickSlotID единичку
                currentQuickSlotID++;
            }
            // Берем предыдущий слот и меняем его картинку на "выбранную"

            quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = selectedSprite;
            activeSlot = quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>();
            ShowItemInHand();
            // Здесь добавляем что случится когда мы ВЫДЕЛЯЕМ слот (Включить нужный нам предмет, поменять аниматор ...)

        }
        if (mw < -0.1)
        {
            // Берем предыдущий слот и меняем его картинку на обычную

            quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = notSelectedSprite;
            // Здесь добавляем что случится когда мы УБЕРАЕМ ВЫДЕЛЕНИЕ со слота (Выключить нужный нам предмет, поменять аниматор ...)


            // Если крутим колесиком мышки назад и наше число currentQuickSlotID равно 0, то выбираем наш последний слот
            if (currentQuickSlotID <= 0)
            {
                currentQuickSlotID = quickSlotParent.childCount - 1;
            }
            else
            {
                // Уменьшаем число currentQuickSlotID на 1
                currentQuickSlotID--;
            }
            // Берем предыдущий слот и меняем его картинку на "выбранную"

            quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = selectedSprite;
            activeSlot = quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>();
            ShowItemInHand();
            // Здесь добавляем что случится когда мы ВЫДЕЛЯЕМ слот (Включить нужный нам предмет, поменять аниматор ...)

        }
        // Используем цифры
        for (int i = 0; i < quickSlotParent.childCount; i++)
        {
            // если мы нажимаем на клавиши 1 по 5 то...
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                // проверяем если наш выбранный слот равен слоту который у нас уже выбран, то
                if (currentQuickSlotID == i)
                {
                    // Ставим картинку "selected" на слот если он "not selected" или наоборот
                    if (quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite == notSelectedSprite)
                    {
                        quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = selectedSprite;
                        activeSlot = quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>();
                        ShowItemInHand();
                        //foreach ...
                    }
                    else
                    {
                        quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite = notSelectedSprite;
                        activeSlot = null;
                        HideItemsInHand();
                        //foreach ...

                    }
                }
                // Иначе мы убираем свечение с предыдущего слота и светим слот который мы выбираем
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
        // Используем предмет по нажатию на левую кнопку мыши
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().item != null)
            {
                if (quickSlotParent.GetChild(currentQuickSlotID).GetComponent<InventorySlot>().item.isConsumeable && !inventoryManager.isOpened && quickSlotParent.GetChild(currentQuickSlotID).GetComponent<Image>().sprite == selectedSprite)
                {
                    // Применяем изменения к здоровью (будущем к голоду и жажде) 
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
            if (activeSlot.item.itemType == ItemType.Key && nearbyDoor != null) // Проверяем, выбран ли ключ и есть ли дверь рядом
            {
                if (nearbyDoor.CanOpenDoorWithKey(activeSlot.item.keyID)) // Проверяем, подходит ли ключ к двери
                {
                    nearbyDoor.OpenDoor(); // Открываем дверь
                    RemoveKeyFromInventory(); // Убираем ключ из инвентаря и руки
                }
            }
        }
    }
    public void SetNearbyDoors(Door door)
    {
        nearbyDoor = door; // Устанавливаем список дверей, с которыми можно взаимодействовать
    }

    private void RemoveKeyFromInventory()
    {
        // Убираем ключ из быстрого инвентаря и руки
        activeSlot.item = null;
        activeSlot.itemAmountText.text = "";
        activeSlot.GetComponent<Image>().sprite = notSelectedSprite;
        activeSlot = null;
        quickSlotParent.GetChild(currentQuickSlotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
        HideItemsInHand(); // Убираем ключ из руки
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
