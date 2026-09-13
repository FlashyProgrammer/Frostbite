using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Character[] characterList;
    private int characterCount;

    private void Start()
    {
        TriggerDialogue();
    }

    public void EnableTrigger()
    {
        this.gameObject.GetComponent<DialogueTrigger>().enabled = true;
    }
    public void TriggerDialogue()
    {
            FindAnyObjectByType<DialogueManager>().BeginDialogue(characterList[characterCount]);
    }

    public void nextCharacter()
    {
        characterCount++;

        if (characterCount < characterList.Length)
        {
            FindAnyObjectByType<DialogueManager>().BeginDialogue(characterList[characterCount]);
        }
        else 
        {
            characterCount = 0;
        }

    }
}
