using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsButton : MonoBehaviour
{
    public Button questsButton;

    public GameObject resourcesPanel;

    public RectTransform questsTransform;

    public bool isClicked;

    public void OnClick()
    {
        isClicked = true;

        questsButton.interactable = false;

        questsTransform.localScale = new Vector3(1, 1, 1);

    }

    public void MouseEnter()
    {

        if (isClicked == false)
        {
            questsTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

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
            questsTransform.localScale = new Vector3(1, 1, 1);

            if (resourcesPanel.activeInHierarchy == true)
            {
                resourcesPanel.SetActive(false);
            }
        }
    }
}
