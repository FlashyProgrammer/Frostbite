using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slots;
    [SerializeField] private PlayerInteraction playerInteraction;

    [SerializeField] private List<Image> slotImages;
    [SerializeField] private List<Image> activeSlotImages;
    [SerializeField] private List<TextMeshProUGUI> numberText;
    [SerializeField] private int maxInventorySize = 5;

    [SerializeField] private Transform followObject;
    [SerializeField] private Transform groundPoint;
    private int currentIndex = 0;

    private void Awake()
    {
        for (int i = 0; i < maxInventorySize; i++)
        {
            slots.Add(new InventorySlot());  
        }
    }
    private void Update()
    {
        MouseItemScroll();
    }
    public bool AddItem(Item itemToAdd, int amount, GameObject itemObject)
    {
        itemObject.SetActive(false);

        int remainingAmount = amount;

        if (itemToAdd.canStack)
        {
            foreach (var slot in slots)
            {
                slot.SetText(numberText[slots.IndexOf(slot)]);
                slot.SetImage(slotImages[slots.IndexOf(slot)]);

                if (!slot.IsEmpty && slot.item == itemToAdd && !slot.IsFull)
                {
                    remainingAmount = slot.AddToStack(remainingAmount);
                    slot.textAmount.text = slot.quantity.ToString();

                    if (remainingAmount <= 0) 
                    {
                        Destroy(itemObject);
                        return true;
                    }
                    
                }
            }
            
        }


        while (remainingAmount > 0)
        {
            InventorySlot freeSlot = FindEmptySlot();

            if(freeSlot != null)
            {
                freeSlot.item = itemToAdd;
                freeSlot.itemObject = itemObject;
                GrabObject(freeSlot.itemObject);

            }
           
       
            if (freeSlot == null)
            {
                Debug.LogWarning("Inventory Full! Could not fit remaining: " + remainingAmount);
                itemObject.SetActive(true);
                return false; 
            }
          
            int maxAmount = itemToAdd.canStack ? itemToAdd.stackLimit : itemToAdd.baseQuantity;
            int currentBatch = Mathf.Min(remainingAmount, maxAmount);

            freeSlot.quantity = currentBatch;
            remainingAmount -= currentBatch;

            freeSlot.SetText(numberText[slots.IndexOf(freeSlot)]);
            freeSlot.SetImage(slotImages[slots.IndexOf(freeSlot)]);

            freeSlot.itemImage.enabled = true;
            freeSlot.itemImage.sprite = itemToAdd.itemIcon;
            freeSlot.textAmount.text = freeSlot.quantity.ToString();
       

        }

        GrabObject(itemObject);
        return true;
    }

    private InventorySlot FindEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty) return slot;
        }
        return null;
    }

    public void RemoveItem(GameObject item)
    {
        InventorySlot slot = slots[currentIndex];
        int quantityDropped = slot.quantity;

        ItemTrigger trigger = item.GetComponent<ItemTrigger>();

        if (trigger != null)
        {
            trigger.quantity = quantityDropped;
        }

        item.transform.position = groundPoint.position;
        item.transform.rotation = groundPoint.rotation;
        item.SetActive(true);
        item.transform.parent = null;
        slots[currentIndex].Drop();
     

    }
    public InventorySlot GetSlot() 
    {
        activeSlotImages[currentIndex].gameObject.SetActive(false);
        return slots[currentIndex];
  
    }

    public void GrabObject(GameObject obj)
    {
        obj.transform.position = followObject.position;
        obj.transform.parent = followObject;
    }
    public void NextItem(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            if (slots[currentIndex].itemObject != null)
            {
                slots[currentIndex].itemObject.SetActive(false);
            }
            
            currentIndex++;


            if (currentIndex == maxInventorySize)
            {
                currentIndex = slots.Count - 1;
            }


            if (slots[currentIndex].itemObject != null)
            {
                slots[currentIndex].itemObject.SetActive(true);
            }

            else
            {
                currentIndex = 0;
            }
           
        }
    }

    public void PreviousItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (slots[currentIndex].itemObject != null)
            {
                slots[currentIndex].itemObject.SetActive(false);
            }

            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = 0;
            }


            if (slots[currentIndex].itemObject != null)
            {
                slots[currentIndex].itemObject.SetActive(true);
            }

            else
            {
                currentIndex = 0;
            }

        }
    }

    private void MouseItemScroll()
    {
        if(Mouse.current != null)
        {
            Vector2 scrollDelta = Mouse.current.scroll.ReadValue();
            float scrollY = scrollDelta.y;

            if(scrollY < 0f)
            {
                if (slots[currentIndex].itemObject != null)
                {
                    slots[currentIndex].itemObject.SetActive(false);
                    activeSlotImages[currentIndex].gameObject.SetActive(false);
                }

                currentIndex--;

                if (currentIndex < 0)
                {
                    currentIndex = 0;
                }


                if (slots[currentIndex].itemObject != null)
                {
                    slots[currentIndex].itemObject.SetActive(true);
                    activeSlotImages[currentIndex].gameObject.SetActive(true);
                }
            }

            if (scrollY > 0f)
            {
                if (slots[currentIndex].itemObject != null)
                {
                    slots[currentIndex].itemObject.SetActive(false);
                    activeSlotImages[currentIndex].gameObject.SetActive(false);
                }

                currentIndex++;


                if (currentIndex == maxInventorySize)
                {
                    currentIndex = slots.Count - 1;
                }


                if (slots[currentIndex].itemObject != null)
                {
                    slots[currentIndex].itemObject.SetActive(true);
                    activeSlotImages[currentIndex].gameObject.SetActive(true);
                }

            }
        }
    }
    public void DropItem(InputAction.CallbackContext context)
    {
        if(context.performed && slots[currentIndex] != null && slots[currentIndex].item != null)
        {
            RemoveItem(slots[currentIndex].itemObject);
        }
    }

    public List<InventorySlot> GetCurrentInventory()
    {
        return slots;
    }
}
