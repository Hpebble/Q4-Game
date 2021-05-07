using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public float typingSpeed;
    public float camZoom;
    public GameObject camLocation;
    public DialogueTrigger nextDialogue;
    [TextArea(3, 10)]
    public string[] sentences;

}
