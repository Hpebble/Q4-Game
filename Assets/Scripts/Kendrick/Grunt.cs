using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    public enum ENEMYSTATE { Idle, Wander, ChasePlayer, Attack }
    public bool dead;

    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        CheckIfDead();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If you touch hurtbox take damage
        Hurtbox hurtbox = collision.GetComponent<Hurtbox>();
        if (collision.tag == "PlayerHurtbox")
        {
            anim.SetTrigger("TakeDamage");
            //takingDamage = true;
            TakeDamage(collision.gameObject, hurtbox);
        }
    }
    IEnumerator Die()
    {
        this.anim.SetBool("Dead", true);
        dead = true;
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
    private void CheckIfDead()
    {
        if (health <= 0)
        {
            if (!dead)
            {
                dead = true;
                StartCoroutine(Die());
            }
        }
    }
    public override void Attack()
    {
    }
}
