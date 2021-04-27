using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    public enum EnemyState { Idle, Wander, ChasePlayer, Attack }
    public EnemyState currentState;
    [Header("AI Values")]
    public float wanderSpeed;
    public float chaseSpeed;

    public Vector2 idleTime; 
    public Vector2 randomWaitTime;
    public Vector2 walkingTime;

    public float playerDetectRange;
    public LayerMask playerLayer;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        switch (currentState)
        {
            case EnemyState.Idle:

                break;

            case EnemyState.Wander:
                
                break;

            case EnemyState.ChasePlayer:
                ChasePlayer();
                break;

            case EnemyState.Attack:

                break;

        }
        //SWITCH CASES//
        if (Physics2D.CircleCast(this.transform.position,playerDetectRange, Vector2.zero,999,playerLayer))
        {
            currentState = EnemyState.ChasePlayer;
        }
        else { currentState = EnemyState.Idle; }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If you touch hurtbox take damage
        Hurtbox hurtbox = collision.GetComponent<Hurtbox>();
        if (collision.tag == "PlayerHurtbox" && !dead)
        {
            anim.SetTrigger("TakeDamage");
            TakeDamage(collision.gameObject, hurtbox);
        }
    }
    void ChasePlayer()
    {
        float playerDirection = -(this.gameObject.transform.position.x - Knight.instance.transform.position.x);
        if (playerDirection <= 0) { playerDirection = -1; } else if (playerDirection > 0) { playerDirection = 1; }
        Debug.Log(playerDirection);
        rb.velocity = new Vector2(chaseSpeed * playerDirection, 0);
    }
    public override void Attack()
    {
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, playerDetectRange);
    }
}
