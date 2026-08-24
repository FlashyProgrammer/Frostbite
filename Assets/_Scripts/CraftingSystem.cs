using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private InventorySystem playerInventory;
    [SerializeField] private List<Image> ingredientIcon;
    [SerializeField] private List<Image> materialsIcon;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private bool canCraft;
    private int index;

    private void Awake()
    {
        foreach(var icon in ingredientIcon)
        {
            icon.enabled = false;
        }
        foreach (var matIcon in materialsIcon)
        {
            matIcon.enabled = false;
        }
        descriptionText.enabled = false;
    }

public void CheckCraft(ItemRecipes itemRecipes)
    {
        canCraft = true;

        var currentInventory = playerInventory.GetCurrentInventory();

        for (int i = 0; i < itemRecipes.recipe.Count; i++)
        {
            var ingredient = itemRecipes.recipe[i];
            int amountOwned = 0;

            foreach (var slot in currentInventory)
            {
                if (slot.item == ingredient.item)
                {
                    amountOwned += slot.quantity;
                }
            }

            bool hasEnough = amountOwned >= ingredient.quantity;
            if (!hasEnough)
            {
                canCraft = false;
            }

            if (i < ingredientIcon.Count)
            {
                ingredientIcon[i].enabled = true;
                ingredientIcon[i].sprite = ingredient.item.itemIcon;
                ingredientIcon[i].color = hasEnough ? Color.white : Color.red;
            }

            if (i < materialsIcon.Count)
            {
                materialsIcon[i].enabled = true;
                materialsIcon[i].sprite = ingredient.item.itemIcon;
            }
        }

        for (int i = itemRecipes.recipe.Count; i < ingredientIcon.Count; i++)
        {
            ingredientIcon[i].enabled = false;
        }

        for (int i = itemRecipes.recipe.Count; i < materialsIcon.Count; i++)
        {
            materialsIcon[i].enabled = false;
        }

        descriptionText.enabled = true;
        descriptionText.text = itemRecipes.output.itemDescription;
    }

    public void CraftItem(ItemRecipes recipe)
    {
        if (!canCraft)
        {
            Debug.Log("Can't craft");
            return;
        }

        var currentInventory = playerInventory.GetCurrentInventory();

        foreach (var ingredient in recipe.recipe)
        {
            int amountToRemove = ingredient.quantity;

            foreach (var slot in currentInventory)
            {
                if (amountToRemove <= 0)
                {
                    break;
                }

                if (slot.item != ingredient.item)
                {
                    continue;
                }

                int amountFromThisSlot = Mathf.Min(slot.quantity, amountToRemove);
                slot.quantity -= amountFromThisSlot;
                amountToRemove -= amountFromThisSlot;

                if (slot.quantity <= 0)
                {
                    slot.Drop();
                }
            }
        }

        playerInventory.AddItem(recipe.output, recipe.output.baseQuantity);
        CheckCraft(recipe);
    }

}
