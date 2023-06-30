using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogUI : MonoBehaviour
{
    //We need a dialogue box to enter the text...
    [SerializeField] private GameObject dialogueBox;
    //And a text label to call inside the dialogue box.
    //and a float for the typing speed...
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private float typeWriterSpeed = 50;

    //A bool to know if the box is visible in the user or not
    //And to change the state of the dialogueBox...
    public bool isOpen = false;

    public void ShowDialogue(Dialog dialogObject)
    {
        isOpen = true;

        dialogueBox.SetActive(true);

        StartCoroutine(TypeDialogue(dialogObject));
    }

    //A void so we make the dialogue box visible
    public IEnumerator TypeDialogue(Dialog dialogObject)
    {
        foreach(string dialog in dialogObject.dialog)
        {
            yield return Run(dialog, textLabel);

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        }
        
        CloseDialogueBox();

        if(dialogObject.hasQuest == true)
        {
            dialogObject.quest.CheckIfTaken();
            dialogObject.quest.isTaken = true;
            FindObjectOfType<QuestsManager>().CacheOptionalQuests(dialogObject.quest);
        }
        
        if(dialogObject.isShop == true)
        {
            SceneManager.LoadScene("Shop");
        }

        if(dialogObject.hasMainQuest == true)
        {
            dialogObject.mainQuest.isCompleted = true;
        }
    }

    public void CloseDialogueBox()
    {
        isOpen = false;

        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }



    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * typeWriterSpeed;

            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);
            yield return null;
        }

        textLabel.text = textToType;
    }

}
