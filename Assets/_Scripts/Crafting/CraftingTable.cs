using UnityEngine;

public class CraftingTable : MonoBehaviour, IInteractable
{
    [Header("Player")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private CameraMove mouseLook;
    [SerializeField] private PlayerMovement player;

    [Header("UI")]
    [SerializeField] private GameObject craftingWindow;

    public string InteractPrompt => gameObject.name;

    public bool ShowsUI()
    {
        return true;
    }
   
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
        return true;
    }
}
