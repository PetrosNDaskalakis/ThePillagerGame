using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackButton : MonoBehaviour
{
    public Button backbackButton;

    public GameObject resourcesPanel;

    public RectTransform backpackTransform;

    public bool isClicked;

    public void OnClick()
    {
        isClicked = true;

        backbackButton.interactable = false;

        backpackTransform.localScale = new Vector3(1, 1, 1);

        resourcesPanel.SetActive(true);

        FindObjectOfType<InventoryManager>().backpackButton.SetActive(true);
    }

    public void MouseEnter()
    {

        if (isClicked == false)
        {
            backpackTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

            if (resourcesPanel.activeInHierarchy == false)
            {
                resourcesPanel.SetActive(true);
            }
        }
    }

    public void MouseExit()
    {
        if (isClicked == false)
        {
            backpackTransform.localScale = new Vector3(1, 1, 1);

            if (resourcesPanel.activeInHierarchy == true)
            {
                resourcesPanel.SetActive(false);
            }
        }
    }
}
