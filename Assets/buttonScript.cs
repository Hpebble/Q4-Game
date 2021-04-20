using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour
{
    public static GameObject eButton;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("atDesk", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
    
        Debug.Log("Went to Desk");
        anim.SetBool("atDesk", true);
        eLoaded.eLoad = true;
    
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Left Desk");
        anim.SetBool("atDesk", false);
        eLoaded.eLoad = false;
    }
}
