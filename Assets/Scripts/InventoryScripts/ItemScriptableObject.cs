using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {Default,Food,Weapon,Instrument,Torch,Heal,Key,Gun,PistolMagazine,RiffleMagazine}
public class ItemScriptableObject : ScriptableObject
{

    public string itemName;
    public int maximumAmount;
    public GameObject itemPrefab;
    public Sprite icon;
    public ItemType itemType;

    public bool isConsumeable;
    public string inHandName;

    public int keyID;

    [Header("Consumable Characteristics")]
    public float changeHealth;

}
