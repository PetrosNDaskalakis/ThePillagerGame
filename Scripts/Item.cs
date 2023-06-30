using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InvIt", menuName = "Inventory/InvIt")]
public class Item : ScriptableObject
{
    //The Name of the item
    public string itemName;

    public int itemPrice;

    public int id;

    public int shopQuantity;

    public Sprite itemSprite;

    //The type of the Item...
    //an item can be an Potion or weapon
    public string itemType;

    public string CheckItemType()
    {
        return itemType;
    }

    //This is the description of the Item
    [TextArea]
    public string itemDescription;

    //Check if the player has obtain this item
    public bool canTakeItem = false;

    public int boost;

    public int amount;
}
