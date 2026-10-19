using UnityEngine;

public class CraftingTable : MonoBehaviour, IInteractable
{
    [Header("Player")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private CameraMove mouseLook;
    [SerializeField] private PlayerMovement player;

    [Header("UI")]
    [SerializeField] private GameObject craftingWindow;
    private bool hasInteracted;
    private DialogueTrigger objDialogue;

    public string InteractPrompt => gameObject.name;


    public bool Interact()
    {
        if (!craftingWindow.activeInHierarchy)
        {
            craftingWindow.SetActive(true);
            mouseLook.enabled = false;
            player.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }

        else 
        {
            craftingWindow.SetActive(false);
            mouseLook.enabled = true;
            player.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            
        }
        if (TryGetComponent<DialogueTrigger>(out DialogueTrigger trigger) && !hasInteracted)
        {
            objDialogue = trigger;
            objDialogue.EnableTrigger();
            hasInteracted = true;
        }
        return true;
    }
}
