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
    public Vector2 randomWaitTime;
    public Vector2 walkingTime;
    private float attackCD;
    private float idleTimeCD;
    private float randomWaitTimeCD;
    private float walkingTimeCD;

    public LayerMask playerLayer;
    public LayerMask WorldLayer;
    public LayerMask enemyLayer;
    private CircleCollider2D cCol;
    private Collider2D col;

    protected override void Start()
    {
        base.Start();
        col = this.GetComponent<Collider2D>();
        cCol = this.GetComponentInChildren<CircleCollider2D>();
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
        //SWITCH CASES//
        /*
        */
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If you touch hurtbox take damage
        Hurtbox hurtbox = collision.GetComponent<Hurtbox>();
        if (collision.tag == "PlayerHurtbox" && !dead)
        {
            TakeDamage(collision.gameObject, hurtbox);
            currentState = EnemyState.TakingDamage;
            anim.SetTrigger("TakeDamage");
            if (currentState == EnemyState.Attack && inturruptWhendamaged)
            {
            }
        }
    }
    void CheckPlayerInRange()
    {
        RaycastHit2D ray = Physics2D.Raycast(this.transform.position, Knight.instance.transform.position - this.transform.position, playerDetectRange, WorldLayer + playerLayer);
        if (ray.transform != null && ray.transform.tag == "Player")
        {
            if(Physics2D.CircleCast(this.transform.position, playerAttackRange, Vector2.zero, 999, playerLayer) && currentState != EnemyState.Attack && attackCD <= 0 )
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
            if (Physics2D.CircleCast(this.transform.position, playerDetectRange, Vector2.zero, 999, playerLayer) && !Physics2D.CircleCast(this.transform.position, playerAttackRange, Vector2.zero, 999, playerLayer) && !anim.GetBool("Attacking"))
            {
                //anim.ResetTrigger("Attack");
                currentState = EnemyState.ChasePlayer;
                return;
            }
        }
        else { currentState = EnemyState.Idle; }
    }
    void ChasePlayer()
    {
        float playerDirection = -(this.gameObject.transform.position.x - Knight.instance.transform.position.x);
        float yDiff = Knight.instance.transform.position.y - (this.gameObject.transform.position.y - col.bounds.extents.y);
        if (playerDirection <= 0) { playerDirection = -1; } else if (playerDirection > 0) { playerDirection = 1; }
        //Debug.Log(playerDirection);
        if (!anim.GetBool("Attacking") && !anim.GetBool("TakingDamage") && !dead)
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
    public override void Attack()
    {
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, playerDetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, playerAttackRange);
    }
}
