using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;
    public TextMeshProUGUI textAmount;
    public Image itemImage;
    public GameObject objectPrefab;

    public InventorySlot()
    {
        Clear();
    }

    public void Drop()
    {
        item = null;
        if (textAmount!= null && itemImage != null)
        {
            textAmount.text = "0";
            itemImage.sprite = null;
            itemImage.enabled = false;
        }
    
    }
    public void Clear()
    {
        item = null;
        if (textAmount != null && itemImage != null)
        {
            textAmount.text = "0";
            itemImage.sprite = null;
            itemImage.enabled = false;
        }
        quantity = 0;
    }

    public bool IsEmpty => item == null;
    public bool IsFull => item != null && quantity >= item.stackLimit;

    public int AddToStack(int amount)
    {
        int roomLeft = item.stackLimit - quantity;
        int amountToAdd = Math.Min(roomLeft, amount);

        quantity += amountToAdd;
        return amount - amountToAdd;
    }

    public TextMeshProUGUI SetText(TextMeshProUGUI text)
    {
        textAmount = text;
        return textAmount;
    }

    public Image SetImage(Image image) 
    {
        itemImage = image;
        return itemImage;
    }

}
