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

    protected bool grounded;
    protected bool dead;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    public LayerMask groundLayer;
    protected virtual void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        col = this.GetComponent<Collider2D>();
    }
    protected virtual void Update()
    {
        CheckIfDead();
        CheckGround();
    }
    public virtual void ApplyKnockback(GameObject knockbackSource, float knockbackStrength, float upforce)
    {
        Vector2 kbDir;
        kbDir = new Vector2(Knight.instance.directionFacing, 0);
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2((kbDir.x * knockbackStrength), (upforce));
    }
    public void TakeDamage(GameObject kbSource, Hurtbox hurtbox, bool applyKnockback)
    {
        health -= hurtbox.damage;
        if(applyKnockback)
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
    void CheckGround()
    {
        //Boxcast under player to detect ground
        float boxHeight = 0.1f;

        grounded = Physics2D.BoxCast(new Vector2(col.bounds.center.x, col.bounds.center.y - col.bounds.extents.y), new Vector2(col.bounds.extents.x * 2, 0.02f), 0f, Vector2.down, boxHeight, groundLayer);
        /*bool downPressed;
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            downPressed = true;
        }
        else downPressed = false;
        
        if (Input.GetButtonDown("Jump") && grounded && !CheckIfActionCurrentlyTaken())// && !downPressed)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpStrength);
            grounded = false;
        }
        */
    }
}
