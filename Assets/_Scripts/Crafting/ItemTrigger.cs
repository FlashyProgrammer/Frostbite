using UnityEngine;

public class ItemTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Item item;
    [SerializeField] private InventorySystem inventory;
    public int quantity = 1;

    private void Awake()
    {
        if (inventory == null)
        {
            inventory = FindFirstObjectByType<InventorySystem>();
        }

    }   
    public Item ItemProperties()
    {
        return item;
    }

    public string InteractPrompt => item.name;
    public bool ShowsUI()
    {
        return false;
    }

    public bool Interact()
    {
        inventory.AddItem(item, quantity, this.gameObject);
        return true;
    }

}
