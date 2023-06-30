using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestsManager : MonoBehaviour
{
    private void Update()
    {
        CompletedMainQuests();

        ClaimMainReward();
    }

    private void Awake()
    {
        CacheMainQuests();
        FindOptionalQuests();
    }

    private void Start()
    {
        SortByType();
    }

    #region MAIN QUEST
    //A list with all the quests
    public List<MainQuestObject> Quests = new List<MainQuestObject>();

    public List<MainQuestObject> GetQuest()
    {
        return Quests;
    }

    public void CacheMainQuests()
    {
        MainQuestObject[] allQuests;

        allQuests = Resources.LoadAll<MainQuestObject>("Main");

        Quests.AddRange(allQuests);

        if(Quests.Count > 0)
        {
            Debug.Log(Quests.Count + " ITEM(S) WAS FOUND");
        }
        else
        {
            Debug.Log("NO ITEMS FOUND");
            Debug.Log(Quests.Count);
        }
    }

    //Another List For The Completed Quests...
    List<MainQuestObject> finished = new List<MainQuestObject>();

    public List<MainQuestObject> GetFinishedQuest()
    {
        return finished;
    }

    public void CompletedMainQuests()
    {
        foreach (MainQuestObject quest in Quests.ToList())
        {
            quest.CheckIfCompleted();

            //Check if quest is completed
            if (quest.isCompleted == true)
            {
                //Remove From The Other Lists...
                Quests.Remove(quest);

                //And Cache To The Finished List.
                finished.Add(quest);
            }
            else
            {
                return;
            }
        }
    }

    //Check if the user got the reward
    public void ClaimMainReward()
    {
        foreach (MainQuestObject quest in Quests.ToList())
        {
            quest.CheckIfRewardClaimed();

            if (finished.Contains(quest) && quest.rewardClaimed == false)
            {
                //TODO: Give the user the reward
                // example (player.GetReward();) 

                //Change the boolean to true
                quest.rewardClaimed = true;
            }
            else
            {
                return;
            }
        }
    }
    #endregion

    #region OPTIONAL QUEST
    //A list with all the quests
    public List<OptionalQuestObject> Oquests = new List<OptionalQuestObject>();

    public List<OptionalQuestObject> GetOptionalQuest()
    {
        return Oquests;
    }

    public void FindOptionalQuests()
    {
        OptionalQuestObject[] allQuests;

        allQuests = Resources.LoadAll<OptionalQuestObject>("Optional");

        foreach(OptionalQuestObject quest in allQuests)
        {
            Oquests.Add(quest);
        }

        if (Oquests.Count > 0)
        {
            Debug.Log(Oquests.Count + " ITEM(S) WAS FOUND");
        }
        else
        {
            Debug.Log("NO ITEMS FOUND");
            Debug.Log(Oquests.Count);
        }
    }

    public void CacheOptionalQuests(OptionalQuestObject quest)
    {
        quest.CheckIfTaken();

        if (quest.isTaken)
        {
            if (Oquests.Contains(quest))
            {
                FindObjectOfType<OptionalQuestSystemUI>().GetOptionalQuestDescription(quest);
                StartCoroutine(FindObjectOfType<OptionalQuestSystemUI>().ShowQuestTakenSign());
            }
            else
            {
                //TODO: NPC TALK
            }
        }
    }

    //Two lists to sort by the quests by type
    public List<OptionalQuestObject> OquestKill = new List<OptionalQuestObject>();
    public List<OptionalQuestObject> OquestObtain = new List<OptionalQuestObject>();

    public List<OptionalQuestObject> GetKillQuest()
    {
        return OquestKill;
    }

    public List<OptionalQuestObject> GetObtainQuest()
    {
        return OquestObtain;
    }

    public void SortByType()
    {
        foreach (OptionalQuestObject quest in Oquests.ToList())
        {
            quest.CheckQuestType();

            var type = string.Empty;

            foreach(var item in quest.CheckQuestType())
            {
                type += item;
            }

            // Check the Quest Type from the string[] in Quests class
            if (type == "Kill")
            {
                //Cache The Quests by Type
                OquestKill.Add(quest);
            }
            else if(type == "Obtain")
            {
                OquestObtain.Add(quest);
            }
            else if(type != "Kill" && type != "Obtain" || type == null)
            {
                //
            }
        }

    }

    //Another List For The Completed Quests...
    List<OptionalQuestObject> OQuestfinished = new List<OptionalQuestObject>();
    
    public List<OptionalQuestObject> GetFinishedOQuests()
    {
        return OQuestfinished;
    }

    public void CompletedOptionalQuests(OptionalQuestObject quest)
    {
        quest.CheckIfCompleted();

        //Check if quest is completed
        if (quest.isCompleted == true)
        {
            if (!OQuestfinished.Contains(quest))
            {
                //And Cache To The Finished List.
               
                StartCoroutine(FindObjectOfType<OptionalQuestSystemUI>().ShowQuestCompletedSign(quest));
            }
            else
            {
                return;
            }
        }
        else
        {
            //
        }
    }

    //Check if the user got the reward
    public void ClaimOptionalReward(OptionalQuestObject quest)
    {
        CompletedOptionalQuests(quest);
        OQuestfinished.Add(quest);

        if (OQuestfinished.Contains(quest) && quest.rewardClaimed == false)
        {
            int gems = quest.CheckGemsReward();
            int gold = quest.CheckGoldReward();

            //TODO: Give the user the reward
            Managers.StorageManager.AddGold(gold);
            Managers.StorageManager.AddGems(gems);

            //Change the boolean to true
            quest.rewardClaimed = true;
            PlayerPrefs.SetInt("rewardClaimed", (quest.rewardClaimed ? 1 : 0));

            quest.isActive = false;
            PlayerPrefs.SetInt("isActive", (quest.isActive ? 1 : 0));
            quest.isTaken = false;
            PlayerPrefs.SetInt("isTaken", (quest.isTaken ? 1 : 0));

            FindObjectOfType<ClaimReward>().quest = null;
            FindObjectOfType<ClaimReward>().claim.interactable = false;
            FindObjectOfType<OptionalQuestSystemUI>().hasActiveQuest = false;
            FindObjectOfType<OptionalQuestSystemUI>().activeQuest = null;
            FindObjectOfType<OptionalQuestSystemUI>().GetNextOptionalQuest(quest);
        }
        else
        {
            return;
        }
    }

    public List<OptionalQuestObject> activeQuest = new List<OptionalQuestObject>();

    public List<OptionalQuestObject> activeQuestsList()
    {
        return activeQuest;
    }

    public void UpdateQuest(OptionalQuestObject quest)
    {
        quest.amountOfObjectsKilled += 1;

        if (quest.amountOfObjectsKilled == quest.amountOfObjectsToCounter)
        {
            quest.isCompleted = true;
            PlayerPrefs.SetInt("isCompleted", (quest.isCompleted ? 1 : 0));
        }
    }

    public void GetCurrentActiveQuest(OptionalQuestObject quest)
    {
        quest.CheckIfActive();

        if (quest.isActive)
        {
            PlayerPrefs.SetInt("isActive", (quest.isActive ? 1 : 0));

            if (!activeQuest.Contains(quest))
            {
                activeQuest.Add(quest);

                StartCoroutine(FindObjectOfType<OptionalQuestSystemUI>().ShowQuestIsActive(quest));
            }
            else
            {
                //
            }
        }
    }
    #endregion
}
