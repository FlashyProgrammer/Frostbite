
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] private Camera playerCam;
    [SerializeField] private CameraMove mouseLook;
    [SerializeField] private LayerMask interactionLayers;
    [SerializeField] private float rayDistance;

    [SerializeField] private Transform followPoint;
    [SerializeField] private Transform dropPoint;

    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private GameObject craftingWindow;

    [Header("Inventory Management")]
    [SerializeField] private InventorySystem inventory;
    private int buttonCounter = 2;
    private GameObject currentInteractable;
    private GameObject currentItem;
    private PlayerMovement player;
    private TrapPlacement placementPoint;

    [Header("Dialogue Interactions")]
    [SerializeField] private GameObject radarInteractions;
    [SerializeField] private GameObject craftingInteractions;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        ObjectInteractions();
        RayCasting();
    }

    private void RayCasting()
    {
        Ray camRay = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, rayDistance, interactionLayers))
        {
            currentInteractable = hit.collider.gameObject;
        }
        else
        {
            currentInteractable = null;
        }
        Debug.DrawRay(camRay.origin, camRay.direction * rayDistance, Color.yellow);
    }

    private void ObjectInteractions()
    {

        if (currentInteractable != null)
        {
            interactText.text = currentInteractable.name;
            interactPrompt.SetActive(true);

            if (currentInteractable.CompareTag("Radar") && buttonCounter == 0) 
            {
                currentInteractable.GetComponent<Radar>().showRadar();
                if (radarInteractions.TryGetComponent<DialogueTrigger>(out DialogueTrigger trigger)) trigger.EnableTrigger();
                interactPrompt.SetActive(false);
                mouseLook.enabled = false;
                player.enabled = false;
            }
            if (currentInteractable.CompareTag("Radar") && buttonCounter == 2)
            {
                currentInteractable.GetComponent<Radar>().hideRadar();
                mouseLook.enabled = true;
                player.enabled = true; 
            }


            if (currentInteractable.CompareTag("Item") && buttonCounter == 0)
            {
                currentItem = currentInteractable;
                var worldData = currentInteractable.GetComponent<ItemTrigger>();
                var amount = worldData != null ? worldData.quantity : 1;
                interactPrompt.SetActive(false);
                inventory.AddItem(worldData.ItemProperties(), amount, currentItem);
                buttonCounter = 2;
            }

            if (currentInteractable.CompareTag("Placement Point") && buttonCounter == 0)
            {

                interactPrompt.SetActive(false);
                placementPoint = currentInteractable.GetComponent<TrapPlacement>();
                placementPoint.ActivateTrap();
                buttonCounter = 2;
            }
            if (currentInteractable.CompareTag("Crafting Table") && buttonCounter == 0)
            {
                if (craftingInteractions.TryGetComponent<DialogueTrigger>(out DialogueTrigger trigger)) trigger.EnableTrigger();
                interactPrompt.SetActive(false);
                craftingWindow.SetActive(true);
                mouseLook.enabled = false;
                player.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
            if (currentInteractable.CompareTag("Crafting Table") && buttonCounter == 2)
            {
                craftingWindow.SetActive(false);
                mouseLook.enabled = true;
                player.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }

            if(currentInteractable.CompareTag("Item Spawner") && buttonCounter == 0)
            {
                interactPrompt.SetActive(true);
                currentInteractable.GetComponent<ItemSpawner>().SpawnItem();
                buttonCounter = 2;
            }
        }

        else
        {
            interactPrompt.SetActive(false);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && buttonCounter == 0 && currentInteractable != null)
        {
            buttonCounter++;
        }

        if(context.canceled && buttonCounter == 1 && currentInteractable != null)
        {
            buttonCounter++;
        }

        if (context.performed && buttonCounter == 2 && currentInteractable != null)
        {
            if (currentInteractable.TryGetComponent<DialogueTrigger>(out DialogueTrigger trigger)) trigger.EnableTrigger();
            buttonCounter = 0;
        }


    }

}
