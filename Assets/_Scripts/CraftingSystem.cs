using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        descriptionText.enabled = true;
        descriptionText.text = itemRecipes.output.itemDescription;
      
    }
    public void CraftItem(ItemRecipes recipe)
    {
        if (!canCraft)
        {
            Debug.Log("Can't craft");
        }
        else
        {
            playerInventory.GetCurrentInventory()[index].Drop();
            playerInventory.AddItem(recipe.output, recipe.output.baseQuantity);

        }
    }

}
