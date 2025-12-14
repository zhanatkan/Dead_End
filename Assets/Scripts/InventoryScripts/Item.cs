using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    [HideInInspector] public string name;
    private void Update()
    {
        name = item.itemName;
    }
}
