using UnityEngine;
public interface IInteractable
{
    string InteractPrompt { get; }
    bool Interact(Transform interactor);
}
