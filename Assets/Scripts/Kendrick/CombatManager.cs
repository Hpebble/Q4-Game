using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public bool canReceiveInput;
    public bool inputReceived;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Knight.instance.anim.SetBool("InputRevieved",inputReceived);
    }
    public void Attack()
    {
        if (canReceiveInput)
        {
            inputReceived = true;
            canReceiveInput = false;
        }
        else
        {
            return;
        }
    }

    public void InputManager()
    {
        if (!canReceiveInput)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }
    }
}
