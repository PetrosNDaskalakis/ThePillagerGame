using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using UnityEngine.SceneManagement;

public class InventorySystemUI : MonoBehaviour
{
    #region UI PANELS
    //Show the Inventory panel
    public Button inventoryButton;
    public Button BackToInventoryButton;

    //UI Panels
    public GameObject InventoryListPanel;
    public GameObject ItemDescPanel;

    //description panel
    public TMP_Text Name;
    public TMP_Text description;
    public Image itemImage;

    //Currency Texts
    public TMP_Text goldText;
    public TMP_Text gemsText;

    public Item item = null;

    public List<Item> itemPresets = new List<Item>();

    public void UpdateCurrency()
    {
        goldText.text = Managers.GameManager.WriteString("Gold: " + StorageManager.gold);

        gemsText.text = Managers.GameManager.WriteString("Gems: " + StorageManager.gems);
    }

    public bool isOpen = false;

    private void Update()
    {
        UpdateCurrency();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            OptionalQuestSystemUI questsUI = FindObjectOfType<OptionalQuestSystemUI>();

            if(isOpen == true)
            {
                CloseInventoryPanel();
                isOpen = false;
            }
            else if(isOpen == false)
            {
                OpenInventoryPanel();
                if (questsUI.isOpen == true)
                {
                    questsUI.CloseQuestPanel();
                    questsUI.isOpen = false;
                }
                isOpen = true;
            }
            
        }
    }

    public void ClickButton()
    {
        if (inventoryButton.onClick != null)
        {
            OpenInventoryPanel();
        }
        else
        {
            return;
        }
    }

    public void OpenInventoryPanel()
    {
        InventoryListPanel.SetActive(true);
    }

    public void CloseInventoryPanel()
    {
        InventoryListPanel.SetActive(false);
    }

    public void OpenItemDescPanel(Item _item)
    {
        if(inventory.quantity != 0)
        {
            ItemDescPanel.SetActive(true);

            Name.text = _item.itemName;
            description.text = _item.itemDescription;
            itemImage.sprite = _item.itemSprite;
        }
    }

    public void UseItem()
    {
        foreach (var _item in FindObjectOfType<InventoryManager>().Inventory)
        {
            if (_item == item)
            {
                FindObjectOfType<InventoryManager>().UseItem(item);
                return;
            }
        }
    }

    public void BackToButton()
    {
        if(SceneManager.GetActiveScene().name == "BattleScene")
        {
            InventoryListPanel.SetActive(false);
            ItemDescPanel.SetActive(false);

            BackToInventoryButton.gameObject.SetActive(false);

            if (FindObjectOfType<BackpackButton>() != null)
            {
                BackpackButton button_ = FindObjectOfType<BackpackButton>();
                button_.gameObject.SetActive(false);
            }
        }
        else
        {
            BackpackButton button_ = FindObjectOfType<BackpackButton>();
            button_.isClicked = false;
            button_.backbackButton.interactable = true;
            InventoryListPanel.SetActive(false);
            ItemDescPanel.SetActive(false);
        }
    }

    public void BackToInventoryPanel()
    {
        ItemDescPanel.SetActive(false);
    }
    #endregion

    #region UI MECHANICS
    //cache the inventory
    public InventoryManager inventory;
    //cache the Slots Script that's on the slot prefab
    public Slot temp;
    //cache the content box 
    public GameObject content;

    private void Start()
    {
        CreateSlots();
    }

    //we need a list of Items in slots
    public List<Slot> slots = new List<Slot>();

    public List<Slot> CheckSlots()
    {
        return slots;
    }

    public void CreateSlots()
    {
        //based on the backpack...

        for (int i = 0; i < inventory.inventorySize; i++)
        {
            Slot newSlot = Instantiate(temp, content.transform);
            slots.Add(newSlot);
            newSlot.SlotIndex = slots.Count;
            newSlot.amount = 0;
        }

        if (SaveSystem.CheckFile())
        {
            List<Item> items = new List<Item>();

            foreach(var i in inventory.Inventory)
            {
                items.Add(i);
            }

            inventory.Inventory.Clear();

            foreach(var item in items)
            {
                if(item.id == 1)
                {
                    inventory.StoreSlotItems(itemPresets[0]);
                }
                else if(item.id == 2)
                {
                    inventory.StoreSlotItems(itemPresets[1]);
                }
                else if(item.id == 3)
                {
                    inventory.StoreSlotItems(itemPresets[2]);
                }
            }
        }
    }

    public void SetupSlot(Slot slot, Item item, string name, int amount, string _desc, Sprite _image)
    {
        slot.item = item;
        slot.nameText.text = name;
        slot.desc = _desc;
        slot.amount = amount;
        slot.itemImage.sprite = _image;
    }

    #endregion
}
