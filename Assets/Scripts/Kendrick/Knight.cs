using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knight : MonoBehaviour
{
    public static Knight instance;
    public KnightStats stats;
    public Vector2 respawnPoint;

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
    private bool downPressed;
    #region defaultvals
    private float defaultJumpStrength = 22;
    private float defaultGroundSpeed = 430;
    private float defaultGroundDampening = 0.95f;
    private float defaultAirSpeed = 83;
    private float defaultAirDampening = 0.5f;
    private float defaultJumpFallMultiplier = 5;
    private float defaultLowJumpFallMultiplier = 5f;
    private float defaultMaxFallSpeed = -22f;
    #endregion
    public float dashDist;
    public float dashTime;
    public bool isDashing;
    public bool isGroundPounding;
    public bool attacking;
    public bool takingDamage;
    public bool disableMovement;
    public bool isInvulnerable;

    [Header("Other Stuff")]
    public bool inTransition;
    public int directionFacing;

    //Reference Variables
    public PhysicsMaterial2D noFric;
    public PhysicsMaterial2D defaultFric;
    public Animator anim;
    SpriteRenderer sr;
    public Rigidbody2D rb;
    public BoxCollider2D boxCol;
    public EdgeCollider2D edgeCol;
    public bool edgeColColliding;
    public LayerMask platformLayermask;

    [Header("Animator Stuff")]
    public Animator RuneAnimator;

    //Define Reference Variables
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        instance = this;
    }
    private void Start()
    {
        edgeCol = this.gameObject.GetComponentInChildren<EdgeCollider2D>();
        Camera.main.transform.position = this.transform.position;
        Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -12);
        GameObject gm = GameObject.Find("GameManager");
        if (gm == null)
        {
            SceneManager.LoadScene("Managers", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        }
        this.gameObject.transform.position = respawnPoint;
    }
    void Update()
    {
        if (GameManager.instance.inKnightGame)
        {
            checkGround();
            //Dashing
            if (!GameManager.instance.inDialogue)
            {
                //Jump
                if (Input.GetButtonDown("Jump") && grounded && !CheckIfActionCurrentlyTaken())// && !downPressed)
                {
                    if (downPressed && edgeColColliding)
                    {
                        return;
                    }
                    rb.velocity = new Vector3(rb.velocity.x, jumpStrength);
                    grounded = false;
                }
                if (Input.GetKeyDown(KeyCode.LeftShift) && !takingDamage && !stats.dead && !CooldownManager.instance.CheckOnCooldown("Dash") && stats.DashBars > 0) //!CheckIfActionCurrentlyTaken()
                {
                    CombatManager.instance.InputDash();
                }
                //Basic Attack
                if (Input.GetButtonDown("Fire1") && !CooldownManager.instance.CheckOnCooldown("BasicAttack") && !GameManager.instance.paused)
                {
                    if (grounded)
                    {
                        CombatManager.instance.InputAttack();
                    }
                    if (!grounded && !downPressed)
                    {
                        CombatManager.instance.InputAttack();
                    }
                }
                //Upslash
                if (!GameManager.instance.paused && Input.GetKeyDown(KeyCode.C) && stats.CheckEnoughMana(stats.UpSlashCost, true) && !CooldownManager.instance.CheckOnCooldown("UpSlash"))
                {
                    CombatManager.instance.InputUpSlash();
                    stats.UseMana(stats.UpSlashCost);
                }
                //Groundpound
                if (!GameManager.instance.paused && Input.GetButton("Fire1") && downPressed && !grounded && !CooldownManager.instance.CheckOnCooldown("GroundPound"))
                {
                    CombatManager.instance.InputGroundPound();
                }
            }
        }
    }
    void FixedUpdate()
    {
        //Animator Stuff
        if (!GameManager.instance.inDialogue)
        {
            anim.SetFloat("xVelo", Input.GetAxisRaw("Horizontal"));
        }
        else { anim.SetFloat("xVelo", rb.velocity.x); }
        anim.SetBool("Attacking",attacking);
        anim.SetBool("IsDashing",isDashing);
        anim.SetBool("Grounded", grounded);
        //Moving
        if (!disableMovement || !disableMovement && !CheckIfActionCurrentlyTaken())
        {
            CalcMovement();
            if (takingDamage == true)
            {
                rb.sharedMaterial = defaultFric;
            }
            else
            {
                rb.sharedMaterial = noFric;
            }
        } 
        else
        {
            rb.sharedMaterial = defaultFric;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //If you touch hurtbox take damage
        Hurtbox hurtbox = collision.GetComponent<Hurtbox>();
        if (collision.tag == "Hurtbox" && !isInvulnerable && !stats.dead)
        {
            isInvulnerable = true;
            anim.SetFloat("xVelo", 0f);
            anim.SetTrigger("TakeDamage");
            takingDamage = true;
            stats.TakeDamage(collision.gameObject, hurtbox);
        }
    }
    #region MovingPlatform
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.gameObject.transform.parent = collision.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.gameObject.transform.parent = null;
        }
    }
    #endregion
    void checkGround()
    {
        //Boxcast under player to detect ground
        float boxHeight = 0.1f;

        grounded = Physics2D.BoxCast(new Vector2(boxCol.bounds.center.x, boxCol.bounds.center.y - boxCol.bounds.extents.y), new Vector2(boxCol.bounds.extents.x * 2, 0.01f), 0f, Vector2.down, boxHeight, platformLayermask);       
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            downPressed = true;
        }
        else downPressed = false;
        if (edgeColColliding) { grounded = true; }
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
    public void ApplyKnockback(GameObject knockbackSource, float knockbackStrength, float upforce)
    {
        Vector2 kbDir;
        kbDir = (this.gameObject.transform.position - knockbackSource.transform.position).normalized;
        if (kbDir.x > 0)
        {
            kbDir.x = 1;
        } else { kbDir.x = -1; }
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2((kbDir.x * knockbackStrength), (upforce)), ForceMode2D.Impulse);
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
    public bool CheckIfActionCurrentlyTaken()
    {
        if(isDashing || attacking || takingDamage || isGroundPounding ||stats.dead)
        {
            return true;
        }
        else { return false; }
    }
    public bool GetGrounded()
    {
        return grounded;
    }
    public Vector3 GetCenter()
    {
        return boxCol.bounds.center;
    }
    public bool IsDownPressed()
    {
        return downPressed;
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
