using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue[] dialogue;
    [SerializeField] private bool isOneTime;
    private int dialogueCount = 0;

    private void OnEnable()
    {
        if(isOneTime) TriggerDialogue();
    }
    private void Start()
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
