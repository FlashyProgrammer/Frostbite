using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue[] dialogue;
    private int dialogueCount;
    
    private void Start()
    {
        TriggerDialogue();
    }

    private void OnEnable()
    {
        TriggerDialogue();
    }

    public void EnableTrigger()
    {
        this.gameObject.GetComponent<DialogueTrigger>().enabled = true;
    }
    public void DisableTrigger()
    {
        this.gameObject.GetComponent<DialogueTrigger>().enabled = false;
    }

    public void TriggerDialogue()
    {
            FindAnyObjectByType<DialogueManager>().BeginDialogue(dialogue[dialogueCount]);
    }

    public void nextDialogue()
    {
        dialogueCount++;

        if (dialogueCount < dialogue.Length)
        {
            FindAnyObjectByType<DialogueManager>().BeginDialogue(dialogue[dialogueCount]);
        }
        else 
        {
            dialogueCount = 0;
        }

    }
}
