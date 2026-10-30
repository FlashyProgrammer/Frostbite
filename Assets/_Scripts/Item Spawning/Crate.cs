using System.Collections.Generic;
using Unity.Mathematics;
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

    private void Awake()
    {
        crateDialogue = GetComponent<DialogueTrigger>();
    }

    public string InteractPrompt => gameObject.name;
    public bool Interact()
    {
        if (MiniGameManager.isActiveAndEnabled && isMinigameTwo)
        {
            if (!MiniGameManager.GetMiniGameTwo().activeInHierarchy && hasInteracted)
            {
                MiniGameManager.GetMiniGameTwo().SetActive(true);
                MiniGameManager.reward = GiveItems;

            }
          
        }
        if (MiniGameManager.isActiveAndEnabled && isMinigameOne)
        {
            if (!MiniGameManager.GetMiniGameOne().activeInHierarchy && hasInteracted)
            {
                MiniGameManager.GetMiniGameOne().SetActive(true);
                MiniGameManager.reward = GiveItems;

            }

        }
        crateDialogue.enabled = true;
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

                inventorySystem.AddItem(itemsToSpawn[randomIndex], itemsToSpawn[randomIndex].baseQuantity, currentItem);
                itemsToSpawn.RemoveAt(randomIndex);
            }
          
        }


    }
}
