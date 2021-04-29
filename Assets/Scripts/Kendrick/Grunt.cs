using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    public enum EnemyState { Idle, Wander, ChasePlayer, Attack, TakingDamage }
    public EnemyState currentState;
    [Header("AI Values")]
    private float staggerTime;
    public float staggerCD;
    public bool inturruptWhendamaged;
    public float wanderSpeed;
    public float chaseSpeed;
    public float playerAttackRange;
    public float playerDetectRange;

    public Vector2 attackCooldown;
    public Vector2 idleTime; 
    public Vector2 wanderTime;
    public Vector2 initChaseTime;
    private float attackCD;
    private float idleTimeCD;
    private float walkingTimeCD;
    private float initiateChaseCD;
    private bool initiatingChase;
    public float wanderDirection;

    public LayerMask playerLayer;
    public LayerMask WorldLayer;
    public LayerMask enemyLayer;
    private CircleCollider2D cCol;
    private float forceForward = 800;
    private float forceLength = 0.04f;
    private float disableYTime = 0.5f;
    private float disableYTimeCD;

    protected override void Start()
    {
        base.Start();
        col = this.GetComponent<Collider2D>();
        cCol = this.GetComponentInChildren<CircleCollider2D>();
        wanderDirection = Random.Range(-1f,1f);
        if (wanderDirection < 0)
        {
            wanderDirection = -1;
        }
        else if (wanderDirection >= 0) wanderDirection = 1;
    }
    protected override void Update()
    {
        base.Update();
        attackCD -= Time.deltaTime;
        switch (currentState)
        {
            case EnemyState.Idle:
                CheckPlayerInRange();
                break;

            case EnemyState.Wander:
                CheckPlayerInRange();
                //If not idling and walking, then Reset walking and idle timers
                if (walkingTimeCD <= 0 && idleTimeCD <= 0 && grounded)
                {
                    wanderDirection = Random.Range(-1, 1);
                    if (wanderDirection < 0) { wanderDirection = -1; } else wanderDirection = 1;
                    walkingTimeCD = Random.Range(wanderTime.x, wanderTime.y);
                    idleTimeCD = Random.Range(idleTime.x, idleTime.y);
                }

                //if walking, then count down walkingtimeCD and move
                if(walkingTimeCD > 0 && grounded)
                {
                    walkingTimeCD -= Time.deltaTime;
                    rb.velocity = new Vector2(wanderSpeed * wanderDirection, 0);
                    CheckIfRunningIntoWall();
                }
                else if(idleTimeCD > 0) //if idling, then lower idle time
                {
                    idleTimeCD -= Time.deltaTime;
                }
                break;

            case EnemyState.ChasePlayer:
                CheckPlayerInRange();
                ChasePlayer();
                AvoidOtherEnemies();
                break;

            case EnemyState.Attack:
                CheckPlayerInRange();
                break;

            case EnemyState.TakingDamage:
                if(!anim.GetBool("TakingDamage"))
                {
                    currentState = EnemyState.Idle;
                }
                break;
        }
        if (!grounded)
        {
            anim.SetTrigger("CancelAttack");
        }
        DisableYVelocity();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If you touch hurtbox take damage
        Hurtbox hurtbox = collision.GetComponent<Hurtbox>();
        if (collision.tag == "PlayerHurtbox" && !dead)
        {
            if (hurtbox.disableGravityOnHit && !grounded)
            {
                disableYTimeCD = disableYTime;
                TakeDamage(collision.gameObject, hurtbox, false);
                StartCoroutine(ForcePush(anim));
                currentState = EnemyState.TakingDamage;
                anim.SetTrigger("TakeDamage");
                return;
            }
            TakeDamage(collision.gameObject, hurtbox,true);
            currentState = EnemyState.TakingDamage;
            anim.SetTrigger("TakeDamage");
        }
    }
    void CheckPlayerInRange()
    {
        RaycastHit2D ray = Physics2D.Raycast(this.transform.position, Knight.instance.transform.position - this.transform.position, playerDetectRange, WorldLayer + playerLayer);
        if (ray.transform != null && ray.transform.tag == "Player")
        {
            //Isplayer In attack range + in state attate + attaCD <= 0
            if (Physics2D.CircleCast(this.transform.position, playerAttackRange, Vector2.zero, 999, playerLayer) && currentState != EnemyState.Attack && attackCD <= 0)
            {
                attackCD = Random.Range(attackCooldown.x, attackCooldown.y);
                anim.SetTrigger("Attack");
                currentState = EnemyState.Attack;
                return;
            }
            else
            {
                if (attackCD <= 0 && Physics2D.CircleCast(this.transform.position, playerAttackRange, Vector2.zero, 999, playerLayer))
                {
                    anim.SetTrigger("Attack");
                    attackCD = Random.Range(attackCooldown.x, attackCooldown.y);
                }
                else
                {
                    anim.ResetTrigger("Attack");
                }
                currentState = EnemyState.Attack;
            }
            //Is player in Chase Range;
            if (Physics2D.CircleCast(this.transform.position, playerDetectRange, Vector2.zero, 999, playerLayer) && !Physics2D.CircleCast(this.transform.position, playerAttackRange, Vector2.zero, 999, playerLayer) && !anim.GetBool("Attacking"))
            {
                {
                    currentState = EnemyState.ChasePlayer;
                }
                //currentState = EnemyState.ChasePlayer;
                return;
            }
        }
        else
        {
            if (currentState != EnemyState.Wander)
            {
                idleTimeCD = Random.Range(idleTime.x, idleTime.y);
            }
            currentState = EnemyState.Wander;
        }
    }
    void ChasePlayer()
    {
        float playerDirection = -(this.gameObject.transform.position.x - Knight.instance.transform.position.x);
        float yDiff = Knight.instance.transform.position.y - (this.gameObject.transform.position.y - col.bounds.extents.y);
        if (playerDirection <= 0) { playerDirection = -1; } else if (playerDirection > 0) { playerDirection = 1; }
        //Debug.Log(playerDirection);
        if (!anim.GetBool("Attacking") && !anim.GetBool("TakingDamage") && !dead && grounded)
        {
            if (yDiff > 1.9 && Knight.instance.GetGrounded())
            {
                return;
            }
            rb.velocity = new Vector2(chaseSpeed * playerDirection, 0);
        }
    }
    void AvoidOtherEnemies()
    {
        if (currentState == EnemyState.ChasePlayer)
        {
            cCol.enabled = true;
        }
        else cCol.enabled = false;
    }
    void CheckIfRunningIntoWall()
    {
        Vector2 startingPos;
        if (wanderDirection == -1)
        {
            startingPos = new Vector2(-col.bounds.extents.x, 0);
        }
        else { startingPos = new Vector2(col.bounds.extents.x, 0); }
        if (Physics2D.Raycast(startingPos + new Vector2(col.bounds.center.x, col.bounds.center.y), new Vector2(wanderDirection, 0), 0.5f, WorldLayer))
        {
            walkingTimeCD = 0;
        }
    }
    IEnumerator ForcePush(Animator animator)
    {
        rb.AddForce(new Vector2((Knight.instance.directionFacing * forceForward), 0));
        DisableYVelocity();
        yield return new WaitForSeconds(forceLength);
        //if (animator.GetBool("Attacking") && !animator.GetBool("TakingDamage"))
        {
            rb.velocity = Vector2.zero;
        }
    }
    void DisableYVelocity()
    {
        if (disableYTimeCD > 0)
        {
            disableYTimeCD -= Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, playerDetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, playerAttackRange);
    }
}
