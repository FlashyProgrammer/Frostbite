using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slots;
    [SerializeField] private List<Image> slotImages;
    [SerializeField] private List<Image> activeSlotImages;
    [SerializeField] private List<TextMeshProUGUI> numberText;
    [SerializeField] private int maxInventorySize = 5;
    [SerializeField] private bool wrapAround = true;

    [SerializeField] private Transform followObject;
    [SerializeField] private Transform groundPoint;

    private int currentIndex;

    private void Awake()
    {
        maxInventorySize = Mathf.Max(1, maxInventorySize);

        while (slots.Count < maxInventorySize)
        {
            slots.Add(new InventorySlot());
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < numberText.Count) slots[i].SetText(numberText[i]);
            if (i < slotImages.Count) slots[i].SetImage(slotImages[i]);
            UpdateSlotUI(i);
        }

        RefreshSelection();
    }

    private void Update()
    {
        MouseItemScroll();
    }


    public bool AddItem(Item itemToAdd, int amount, GameObject itemObject)
    {
        if (itemToAdd == null || itemObject == null || amount <= 0) return false;

        int maxPerSlot = Mathf.Max(1, itemToAdd.canStack ? itemToAdd.stackLimit : itemToAdd.baseQuantity);

        
        if (!CanFit(itemToAdd, amount, maxPerSlot))
        {
            Debug.LogWarning("Inventory full! Could not fit: " + amount);
            return false;
        }

        int remaining = amount;

        
        if (itemToAdd.canStack)
        {
            for (int i = 0; i < slots.Count && remaining > 0; i++)
            {
                InventorySlot stackSlot = slots[i];
                if (stackSlot.IsEmpty || stackSlot.item != itemToAdd || stackSlot.IsFull) continue;

                remaining = stackSlot.AddToStack(remaining);
                UpdateSlotUI(i);
            }

            if (remaining <= 0)
            {
                Destroy(itemObject);
                return true;
            }
        }


        bool originalUsed = false;

        // Stacking find empty remaining slots
        while (remaining > 0)
        {
            int index = FindEmptySlotIndex();
            if (index < 0) break; 

            int batch = Mathf.Min(remaining, maxPerSlot);

            
            GameObject obj = originalUsed ? Instantiate(itemObject) : itemObject;
            originalUsed = true;

            GrabObject(obj);
            obj.SetActive(false);

            InventorySlot freeSlot = slots[index];
            freeSlot.item = itemToAdd;
            freeSlot.itemObject = obj;
            freeSlot.quantity = batch;
            UpdateSlotUI(index);

            remaining -= batch;
        }

        RefreshSelection();
        return true;
    }

    private bool CanFit(Item item, int amount, int maxPerSlot)
    {
        int room = 0;

        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty)
            {
                room += maxPerSlot;
            }
            else if (item.canStack && slot.item == item && !slot.IsFull)
            {
                room += Mathf.Max(0, item.stackLimit - slot.quantity);
            }
        }

        return room >= amount;
    }

    private int FindEmptySlotIndex()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty) return i;
        }
        return -1;
    }

    private int FindSlotIndexOf(GameObject obj)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemObject == obj) return i;
        }
        return -1;
    }

  

    public void RemoveItem(GameObject item)
    {
        if (item == null) return;

        
        int index = FindSlotIndexOf(item);
        if (index < 0) index = currentIndex;

        InventorySlot slot = slots[index];

        if (item.TryGetComponent(out ItemTrigger trigger))
        {
            trigger.quantity = slot.quantity;
        }

        item.transform.SetParent(null);
        item.transform.position = (groundPoint.position);
        ReleaseObject(item);
        item.SetActive(true);

        slot.Drop();
        slot.itemObject = null;

        UpdateSlotUI(index);
        RefreshSelection();
    }

    public void DropItem(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        InventorySlot slot = slots[currentIndex];
        if (slot.item == null || slot.itemObject == null) return;

        RemoveItem(slot.itemObject);
    }

    

    public void GrabObject(GameObject obj)
    {
        if (obj == null) return;

        if (obj.TryGetComponent(out Rigidbody rb))
        {
            rb.useGravity = false;
            rb.isKinematic = true; 
        }

        if (obj.TryGetComponent(out MeshCollider col))
        {
            col.isTrigger = true;
        }

        obj.transform.SetParent(followObject);
        obj.transform.position = (followObject.position);
    }

    private void ReleaseObject(GameObject obj)
    {
        if (obj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (obj.TryGetComponent(out MeshCollider col))
        {
            col.isTrigger = false;
        }
    }

    public InventorySlot GetSlot()
    {
        return slots[currentIndex];
    }

    public void NextItem(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeSlot(1);
    }

    public void PreviousItem(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeSlot(-1);
    }

    private void MouseItemScroll()
    {
        if (Mouse.current == null) return;

        float scrollY = Mouse.current.scroll.ReadValue().y;

        if (scrollY > 0f) ChangeSlot(-1);
        else if (scrollY < 0f) ChangeSlot(1);
    }

    private void ChangeSlot(int step)
    {
        if (slots.Count == 0) return;

        int target = currentIndex + step;

        if (wrapAround)
        {
            target = ((target % slots.Count) + slots.Count) % slots.Count;
        }
        else
        {
            target = Mathf.Clamp(target, 0, slots.Count - 1);
        }

        if (target == currentIndex) return;

        currentIndex = target;
        RefreshSelection();
    }

    
    public void RefreshSelection()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            bool selected = i == currentIndex;
            GameObject obj = slots[i].itemObject;

            if (obj != null) obj.SetActive(selected);

            if (i < activeSlotImages.Count && activeSlotImages[i] != null)
            {
                activeSlotImages[i].gameObject.SetActive(selected && obj != null);
            }
        }
    }


    private void UpdateSlotUI(int index)
    {
        InventorySlot slot = slots[index];
        bool empty = slot.IsEmpty;

        if (index < slotImages.Count && slotImages[index] != null)
        {
            slotImages[index].enabled = !empty;

            if (!empty && slot.item != null)
            {
                slotImages[index].sprite = slot.item.itemIcon;
            }
        }

        if (index < numberText.Count && numberText[index] != null)
        {
            numberText[index].text = empty ? string.Empty : slot.quantity.ToString();
        }
    }

    public List<InventorySlot> GetCurrentInventory()
    {
        return slots;
    }
}