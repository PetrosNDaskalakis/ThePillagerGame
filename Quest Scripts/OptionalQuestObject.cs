using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "oQ", menuName = "OptionalQuestObject/oQ")]
public class OptionalQuestObject : ScriptableObject
{
    //The Name of the Optional quest
    public string nameOfOptionalQuest;

    //The ID of the Quest
    public int id;

    public int GetId()
    {
        return id;
    }

    public Sprite icon;

    // A array of Quest Types
    public string[] questTypes = {"Kill", "Obtain"};

    public string[] CheckQuestType()
    {
        return questTypes;
    }

    // Write the description of the Quest
    [TextArea] public string description;

    public string CheckDescription()
    {
        return description;
    }

    //Deside which objects will the player have to face for the quests
    public string[] objectToCounter;
    //Amount of objects that you have to counter
    public int amountOfObjectsToCounter;
    public int amountOfObjectsKilled;

    public string[] CheckObjectsToCounter()
    {
        return objectToCounter;
    }

    public int CheckAmountOfObjectsToCounter()
    {
        return amountOfObjectsToCounter;
    }

    public int CheckAmountOfObjectsLeft()
    {
        return amountOfObjectsKilled;
    }

    //A bool to check if the reward is claimed...
    public bool rewardClaimed = false;

    public bool CheckIfRewardClaimed()
    {
        return rewardClaimed;
    }

    //The amount and the type of the reward...
    //It can be a weapon which we call it with a string e.g ("Sword")...
    //Or it can be something like "money" reward where we call it with an int...
    public int goldReward;

    public int gemsReward;

    public int CheckGemsReward()
    {
        return gemsReward;
    }

    public int CheckGoldReward()
    {
        return goldReward;
    }

    //Check if the optional quest is taken
    public bool isTaken = false;

    public bool CheckIfTaken()
    {
        return isTaken;
    }

    //Check if the optional quest is activated
    public bool isActive = false;

    public bool CheckIfActive()
    {
        return isActive;
    }

    //A bool to check if its finished...
    public bool isCompleted = false;

    public bool CheckIfCompleted()
    {
        return isCompleted;
    }
}
