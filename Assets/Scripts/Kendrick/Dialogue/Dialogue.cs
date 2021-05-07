using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string name;
    public float typingSpeed = 0.5f;
    public float camZoom;
    public GameObject camLocation;
    public DialogueTrigger nextDialogue;
    public Button button;
    public enum CameraType { Knight, OS};
    public CameraType camType;
    [TextArea(3, 10)]
    public string[] sentences;
}
