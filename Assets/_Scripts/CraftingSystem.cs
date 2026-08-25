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

        for (int index = 0; index < itemRecipes.recipe.Count; index++)
        {
            var ingredient = itemRecipes.recipe[index];

            // Checks overall amount of qualified ingredients owned
            int amountOwned = 0;

            // Foreach = will run twice
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

            if (index < ingredientIcon.Count)
            {
                ingredientIcon[index].enabled = true;
                ingredientIcon[index].sprite = ingredient.item.itemIcon;
                ingredientIcon[index].color = hasEnough ? Color.white : Color.red;
            }

            if (index < materialsIcon.Count)
            {
                materialsIcon[index].enabled = true;
                materialsIcon[index].sprite = ingredient.item.itemIcon;
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
                    Destroy(slot.itemObject);
                    slot.Clear();
                }
            }
        }

        var itemObject = Instantiate(recipe.itemObject);
        playerInventory.AddItem(recipe.output, recipe.output.baseQuantity, itemObject);
        playerInventory.GrabObject(itemObject);
        CheckCraft(recipe);
    }

}
