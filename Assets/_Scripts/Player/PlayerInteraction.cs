
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction : MonoBehaviour
{

    [Header("Misc")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private CameraMove mouseLook;
    [SerializeField] private LayerMask interactionLayers;

    [SerializeField] private Transform followPoint;
    [SerializeField] private Transform dropPoint;

    [Header("Collision Parameters")]
    [SerializeField] private Transform playerEyes;
    [SerializeField] private float sphereRadius;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private GameObject craftingWindow;

    [Header("Inventory Management")]
    [SerializeField] private InventorySystem inventory;
    private GameObject currentInteractable;
    private PlayerMovement player;
    private Collider[] hitBuffer = new Collider[20];
    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        if (currentInteractable != null)
        {
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }

    }

    private void OnDisable()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }
    void Update()
    {

        RayCasting();
    }

    private void RayCasting()
    {
        int hitCount = Physics.OverlapCapsuleNonAlloc(Camera.main.transform.position, playerEyes.position, sphereRadius, hitBuffer, interactionLayers);
        if (hitCount != 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                currentInteractable = hitBuffer[i].gameObject;
            }
        }

        else
        {
            currentInteractable = null;
        }

        if (currentInteractable != null && player.enabled)
        {
            interactPrompt.SetActive(true);

            if (currentInteractable.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactText.text = interactable.InteractPrompt;

            }
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
