using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct characterLine
{
    public string charName;
    [TextArea(3, 10)]
    public string line;
    public float lineStayTime;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogues")]

public class Dialogue : ScriptableObject
{
    public AudioClip[] charVoiceLines;
    public characterLine[] charLines; 

}
