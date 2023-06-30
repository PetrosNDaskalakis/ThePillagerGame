using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Variables

    public int inventorySize;

    public int itemsCapacity;

    public int tempInt;

    Item item;

    public string nameOfItem;

    public string descOfItem;

    public int quantity;

    public Sprite itemIcon;

    InventorySystemUI ui;

    public Slot slot;

    #endregion

    public GameObject backpackButton;

    public List<Item> Items = new List<Item>();

    public List<Item> CheckItems()
    {
        return Items;
    }

    public void CacheItems()
    {
        Item[] allItems;

        allItems =  Resources.LoadAll<Item>("Items");

        Items.AddRange(allItems);

        if (Items.Count > 0)
        {
            Debug.Log(Items.Count + " ITEM(S) WAS FOUND");
        }
        else
        {
            Debug.Log("NO ITEMS FOUND");
            Debug.Log(Items.Count);
        }
    }

    public List<Item> Inventory = new List<Item>();

    public List<Item> GetInventory()
    {
        return Inventory;
    }

    public void StoreItems(Item item)
    {
        item.CheckItemType();

        if (item.canTakeItem)
        {
            StoreSlotItems(item);
            Debug.Log(item.itemName + " has been stored to your inventory");
        }
    }

    public void UseItem(Item item)
    {
        if(item.itemType == "Potion")
        {
            if(item.itemName == "Health Potion")
            {
                if(Managers.StorageManager.playerHealth == Managers.StorageManager.maxPlayerHealth)
                {
                    Debug.Log("Player has max health");
                    return;
                }
                else if(Managers.StorageManager.playerHealth < Managers.StorageManager.maxPlayerHealth)
                {
                    Debug.Log(Managers.StorageManager.playerHealth);

                    if(Managers.StorageManager.playerHealth + item.boost >= Managers.StorageManager.maxPlayerHealth)
                    {
                        Managers.StorageManager.playerHealth = Managers.StorageManager.maxPlayerHealth;
                        RemoveItem(item);
                    }
                    else if(Managers.StorageManager.playerHealth + item.boost < Managers.StorageManager.maxPlayerHealth)
                    {
                        Managers.StorageManager.playerHealth += item.boost;
                        RemoveItem(item);
                    }
                }
            }
            else if(item.itemName == "Mana Potion")
            {
                if (Managers.StorageManager.playerMana == Managers.StorageManager.maxPlayerMana)
                {
                    return;
                }
                else if (Managers.StorageManager.playerMana < Managers.StorageManager.maxPlayerMana)
                {
                    Debug.Log(Managers.StorageManager.playerMana);

                    if (Managers.StorageManager.playerMana + item.boost > Managers.StorageManager.maxPlayerMana)
                    {
                        Managers.StorageManager.playerMana = Managers.StorageManager.maxPlayerMana;
                        RemoveItem(item);
                    }
                    else if (Managers.StorageManager.playerMana + item.boost < Managers.StorageManager.maxPlayerMana)
                    {
                        Managers.StorageManager.playerMana += item.boost;
                        RemoveItem(item);
                    }
                }
            }
        }
    }

    public void RemoveItem(Item item)
    {
        Slot slot = null;

        foreach(var slots in ui.slots)
        {
            if (slots.item == item)
            {
                if(slots.isMax == false)
                {
                    slot = slots;
                    continue;
                }

                if (slots.isMax == true)
                {
                    slot = slots;
                    continue;
                }
            }
        }

        if (slot.amount == 1)
        {
            Inventory.Remove(item);
            slot.ResetSlot();
            return;
        }
        else if (slot.amount > 1)
        {
            Inventory.Remove(item);
            slot.amount -= 1;
            return;
        }
    }

    public void StoreSlotItems(Item item)
    {
        InventorySystemUI ui = FindObjectOfType<InventorySystemUI>();

        bool hasItem = false;

        foreach (var i in Inventory)
        {
            if (i.itemName == item.itemName)
            {
                hasItem = true;
            }
        }

        if(itemsCapacity < inventorySize)
        {
            if(hasItem == true)
            {
                Inventory.Add(item);
                tempInt++;
                ReachedMaxCapacity();
                AddAmount(item);
            }
            else
            {
                foreach (var slot in ui.slots)
                {   
                    if(slot.item == null)
                    {
                        Inventory.Add(item);
                        tempInt++;
                        ReachedMaxCapacity();
                        ui.SetupSlot(slot, item, item.itemName, 1, item.itemDescription, item.itemSprite);
                        return;
                    }
                }
            }
        }
    }

    public void AddAmount(Item item)
    {
        InventorySystemUI ui = FindObjectOfType<InventorySystemUI>();

        foreach(var slot in ui.slots)
        {
            if (slot.item == item)
            {
                if (slot.isMax)
                {
                    continue;
                }

                if (!slot.isMax)
                {
                    if (slot.amount == slot.limit)
                    {
                        slot.isMax = true;

                        foreach (var slots in ui.slots)
                        {
                            if (slots.item == null)
                            {
                                ui.SetupSlot(slots, item, item.itemName, 1, item.itemDescription, item.itemSprite);
                                return;
                            }
                        }
                    }
                    else
                    {
                        ui.SetupSlot(slot, slot.item, item.itemName, slot.amount + 1, item.itemName, item.itemSprite);
                        return;
                    }
                }
            }
        }
    }

    public List<Item> ClassPotion = new List<Item>();
    private readonly List<Item> ClassWeapon = new List<Item>();

    public void SortByType()
    {
        string Itemtype;

        foreach (var item in Items)
        {
            Itemtype = string.Empty;

            Itemtype += item.itemType;

            if (Itemtype == "Potion")
            {
                ClassPotion.Add(item);
            }
            else if (Itemtype == "Weapon")
            {
                ClassWeapon.Add(item);
            }
            else
            {
                Debug.Log("Type is null... Give Type (Weapon OR Potion)");
            }
        }

        Debug.Log("Class Potion = " + ClassPotion.Count);
        Debug.Log("Class Weapon = " + ClassWeapon.Count);
    }

    public void SetData(Item _item, int _quantity, string _nameOfItem, string _desc, Sprite _itemIcon)
    {
        item = _item;
        quantity = _quantity;
        nameOfItem = _nameOfItem;
        descOfItem = _desc;
        itemIcon = _itemIcon;
    }

    private void Awake()
    {
        CacheItems();
    }

    private void Start()
    {
        //SortByType();
        ui = FindObjectOfType<InventorySystemUI>();
    }

    public void ReachedMaxCapacity()
    {
        if(tempInt == 4)
        {
            itemsCapacity++;
            tempInt = 0;
        }
    }
}
