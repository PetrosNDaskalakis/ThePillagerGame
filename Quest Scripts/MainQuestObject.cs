using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "mQ", menuName = "MainQuestObject/mQ")]
public class MainQuestObject : ScriptableObject
{
    //The quest object have a reference on what type of quest it is...
    //An amount of reward, a description and its called in the QuestsManager.cs...
    //Where its doing everything automaticly...

    //The Name of the quest
    public string nameOfMainQuest;

    public Sprite icon;

    public virtual string CheckName()
    {
        return nameOfMainQuest; 
    }

    // Write the description of the Quest
    [TextArea] public string description;

    public virtual string GiveDescription()
    {
        return description;
    }

    //Deside which objects will the player have to face for the quests
    public string[] objectToCounter;

    public string[] CheckObjectToCounter()
    {
        return objectToCounter;
    }

    //Amount of objects that you have to counter
    public int[] amountOfObjectsToCounter;

    public int[] CheckAmountOfObjectsToCounter()
    {
        return amountOfObjectsToCounter;
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
    public string[] objectReward;
    public int currencyReward;
    
    public string[] CheckObjectReward()
    {
        return objectReward;
    }

    public int CheckCurrencyReward()
    {
        return currencyReward;
    }
    
    //A bool to check if its finished...
    public bool isCompleted = false;

    public bool CheckIfCompleted()
    {
        return isCompleted;
    }
}
