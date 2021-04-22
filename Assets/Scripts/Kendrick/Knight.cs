using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knight : MonoBehaviour
{
    public static Knight instance;
    [Header("Player Stats")]
    public float health;
    public float maxHealth;

    [Header("Movement")]//Player Variables
    public float jumpStrength = 10;
    public float groundSpeed;
    public float groundDampening;
    public float airSpeed;
    public float airDampening;
    public float jumpFallMultiplier = 2.5f;
    public float lowJumpFallMultiplier = 5f;
    public float maxFallSpeed = -29f;
    private bool grounded;
    private float defaultJumpStrength = 22;
    private float defaultGroundSpeed = 430;
    private float defaultGroundDampening = 0.95f;
    private float defaultAirSpeed = 83;
    private float defaultAirDampening = 0.5f;
    private float defaultJumpFallMultiplier = 5;
    private float defaultLowJumpFallMultiplier = 5f;
    private float defaultMaxFallSpeed = -22f;

    public float dashDist;
    public float dashTime;
    public bool isDashing;

    [Header("Other Stuff")]
    public bool disableMovement;
    public bool attacking;
    public int directionFacing;

    //Reference Variables
    Animator anim;
    SpriteRenderer sr;
    public Rigidbody2D rb;
    BoxCollider2D boxCol;
    LayerMask platformLayermask = (1 << 8) + 1;

    [Header("Animator Stuff")]
    public Animator RuneAnimator;

    //Define Reference Variables
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
    }
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        checkGround();
        //Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && !CheckIfActionCurrentlyTaken())
        {
            isDashing = true;
            float direction;
            if (Input.GetAxisRaw("Horizontal") < 0.1f && Input.GetAxisRaw("Horizontal") > -0.1f)
            {
                direction = directionFacing;
            }
            else { direction = directionFacing; }

            StartCoroutine(Dash(direction));
            Debug.Log("aaaa");
        }
        if (Input.GetMouseButtonDown(0))
        {
            CombatManager.instance.Attack();
        }
    }
    void FixedUpdate()
    {
        //Animator Stuff
        anim.SetFloat("xVelo", rb.velocity.x);
        anim.SetBool("Attacking",attacking);
        anim.SetBool("IsDashing",isDashing);
        //Moving
        if (!disableMovement || !CheckIfActionCurrentlyTaken())
        {
            CalcMovement();
        }

    }


    void checkGround()
    {

        //Boxcast under player to detect ground
        float boxHeight = 0.1f;

        grounded = Physics2D.BoxCast(new Vector2(boxCol.bounds.center.x, boxCol.bounds.center.y - boxCol.bounds.extents.y), new Vector2(boxCol.bounds.extents.x, 0.02f), 0f, Vector2.down, boxHeight, platformLayermask);
        bool downPressed;
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            downPressed = true;
        }
        else downPressed = false;

        if (Input.GetButtonDown("Jump") && grounded)// && !downPressed)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpStrength);
            grounded = false;
        }
    }
    void CalcMovement()
    {
        float fXVelo = rb.velocity.x;
        if (grounded && !CheckIfActionCurrentlyTaken())
        {

            fXVelo += Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime * groundSpeed;
            fXVelo *= Mathf.Pow(1f - groundDampening, Time.fixedDeltaTime * 10f);//Movement dampening when on ground
        }
        else if (!grounded && !CheckIfActionCurrentlyTaken())
        {
            fXVelo += Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime * airSpeed;
            fXVelo *= Mathf.Pow(1f - airDampening, Time.fixedDeltaTime * 10f);//movement dampening when on air/air resistance
        }

        rb.velocity = new Vector2(fXVelo, rb.velocity.y);
        //Makes the jump feel better by making fall faster
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpFallMultiplier - 1) * Time.fixedDeltaTime;
        }

        //Allows you to jump different heights
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpFallMultiplier - 1) * Time.fixedDeltaTime;
        }
        if (rb.velocity.y < maxFallSpeed) //Cap fall speed
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    IEnumerator Dash(float direction)
    {
        //rb.velocity = new Vector2(rb.velocity.x, 0f);
        //rb.AddForce(new Vector2(dashDist * (Mathf.Clamp(direction,-1,1)), 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(dashDist * (Mathf.Clamp(direction,-1,1)), 0f);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        rb.gravityScale = gravity;
    }
    private void SetDefaultMoveVals()
    {
        jumpStrength = defaultJumpStrength;
        groundSpeed = defaultGroundSpeed;
        groundDampening = defaultGroundDampening;
        airSpeed = defaultAirSpeed;
        airDampening = defaultAirDampening;
        jumpFallMultiplier = defaultJumpFallMultiplier;
        lowJumpFallMultiplier = defaultLowJumpFallMultiplier;
        maxFallSpeed = defaultMaxFallSpeed;
    }
    private void SetCanRecieveCombatInput()
    {
        CombatManager.instance.canReceiveInput = true;
    }
    private bool CheckIfActionCurrentlyTaken()
    {
        if(isDashing || attacking)
        {
            return true;
        }
        else { return false; }
    }
    public void FaceLeft()
    {
        directionFacing = -1;
    }
    public void FaceRight()
    {
        directionFacing = 1;
    }
}
