
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
    private GameObject currentInteractable;

    [Header("Dialogue Interactions")]
    [SerializeField] private GameObject radarInteractions;
    [SerializeField] private GameObject craftingInteractions;

    private PlayerMovement player;
    private bool isUIShown;
    private int buttonCounter;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
    }
    void Update()
    {
       
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

        if (currentInteractable != null && player.enabled)
        {
            interactPrompt.SetActive(true);
            interactText.text = currentInteractable.name;
        }
        else
        {
            interactPrompt.SetActive(false);
        }

    }

    private void ObjectInteractions()
    {
        if (currentInteractable.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactText.text = interactable.InteractPrompt;
            interactable.Interact();
        }
       
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && currentInteractable != null)
        {
            ObjectInteractions();

        }

       

    }

}
