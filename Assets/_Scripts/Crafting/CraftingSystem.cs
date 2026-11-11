using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private InventorySystem playerInventory;

    [Header("UI")]
    [SerializeField] private List<Image> ingredientIcon;
    [SerializeField] private List<Image> materialsIcon;
    [SerializeField] private List<TextMeshProUGUI> quantityTexts;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [Header("Audio")]
    [SerializeField] private AudioSource craftSound;

    public ItemRecipes currentRecipe;
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

        foreach (var text in quantityTexts)
        {
            text.enabled = false;
        }
    }

    public void CheckCraft(ItemRecipes itemRecipes)
    {
        canCraft = true;
        currentRecipe = itemRecipes;
        
        var currentInventory = playerInventory.GetCurrentInventory();

        for (int index = 0; index < itemRecipes.recipe.Count; index++)
        {
            var ingredient = itemRecipes.recipe[index];
            // Checks overall quantity amount of qualified ingredients owned
            int amountOwned = 0;

            // Run number is equal to number of ingredients in recipe
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

            if (index < quantityTexts.Count)
            {
                quantityTexts[index].enabled = true;
                quantityTexts[index].text = ingredient.quantity.ToString(); 
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

        for (int i = itemRecipes.recipe.Count; i < quantityTexts.Count; i++)
        {
            quantityTexts[i].enabled = false;
        }

        descriptionText.enabled = true;
        descriptionText.text = itemRecipes.output.itemDescription;
    }

    public void CraftItem()
    {
        if (!canCraft)
        {
             currentRecipe = null;
            Debug.Log("Can't craft");
            return;
        }

        if (currentRecipe != null)
        {
            var currentInventory = playerInventory.GetCurrentInventory();

            foreach (var ingredient in currentRecipe.recipe)
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

            var itemObject = Instantiate(currentRecipe.itemObject);
            playerInventory.AddItem(currentRecipe.output, currentRecipe.output.baseQuantity, itemObject);
            craftSound.Play();
            CheckCraft(currentRecipe);
        }
        
    }

}
