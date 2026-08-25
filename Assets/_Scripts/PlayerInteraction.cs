
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
    [SerializeField] private GameObject craftingWindow;

    [Header("Inventory Management")]
    [SerializeField] private InventorySystem inventory;
    private int buttonCounter = 2;
    private GameObject currentInteractable;
    private GameObject currentItem;
    private PlayerMovement player;
    private TrapPlacement placementPoint;

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
            interactText.enabled = true;
            interactText.text = currentInteractable.name;

            if (currentInteractable.CompareTag("Radar") && buttonCounter == 0) 
            {
                currentInteractable.GetComponent<Radar>().showRadar();
                interactText.enabled = false;
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
                var itemData = currentInteractable.GetComponent<ItemTrigger>().ItemProperties();
                interactText.enabled = false;
                inventory.AddItem(itemData, itemData.baseQuantity, currentItem);
                buttonCounter = 2;
            }

            if (currentInteractable.CompareTag("Placement Point") && buttonCounter == 0)
            {

                interactText.enabled = false;
                placementPoint = currentInteractable.GetComponent<TrapPlacement>();
                placementPoint.SpawnTrap();
                buttonCounter = 2;
            }
            if (currentInteractable.CompareTag("Crafting Table") && buttonCounter == 0)
            {
                interactText.enabled = false;
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
        }

        else
        {
            interactText.enabled = false;
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
         
            buttonCounter = 0;
        }


    }

}
