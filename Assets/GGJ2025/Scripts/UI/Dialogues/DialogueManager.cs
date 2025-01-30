using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField]
    private DialogueDatabase dialogueDatabase;
    [SerializeField]
    private TextMeshProUGUI text;

    private DialogueEntry currentDialogue;
    private uint currentLevel;
    private bool firstTime;

    private void Start()
    {
        LevelManager.Get().OnStart += OnStart;
        if (!firstTime)
        {
            OnStart();
            firstTime = true;
        }
    }

    private void OnStart()
    {
        gameObject.SetActive(true);

        currentLevel = LevelManager.Get().Level;
        currentDialogue = GetLevelEntry();
        if (currentDialogue == null)
        {
            gameObject.SetActive(false);
            return;
        }
        DisplayEntry();
    }

    private void DisplayEntry()
    {
        text.text = string.Format(currentDialogue.Dialogue_Text);
    }

    private DialogueEntry GetLevelEntry()
    {
        switch (currentLevel)
        {
            case 1:
                return dialogueDatabase.GetEntry(currentLevel, 0);
            case 2:
                return dialogueDatabase.GetEntry(currentLevel, 0);
            case 3:
                return dialogueDatabase.GetEntry(currentLevel, 0);
            case 5:
                return dialogueDatabase.GetEntry(currentLevel, 0);
            case 10:
                return dialogueDatabase.GetEntry(currentLevel, 0);
            case 15:
                return dialogueDatabase.GetEntry(currentLevel, 0);
        }
        return null;
    }
}
