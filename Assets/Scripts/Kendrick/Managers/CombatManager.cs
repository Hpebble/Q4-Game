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
        if (!GameManager.instance.inKnightGame)
        {
            this.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.inKnightGame)
        {
            Knight.instance.anim.SetBool("InputRecieved", inputReceived);
        }
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
            Knight.instance.anim.SetTrigger("Dash");
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
        if (canReceiveInput)
        {
            Knight.instance.anim.SetTrigger("UpSlash");
            inputReceived = true;
            canReceiveInput = false;
        }
        else
        {
            return;
        }
    }
    public void InputGroundPound()
    {
        if (canReceiveInput)
        {
            Knight.instance.anim.SetTrigger("GroundPound");
            inputReceived = true;
            canReceiveInput = false;
        }
        else
        {
            return;
        }
    }
    public IEnumerator Dash()
    {
        yield return new WaitUntil(() => Knight.instance.isDashing);
        Knight.instance.stats.DashBars--;
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
        CooldownManager.instance.ResetCooldown("BasicAttack");
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
