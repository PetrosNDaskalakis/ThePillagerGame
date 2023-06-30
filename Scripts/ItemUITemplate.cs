using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUITemplate : MonoBehaviour
{
    public Item item;

    public Button itemDetails;

    [SerializeField] Image background;

    public TMP_Text amount;

    public void OnClickButton()
    {
        if (itemDetails.onClick != null)
        {
            FindObjectOfType<InventorySystemUI>().OpenItemDescPanel(item);
        }
        else
        {
            return;
        }
    }

    public void AssignItem(Item item)
    {
        this.item = item;
    }
}
