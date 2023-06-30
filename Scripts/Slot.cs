using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int SlotIndex;
    
    public int amount;

    public int limit = 5;

    public bool isMax;

    public Item item;

    public string desc;

    public TMP_Text nameText;
    public TMP_Text amountText;

    public Image itemImage;
    public Sprite itemIcon;
    public Sprite defaultImage;

    public void SetData(string s, int a) { nameText.text = s; amountText.text = a.ToString(); }

    public void ShowDetails()
    {
        FindObjectOfType<InventorySystemUI>().OpenItemDescPanel(item);
    }
    
    public void GetItem()
    {
        FindObjectOfType<InventorySystemUI>().item = item;

    }

    public void ResetSlot()
    {
        item = null;
        desc = null;
        nameText.text = "";
        amountText.text = "";
        amount = 0;
        itemImage.sprite = defaultImage;
        itemIcon = null;
        isMax = false;
    }

    public void AddItem(Item _item)
    {
        if(item == null)
        {
            item = _item;
            amount += 1;
        }
        else if(item == _item)
        {
            amount += 1;
        }
    }

    private void Update()
    {
        if(amount == 0)
        {
            amountText.text = "";
        }
        else
        {
            amountText.text = amount.ToString();
        }
    }

    public void ResetData()
    {
        FindObjectOfType<InventoryManager>().SetData(null, 0, null, null, defaultImage);
    }

    public void GetData()
    {
        FindObjectOfType<InventoryManager>().SetData(item, amount, name, desc, itemImage.sprite);
    }
}
