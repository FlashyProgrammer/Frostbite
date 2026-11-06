using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using TMPro;
using System.Runtime.CompilerServices;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC Dialogue")]
    [SerializeField] private List<DialogueTrigger> characterDialogues;

    [Header("UI")]
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private List<Button> optionButtons;
    [SerializeField] private List<TextMeshProUGUI> buttonText;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject buttomDialogue;
    [SerializeField] private GameObject topDialogue;

    [Header("Interaction settings")]
    [SerializeField] private GameObject characterCamera;
    [SerializeField] private GameMaster gameMaster;

    [Header("Animation")]
    [SerializeField]private Animator animatorController;

    private bool isTalking;
    private void Start()
    {
        for(int i = 0; i < characterDialogues.Count; i++)
        {
            optionButtons[i].onClick.AddListener(characterDialogues[i].TriggerDialogue);
            optionButtons[i].onClick.AddListener(PlayTalk);
            buttonText[i].text = characterDialogues[i].name;
        }
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(buttomDialogue.GetComponent<TextMeshProUGUI>().text))
        {
            for (int i = 0; i < characterDialogues.Count; i++)
            {
                optionButtons[i].gameObject.SetActive(true);
                isTalking = false;
            }
        }
    }
    public string InteractPrompt => gameObject.name;

    public bool Interact()
    {
        if (!isTalking)
        {
            if (!DialogueBox.activeInHierarchy)
            {
                DialogueBox.SetActive(true);
                gameMaster.FreezePlayer();
                characterCamera.SetActive(true);
                inventoryUI.SetActive(false);
                buttomDialogue.SetActive(true);
                topDialogue.SetActive(false);

            }
            else
            {
                DialogueBox.SetActive(false);
                gameMaster.UnfreezePlayer();
                characterCamera.SetActive(false);
                inventoryUI.SetActive(true);
                buttomDialogue.SetActive(false);
                topDialogue.SetActive(true);
                animatorController.Play("Idle");


            }
        }
       
        return true;

        
    }

    private void PlayTalk()
    {
        animatorController.Play("Talking");
        isTalking = true;
        buttomDialogue.SetActive(true);
        topDialogue.SetActive(false);
        gameMaster.FreezePlayer();
        if (buttomDialogue.activeInHierarchy)
        {
            for (int i = 0; i < characterDialogues.Count; i++)
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
