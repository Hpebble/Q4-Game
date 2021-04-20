using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject windowsButtonObj;
    // Start is called before the first frame update
    void Start()
    {
        windowsButtonObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void windowsButton()
    {
        windowsButtonObj.SetActive(true);
    }
}
