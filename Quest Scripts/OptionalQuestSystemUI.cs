using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionalQuestSystemUI : MonoBehaviour
{
    public QuestsManager manager;

    private void Start()
    {
        manager = FindObjectOfType<QuestsManager>();
    }

    //Images
    public Image mainQuestIcon;
    public Image sideQuestIcon;
    public Sprite sideQuestActiveIcon;
    //Show the quests panel
    public Button QuestButton;
    public Button BackToQuestsButton;
    public Button backToButton;
    public Button ShowMoreButton;

    //UI Panels
    public GameObject QuestPanel;
    public GameObject QuestListPanel;
    public GameObject QuestDescPanel;
    public GameObject QuestTakenPanel;
    public GameObject QuestCompletedPanel;
    public GameObject QuestIsActivePanel;
    public GameObject QuestInProgressPanel;

    //UI Texts
    public TMP_Text questCompletedText;
    public TMP_Text questIsActiveText;

    public TMP_Text questNameInProgressText;
    public TMP_Text questDetailsInProgressText;
    public TMP_Text questProgressIntText;

    //UI Quest List

    #region UI MECHANICS
    public void ClickButton()
    {
        if (QuestButton.onClick != null)
        {
            OpenQuestPanel();
        }
        else
        {
            return;
        }
    }

    public void OpenQuestPanel()
    {
        QuestPanel.SetActive(true);
        isOpen = true;
    }

    public void CloseQuestPanel()
    {
        QuestPanel.SetActive(false);
        isOpen = false;
    }

    public void OpenQuestsList()
    {
        QuestListPanel.SetActive(true);
    }

    public void OpenQuestDescPanel()
    {
        QuestDescPanel.SetActive(true);
        QuestListPanel.SetActive(false);
    }

    public void ShowMore()
    {
        QuestDescPanel.SetActive(false);
        OpenQuestsList();

    }

    public void BackToButton()
    {
        QuestsButton button = FindObjectOfType<QuestsButton>();
        button.isClicked = false;
        button.questsButton.interactable = true;
        QuestPanel.SetActive(false);
        QuestListPanel.SetActive(false);
        QuestPanel.SetActive(false);
        QuestDescPanel.SetActive(false);
    }

    public void BackToQuestPanel()
    {
        QuestListPanel.SetActive(false);
    }

    public void BackToActivatedQuests()
    {
        QuestDescPanel.SetActive(false);
        QuestListPanel.SetActive(true);
    }

    public IEnumerator ShowQuestTakenSign()
    {
        QuestTakenPanel.SetActive(true);

        yield return new WaitForSeconds(2);

        QuestTakenPanel.SetActive(false);
    }

    public void OpenQuestDetails()
    {
        QuestListPanel.SetActive(true);
    }

    public void ShowQuestInProgress(OptionalQuestObject quest)
    {
        QuestInProgressPanel.SetActive(true);

        questNameInProgressText.text = quest.nameOfOptionalQuest;
        questDetailsInProgressText.text = quest.CheckDescription().ToString();
        questProgressIntText.text = quest.CheckAmountOfObjectsLeft().ToString() + "/" + quest.CheckAmountOfObjectsToCounter().ToString();
        

        if(quest.CheckAmountOfObjectsLeft() == quest.CheckAmountOfObjectsToCounter())
        {
            quest.isCompleted = true;
            PlayerPrefs.SetInt("isCompleted", (quest.isCompleted ? 1 : 0));
            QuestInProgressPanel.SetActive(false);
        }
    }

    public IEnumerator ShowQuestIsActive(OptionalQuestObject quest)
    {
        QuestIsActivePanel.SetActive(true);

        questIsActiveText.text = quest.nameOfOptionalQuest + " IS ACTIVE !";

        yield return new WaitForSeconds(2);

        QuestIsActivePanel.SetActive(false);
    }
    
    public IEnumerator ShowQuestCompletedSign(OptionalQuestObject quest)
    {
        QuestCompletedPanel.SetActive(true);

        questCompletedText.text = quest.nameOfOptionalQuest + " WAS COMPLETED !";

        yield return new WaitForSeconds(4);

        questCompletedText.text = "+ " + quest.CheckGoldReward().ToString() + " GOLD" + 
            "\n" + "+ " + quest.CheckGemsReward().ToString() + " GEMS";

        yield return new WaitForSeconds(4);

        QuestCompletedPanel.SetActive(false);
    }
    #endregion

    //UI Quests Stats
    public TMP_Text questName;
    public TMP_Text optionalQuestName;

    public TMP_Text mainQuestDesc;

    //This a list of slots
    public List<QuestUITemplate> slot = new List<QuestUITemplate>();

    //Cache the template
    public QuestUITemplate temp;

    //Cache the panel
    public GameObject panel;

    //We are using a stack so when the player is getting a quest
    //the oldest quest is staying on top
    //and the newest quets is going on the bottom
    [SerializeField] public Stack<string> OptionalQuestName = new Stack<string>();
    [SerializeField] public Stack<string> OptionalQuestDesc = new Stack<string>();

    public bool hasActiveQuest = false;

    public OptionalQuestObject activeQuest;

    public bool isOpen = false;

    private void Update()
    {
        GetNextMainQuest();

        GetMainQuestDescription();

        if(hasActiveQuest == true)
        {
            ShowQuestInProgress(activeQuest);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            InventorySystemUI inventoryUI = FindObjectOfType<InventorySystemUI>();

            if(isOpen == false)
            {
                OpenQuestPanel();
                if (inventoryUI.isOpen == true)
                {
                    inventoryUI.CloseInventoryPanel();
                    inventoryUI.isOpen = false;
                }
                isOpen = true;
            }
            else
            {
                CloseQuestPanel();
                isOpen = false;
            }
        }

    }

    public void GetNextMainQuest()
    {
        //When one main quest is completed
        //get the next one from the list

        manager.GetQuest();

        foreach(var stats in manager.GetQuest().ToList())
        { 
            for (int i = 0; i < manager.GetQuest().Count; i++)
            {
                if (!stats.isCompleted)
                {
                    questName.text = stats.nameOfMainQuest;
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public void GetMainQuestDescription()
    {
        manager.GetQuest();

        foreach (var stats in manager.GetQuest().ToList())
        {
            if (!stats.isCompleted)
            {
                mainQuestDesc.text = stats.description;
                mainQuestIcon.sprite = stats.icon;
            }
        }
    }

    public void GetNextOptionalQuest(OptionalQuestObject stats)
    {
        //If the player selects an optional Quest
        //then swap the optional quest slot
        //or if its empty then put it in the active quests list...

        //Remove From The Other Lists...
        if (stats.rewardClaimed)
        {
            manager.GetOptionalQuest().Remove(stats);
            optionalQuestName.text = "EMPTY";
        }

        manager.GetOptionalQuest();

        if (stats.isTaken && manager.GetOptionalQuest().Contains(stats))
        {
            for (int i = 0; i < manager.GetOptionalQuest().Count; i++)
            {
                if (stats.isActive)
                {
                    if (!stats.rewardClaimed)
                    {
                        optionalQuestName.text = stats.nameOfOptionalQuest;
                        sideQuestIcon.sprite = sideQuestActiveIcon;
                    }
                }
            }
        }

    }

    public void UpdateSlots()
    {
        foreach(OptionalQuestObject quest in manager.Oquests)
        {
            if (quest.isTaken)
            {
                //the item is getting pushed in the stack
                OptionalQuestName.Push(quest.nameOfOptionalQuest);
                OptionalQuestDesc.Push(quest.description);
                //we are creating a new slot which is a prefab
                QuestUITemplate newQuest = Instantiate(temp, panel.transform);
                //we add the quest in the slot
                slot.Add(newQuest);
                //the string description is equals to the quests description
                name = quest.nameOfOptionalQuest;
                //we are calling the methods to assign the quest in the slot
                //and we are changing the description

                Debug.Log(name);
                newQuest.ChangeDescription(name);
                newQuest.AssignQuest(quest);
            }
        }
    }

    public void GetNameInTheList(OptionalQuestObject quest)
    {
        Debug.Log("Hello");

        //we want a string because we assing the string in the ChangeDescription() method
        string name;

        //we are checking if the stack already contains the quest
        if (!OptionalQuestName.Contains(quest.nameOfOptionalQuest))
        {
            //we are checking if the quest is completed or not
            if (!quest.rewardClaimed)
            {
                //the item is getting pushed in the stack
                OptionalQuestName.Push(quest.nameOfOptionalQuest);
                OptionalQuestDesc.Push(quest.description);
                //we are creating a new slot which is a prefab
                QuestUITemplate newQuest = Instantiate(temp, panel.transform);
                //we add the quest in the slot
                slot.Add(newQuest);
                //the string description is equals to the quests description
                name = quest.nameOfOptionalQuest;
                //we are calling the methods to assign the quest in the slot
                //and we are changing the description

                Debug.Log(name);
                newQuest.ChangeDescription(name);
                newQuest.AssignQuest(quest);

                name = string.Empty;
            }
            //if the quest is getting completed it removes it from the stack
            else if (quest.rewardClaimed)
            {
                OptionalQuestName.Pop();
            }
        }
    }

    public void GetOptionalQuestDescription(OptionalQuestObject item)
    {
        if (item.isTaken == true)
        {
            Debug.Log(item.name + " is taken");

            PlayerPrefs.SetInt("isTaken", (item.isTaken ? 1 : 0));

            GetNameInTheList(item);
        }
        else
        {
            return;
        }
    }
}
