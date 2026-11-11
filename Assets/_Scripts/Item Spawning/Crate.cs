using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    [SerializeField] private InventorySystem inventorySystem;

    [SerializeField] private Minigame MiniGameManager;

    [SerializeField] private List<Item> itemsToSpawn;
    [SerializeField] private Transform spawnPosition;

    [Header("Mini Game Type")]
    [SerializeField] private bool isMinigameOne;
    [SerializeField] private bool isMinigameTwo;

    private static DialogueTrigger crateDialogue;
    private static bool hasInteracted = false;

    public string InteractPrompt => gameObject.name;
    public bool Interact()
    {
        if (MiniGameManager.isActiveAndEnabled && isMinigameTwo)
        {
            if (!MiniGameManager.GetMiniGameTwo().activeInHierarchy)
            {
                if (itemsToSpawn.Count != 0)
                {
                    MiniGameManager.GetMiniGameTwo().SetActive(true);
                    MiniGameManager.reward = GiveItems;
                }
                else
                {
                    gameObject.name = "Crate (Empty)";
                }

            }

        }
        if (MiniGameManager.isActiveAndEnabled && isMinigameOne)
        {
            if (!MiniGameManager.GetMiniGameOne().activeInHierarchy)
            {
                if (itemsToSpawn.Count != 0)
                {
                    MiniGameManager.GetMiniGameOne().SetActive(true);
                    MiniGameManager.reward = GiveItems;
                }
                else
                {
                    gameObject.name = "Crate (Empty)";
                }


            }

        }
        if (TryGetComponent<DialogueTrigger>(out crateDialogue) && !hasInteracted)
        {
            crateDialogue.TriggerDialogue();

        }
        hasInteracted = true;
        return true;
    }

    public void GiveItems()
    {
        if (itemsToSpawn.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, itemsToSpawn.Count);

            if (itemsToSpawn[randomIndex] != null)
            {
                var currentItem = Instantiate(itemsToSpawn[randomIndex].itemObject, spawnPosition.position,
                    itemsToSpawn[randomIndex].itemObject.transform.rotation);
                inventorySystem.RefreshSelection();
                inventorySystem.AddItem(itemsToSpawn[randomIndex], itemsToSpawn[randomIndex].baseQuantity, currentItem);
                itemsToSpawn.RemoveAt(randomIndex);
            }
          
        }


    }
}
