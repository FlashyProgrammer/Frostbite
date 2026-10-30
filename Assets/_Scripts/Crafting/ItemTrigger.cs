using UnityEngine;

public class ItemTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Item item;
    [SerializeField] private InventorySystem inventory;
    public int quantity = 1;

    private Rigidbody rb;

    private void Awake()
    {
        if (inventory == null)
        {
            inventory = FindFirstObjectByType<InventorySystem>();
        }
        rb = GetComponent<Rigidbody>();

    }
    public Item ItemProperties()
    {
        return item;
    }

    public string InteractPrompt => item.name;

    public bool Interact()
    {
        inventory.AddItem(item, quantity, this.gameObject);
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            rb.useGravity = false;
        }
    }

}
