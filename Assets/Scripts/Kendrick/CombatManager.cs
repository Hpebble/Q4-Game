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
        Knight.instance.anim.SetBool("InputRecieved",inputReceived);
    }
    public void InputAttack()
    {
        if (canReceiveInput)
        {
            Knight.instance.anim.SetTrigger("BasicAttack");
            inputReceived = true;
            canReceiveInput = false;
        }
        else
        {
            return;
        }
    }
    public void InputDash()
    {
        if (canReceiveInput)
        {
            Knight.instance.anim.SetBool("IsDashing", true);
            inputReceived = true;
            canReceiveInput = false;
            StartCoroutine(Dash());
        }
        else
        {
            return;
        }
    }
    public void InputUpSlash()
    {

    }
    public IEnumerator Dash()
    {
        Knight.instance.isDashing = true;
        float direction;
        if (Input.GetAxisRaw("Horizontal") < 0.1f && Input.GetAxisRaw("Horizontal") > -0.1f)
        {
            direction = Knight.instance.directionFacing;
        }
        else { direction = Mathf.Clamp(Input.GetAxisRaw("Horizontal"), -1, 1); }
        //
        float gravity = Knight.instance.rb.gravityScale;
        Knight.instance.rb.gravityScale = 0;
        Knight.instance.rb.velocity = new Vector2(Knight.instance.dashDist * (Mathf.Clamp(direction, -1, 1)), 0f);
        yield return new WaitForSeconds(Knight.instance.dashTime);
        Knight.instance.isDashing = false;
        Knight.instance.rb.gravityScale = gravity;
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
