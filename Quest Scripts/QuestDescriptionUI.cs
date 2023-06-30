using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDescriptionUI : MonoBehaviour
{
    public OptionalQuestObject quest;

    public TMP_Text description;
    public TMP_Text title;

    public void InitializePanel(string d, string t, OptionalQuestObject q)
    {
        description.text = d;
        title.text = t;
        quest = q;
    }

    public void Activate()
    {
        List<OptionalQuestObject> tempList = new List<OptionalQuestObject>();
        tempList = FindObjectOfType<QuestsManager>().activeQuestsList();

        foreach (var temp in tempList)
        {
            temp.isActive = false;
        }

        quest.isActive = true;
        FindObjectOfType<QuestsManager>().GetCurrentActiveQuest(quest);
        FindObjectOfType<OptionalQuestSystemUI>().GetNextOptionalQuest(quest);
        FindObjectOfType<OptionalQuestSystemUI>().hasActiveQuest = true;
        FindObjectOfType<OptionalQuestSystemUI>().activeQuest = quest;
        FindObjectOfType<ClaimReward>().quest = quest;
        FindObjectOfType<ClaimReward>().UpdateButton();
    }
}
