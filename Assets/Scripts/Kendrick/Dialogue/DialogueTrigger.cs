using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    bool Enter = false;

    public Dialogue dialogue;
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
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
