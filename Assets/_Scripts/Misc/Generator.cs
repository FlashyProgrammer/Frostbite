using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Generator : MonoBehaviour, IInteractable
{
    [Header("Facility Variables")]

    [SerializeField] private List<Light> facilityLights;
    [SerializeField] private List<Radar> enemyRadars;

    [Header("Generator Parameters")]
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    [SerializeField] private List<Item> requiredItems;


    [Header("Post processing volumes")]
    [SerializeField] private Volume scenePostProcessing;
    [SerializeField] private VolumeProfile normalProfile;
    [SerializeField] private VolumeProfile darkProfile;

    [Header("Misc")]
    [SerializeField] private Minigame minigameManager;
    [SerializeField] private InventorySystem playerInventory;

    private DialogueTrigger generatorDialogue;
    private float time;
    private float timeCounter;
    private bool generatorDown = false;
    private bool canFix;

    private void Awake()
    {
        generatorDialogue = GetComponent<DialogueTrigger>();
        time = Random.Range(minTime, maxTime);
        timeCounter = time;
    }
    private void Update()
    {
        if (timeCounter > 0 && !generatorDown)
        {
            timeCounter -= Time.deltaTime;
            scenePostProcessing.profile = normalProfile;
        }

        else
        {
            generatorDown = true;

            generatorDialogue.enabled = true;

            foreach (Light light in facilityLights)
            {
                light.enabled = false;
            }

            foreach (Radar radar in enemyRadars) 
            {
                radar.hideRadar();
            }
            scenePostProcessing.profile = darkProfile;
        }

    }

    public string InteractPrompt => gameObject.name;
    public bool Interact()
    {
        var currentInventory = playerInventory.GetCurrentInventory();

        for (int i = 0; i < currentInventory.Count; i++)
        {
            foreach (var item in requiredItems)
            {
                if (currentInventory[i].item == item)
                {
                    canFix = true;
                    Destroy(currentInventory[i].itemObject);
                    currentInventory[i].Clear();
                }
            }
        }

        if (generatorDown && canFix)
        {
            minigameManager.GetMiniGameThree().SetActive(true);
        }

        minigameManager.reward = ActivateFacility;
        return true;
    }

    private void ActivateFacility()
    {
        time = Random.Range(minTime, maxTime);
        timeCounter = time;
        generatorDown = false;
        canFix = false;

        foreach (Light light in facilityLights)
        {
            light.enabled = true;
        }

    }
}
