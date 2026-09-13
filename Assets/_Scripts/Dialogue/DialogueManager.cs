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
    [SerializeField] private float textSpeed;

    [Header("Voice")]
    [SerializeField] private AudioSource voicePoint;
    [SerializeField] private float startTime;


    private string currentName;
    private Queue<string> lineList;
    private string currentLine;


    private Queue<AudioClip> voiceList;
    private AudioClip currentVoiceline;
   

    void Awake()
    {
        lineList = new Queue<string>();
        voiceList = new Queue<AudioClip>();
    }
  
    public void BeginDialogue(Character character)
    {
        lineList.Clear();
        voiceList.Clear();
        currentName = character.charName;


        foreach (string line in character.lines)
        {
            lineList.Enqueue(line);
        }

       
        foreach (AudioClip voiceline in character.charVoicelines)
        {
            voiceList.Enqueue(voiceline);
        }
        

        currentLine = lineList.Dequeue();
        currentVoiceline = voiceList.Dequeue();

        
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
        currentVoiceline = voiceList.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeLine(currentLine));
        StartCoroutine(PlayLine(currentVoiceline));

    }

    public void PlayNextLine()
    {
        if (voiceList.Count == 0)
        {
            EndDialogue();
            return;
        }
        Debug.Log(currentVoiceline);
        currentVoiceline = voiceList.Dequeue();

        StopAllCoroutines();

        StartCoroutine(PlayLine(currentVoiceline));
    }
    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        dialogueText.text = currentName + " : ";

        foreach (char letter in line.ToCharArray())
        {
            yield return new WaitForSeconds(textSpeed);
            dialogueText.text += letter;
            yield return null;
        }

        yield return new WaitUntil(() => !voicePoint.isPlaying);
        DisplayNextLine();
    }

    IEnumerator PlayLine(AudioClip voiceline)
    {
        yield return new WaitForSeconds(startTime);
        voicePoint.clip = voiceline;
        voicePoint.Play();
    }

    private void EndDialogue()
    {
        dialogueText.text = "";
    }

    public void PressNext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayNextLine();
        }
    }
    
}
