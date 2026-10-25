using UnityEngine;
using static UnityEngine.Rendering.STP;

public class Stalker : MonoBehaviour, IInteractable
{
    public string InteractPrompt => gameObject.name;

    [SerializeField] private DialogueTrigger dialogueTrigger;
    public bool Interact()
    {
        dialogueTrigger.enabled = true;
        return true;
    }
}
