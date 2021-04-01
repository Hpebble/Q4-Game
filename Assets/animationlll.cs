using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationlll : MonoBehaviour
{
    public GameObject windowsButtonOb;
    public bool Pressed;
    // Start is called before the first frame update
    void Start()
    {
        windowsButtonOb.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void windowsButton()
    {
        
        if (Pressed == true)
        {
            windowsButtonOb.SetActive(true);
            Pressed = false;
        }
        else if (Pressed == false)
        {
            windowsButtonOb.SetActive(false);
            Pressed = true;
        }



    }
}
