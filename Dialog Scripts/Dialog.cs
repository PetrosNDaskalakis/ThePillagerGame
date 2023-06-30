using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "New Dialog")]
public class Dialog : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField][TextArea] public string[] dialog;

    [SerializeField] public OptionalQuestObject quest;

    public MainQuestObject mainQuest;

    public bool hasMainQuest;

    public bool hasQuest;

    public bool isShop;
}
