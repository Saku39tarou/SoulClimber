using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Heal,
    Item
}

[System.Serializable]
public class Item
{
    public int id;              // アイテムのID
    public ItemType itemType;   // アイテムの種類
    public string itemName;     // アイテムの名前
    [TextArea]
    public int effectValue;     // 効果値
}

[CreateAssetMenu(fileName = "ItemList", menuName = "Inventory/ItemList")]
public class ItemList : ScriptableObject
{
    public Item[] items;

    public Item GetItemById(int id)
    {
        foreach (Item item in items)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        Debug.LogWarning($"Item with ID {id} not found.");
        return null; 
    }
}

