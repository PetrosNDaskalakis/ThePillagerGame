using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySaveLoad : MonoBehaviour
{
    public List<ShopItems> inv = new List<ShopItems>();
    public void SaveInventory()
    {
        string inventoryJson = JsonUtility.ToJson(inv);
        PlayerPrefs.SetString("inventory", inventoryJson);
        PlayerPrefs.Save();
    }
    public void LoadInventory()
    {
        string inventoryJson = PlayerPrefs.GetString("inventory");
        if (!string.IsNullOrEmpty(inventoryJson))
        {
            inv = JsonUtility.FromJson<List<ShopItems>>(inventoryJson);
        }
    }
}
