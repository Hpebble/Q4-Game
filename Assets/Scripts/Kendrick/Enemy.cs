using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public float playerDamage;
    public float bitsToDrop;

    protected bool dead;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected virtual void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }
    protected virtual void Update()
    {
        CheckIfDead();
    }
    public virtual void Attack()
    {

    }
    public virtual void ApplyKnockback(GameObject knockbackSource, float knockbackStrength, float upforce)
    {
        Vector2 kbDir;
        kbDir = new Vector2(Knight.instance.directionFacing, 0);
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2((kbDir.x * knockbackStrength), (upforce));
    }
    public void TakeDamage(GameObject kbSource, Hurtbox hurtbox)
    {
        health -= hurtbox.damage;
        ApplyKnockback(Knight.instance.gameObject, hurtbox.kbStrength, hurtbox.upForce);
    }
    public IEnumerator Die()
    {
        this.anim.SetBool("Dead", true);
        dead = true;
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
    protected void CheckIfDead()
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
}
