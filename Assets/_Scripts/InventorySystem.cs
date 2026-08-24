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
    [SerializeField] private List<TextMeshProUGUI> numberText;
    [SerializeField] private int maxInventorySize = 5;

    [SerializeField] private Transform followObject;
    [SerializeField] private Transform groundPoint;

    private GameObject handVisual;
    private int currentIndex = 0;

    private void Awake()
    {
        for (int i = 0; i < maxInventorySize; i++)
        {
            slots.Add(new InventorySlot());  
        }
    }

    public bool AddItem(Item itemToAdd, int amount)
    {
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
                    itemToAdd.combinedQuantity = slot.quantity;
                    SpawnVisual(slot);

                    if (remainingAmount <= 0) return true;
                }
            }
            
        }


        while (remainingAmount > 0)
        {
            InventorySlot freeSlot = FindEmptySlot();

            freeSlot.item = itemToAdd;
       
            if (freeSlot == null)
            {
                Debug.LogWarning("Inventory Full! Could not fit remaining: " + remainingAmount);
                return false; 
            }
          
            int maxAmount = itemToAdd.canStack ? itemToAdd.stackLimit : itemToAdd.baseQuantity;
            int currentBatch = Mathf.Min(remainingAmount, maxAmount);

            if (freeSlot.quantity > 1)
            {
                currentBatch = itemToAdd.combinedQuantity;
            }
            freeSlot.quantity = currentBatch;
            remainingAmount -= currentBatch;

            freeSlot.SetText(numberText[slots.IndexOf(freeSlot)]);
            freeSlot.SetImage(slotImages[slots.IndexOf(freeSlot)]);

            freeSlot.objectPrefab = itemToAdd.handPrefab;
            freeSlot.itemImage.enabled = true;
            freeSlot.itemImage.sprite = itemToAdd.itemIcon;
            freeSlot.textAmount.text = freeSlot.quantity.ToString();
            SpawnVisual(freeSlot);

        }
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
        slots[currentIndex].Drop();
        item.transform.position = groundPoint.position;
        item.transform.parent = null;
        handVisual = null;

    }
    public InventorySlot GetSlot() 
    {
        return slots[currentIndex];
  
    }
    public GameObject GetHandVisual()
    {
        return handVisual;
    }
    public void NextItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentIndex++;

            if (currentIndex == maxInventorySize)
            {
                currentIndex = slots.Count - 1;
            }


            if (slots[currentIndex] != null)
            {
                SpawnVisual(slots[currentIndex]);

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
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = 0;
            }


            if (slots[currentIndex] != null)
            {

                SpawnVisual(slots[currentIndex]);

            }

            else
            {
                currentIndex = 0;
            }

        }
    }

    public void SpawnVisual(InventorySlot slot)
    {
        if (handVisual == null && slot.objectPrefab != null && slot.item != null)
        {
            handVisual = Instantiate(slot.objectPrefab, followObject.position, Quaternion.identity);
            handVisual.transform.SetParent(followObject.transform);
        }

        if(slot.objectPrefab != null && handVisual != null && slot.item != null) 
        {
            Destroy(handVisual);
            handVisual = Instantiate(slot.objectPrefab, followObject.position, Quaternion.identity);
            handVisual.transform.SetParent(followObject.transform);
        }
        
    }

    public void DropItem(InputAction.CallbackContext context)
    {
        if(context.performed && slots[currentIndex] != null && slots[currentIndex].item != null)
        {
            RemoveItem(handVisual);
        }
    }

    public List<InventorySlot> GetCurrentInventory()
    {
        return slots;
    }
}
