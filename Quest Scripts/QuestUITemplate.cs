using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUITemplate : MonoBehaviour
{
    OptionalQuestObject quest;

    public Image background;

    public Image icon;

    public TextMeshProUGUI text;

    public Button button;

    private void Start()
    {
        Debug.Log(text.text);
    }

    private void Update()
    {
        quest.CheckIfCompleted();

        if (quest.rewardClaimed)
        {
            Destroy(this.gameObject);   
        }
    }

    public void OnClickButton()
    {
        if (button.onClick != null)
        {
            FindObjectOfType<OptionalQuestSystemUI>().OpenQuestDescPanel();
            FindObjectOfType<QuestDescriptionUI>().InitializePanel(quest.description, quest.nameOfOptionalQuest, quest);
        }
        else
        {
            return;
        }
    }

    public void AssignQuest(OptionalQuestObject quest)
    {
        this.quest = quest;
        icon.sprite = quest.icon;
    }

    public void ChangeDescription(string s)
    {
        text.text = s;
    }
}
