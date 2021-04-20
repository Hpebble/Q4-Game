using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knight : MonoBehaviour
{
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

    [Header("Other Stuff")]
    private float dashCD = 0.4f;

    //Additional Direction Variables (Julien)
    public SpriteRenderer playerSprite;
    public float aimDir = 1;
    public bool cannotTurn = false;

    //Reference Variables
    SpriteRenderer sr;
    Rigidbody2D rb;
    BoxCollider2D boxCol;
    LayerMask platformLayermask = (1 << 8) + 1;

    [Header("Animator Stuff")]
    public Animator RuneAnimator;

    //Define Reference Variables
    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        checkGround();
        //Dashing
        if (Input.GetMouseButtonDown(0))
        {
            CombatManager.instance.Attack();
        }
    }
    void FixedUpdate()
    {
        //Moving
        float fXVelo = rb.velocity.x;
        if (grounded)
        {

            fXVelo += Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime * groundSpeed;
            fXVelo *= Mathf.Pow(1f - groundDampening, Time.fixedDeltaTime * 10f);//Movement dampening when on ground

            //Set Player Direction
            if (Input.GetAxisRaw("Horizontal") != 0 && !cannotTurn)
            {
                aimDir = Mathf.Sign(Input.GetAxisRaw("Horizontal"));
            }
        }
        else if (!grounded)
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

        //Set Player Sprite Direction
        if (aimDir == 1)
        {
            playerSprite.flipX = false;
        }
        else playerSprite.flipX = true;
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
}
