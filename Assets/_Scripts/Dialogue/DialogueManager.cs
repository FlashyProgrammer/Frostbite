using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueTextTwo;
    [SerializeField] private float textSpeed;

    [Header("Voice")]
    [SerializeField] private AudioSource voicePoint;
   

    private bool isTyping;
    private string currentName;
    private Queue<string> lineList;
    private Queue<float> stayTimes;
    private Queue<string> charNames;
    private string currentLine;
    private float currentStayTime;


    private Queue<AudioClip> voiceList;
    private AudioClip currentVoiceline;
   

    void Awake()
    {
        lineList = new Queue<string>();
        voiceList = new Queue<AudioClip>();
        stayTimes = new Queue<float>();
        charNames = new Queue<string>();
    }
  
    public void BeginDialogue(Dialogue character)
    {
        lineList.Clear();
        stayTimes.Clear();
        voiceList.Clear();
        charNames.Clear();

        foreach (characterLine charline in character.charLines)
        {
            lineList.Enqueue(charline.line);
            stayTimes.Enqueue(charline.lineStayTime);
            charNames.Enqueue(charline.charName);

        }

       
        foreach (AudioClip voiceline in character.charVoiceLines)
        {
            voiceList.Enqueue(voiceline);
        }
       
        currentLine = lineList.Dequeue();
        currentStayTime = stayTimes.Dequeue();
        currentVoiceline = voiceList.Dequeue();
        currentName = charNames.Dequeue();

        if (!dialogueTextTwo.isActiveAndEnabled)
        {
            dialogueText.gameObject.SetActive(true);
        }
        StopAllCoroutines();
        StartCoroutine(TypeLine(currentLine));
        StartCoroutine(PlayLine(currentVoiceline));
    }
    
    public void DisplayNextLine()
    {
        if (lineList.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lineList.Dequeue();
        currentStayTime = stayTimes.Dequeue();
        currentName = charNames.Dequeue();
        StartCoroutine(TypeLine(currentLine));

    }

    public void PlayNextLine()
    {
        if (voiceList.Count != 0)
        {
            currentVoiceline = voiceList.Dequeue();
            StartCoroutine(PlayLine(currentVoiceline));
        }
      
    }
    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        dialogueText.text = currentName + " : ";
        dialogueTextTwo.text = dialogueText.text;
        foreach (char letter in line.ToCharArray())
        {
            isTyping = true;
            yield return new WaitForSeconds(textSpeed);
            dialogueText.text += letter;
            dialogueTextTwo.text = dialogueText.text;
            yield return null;
        }
        if(currentVoiceline != null)
        {
            yield return new WaitUntil(() => !voicePoint.isPlaying);
            DisplayNextLine();
            PlayNextLine();
        }
        else
        {
            yield return new WaitForSeconds(currentStayTime);
            DisplayNextLine();
            PlayNextLine();
        }
    }

    IEnumerator PlayLine(AudioClip voiceline)
    {
        yield return new WaitUntil(() => isTyping);
        voicePoint.clip = voiceline;
        voicePoint.Play();
    }

    private void EndDialogue()
    {
        dialogueText.text = "";
        dialogueTextTwo.text = dialogueText.text;
        dialogueText.gameObject.SetActive(false);
    }

    public void PressNext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayNextLine();
        }
    }
    
}
