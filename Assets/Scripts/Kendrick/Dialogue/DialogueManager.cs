using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    static public DialogueManager instance;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public bool DialogueEnded;
    private float typingSpeed;

    private Queue<string> sentences;
    private string currentSentence;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        DialogueEnded = false;
        GameManager.instance.inDialogue = true;
        typingSpeed = dialogue.typingSpeed;

        Debug.Log("Starting Conversation with " + dialogue.name);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogue());
            return;
        }
        //GameObject.Find("ContinueSound").GetComponent<AudioScript>().playAudio(); AUDIO REMINDER
        string sentence = sentences.Dequeue();
        currentSentence = sentence;
        //dialogueText.text = sentence;
        StartCoroutine(TypeSentence(sentence));
    }
    public void SkipTyping()
    {
        StopAllCoroutines();
        dialogueText.text = currentSentence;
    }
    public bool CheckIfDoneTyping()
    {
        if (dialogueText.text.Length < currentSentence.Length)
        {
            return false;
        }
        else return true;
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            yield return new WaitForSeconds(typingSpeed * Time.deltaTime);
            dialogueText.text += letter;
            yield return null;
        }
    }
    IEnumerator EndDialogue()
    {
        yield return new WaitForSeconds(0.1f);
        if (DialogueEnded == false)
        {
            DialogueEnded = true;
            GameManager.instance.inDialogue = false;
        }
    }
}
