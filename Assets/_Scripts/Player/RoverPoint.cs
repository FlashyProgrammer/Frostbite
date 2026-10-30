using UnityEngine;

public class RoverPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject roverManager;

    [SerializeField] private float roverCooldownTime;

    private static bool isActive = false;

    private float timeCounter;

    private bool startCooldown;

    public string InteractPrompt => gameObject.name;
    public bool Interact()
    {

        if (roverManager.TryGetComponent<Rover>(out Rover rover))
        {
            if (rover.CanActivate() && !isActive)
            {
                rover.Activate();
                isActive = true;
            }

            else
            {
                rover.Deactivate();
                isActive = false;
            }

        }

        return true;
    }
}
