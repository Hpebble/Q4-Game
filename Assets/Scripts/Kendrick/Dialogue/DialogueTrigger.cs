using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    bool Enter = false;
    public bool startOnAwake;
    public bool destroyOnTrigger;
    public Dialogue dialogue;
    private void Start()
    {
        if (startOnAwake)
        {
            Debug.Log("Started Dialogue");
            TriggerDialogue();
        }
    }
    private void OnEnable()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Enter == false)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            Knight.instance.rb.velocity = Vector2.zero;
            Enter = true;
            Destroy(this.gameObject);
        }
    }
    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
        if (destroyOnTrigger)
        {
            Destroy(this);
        }
    }
}
