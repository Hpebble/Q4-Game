using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioClip hurtSound;
    public float speed;
    public float health;
    public float maxHealth;
    public float playerDamage;
    public bool takeKnockback;
    public Vector2 bitsToDropRange;
    public GameObject bit;

    protected bool grounded;
    protected bool dead;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected AudioSource audioSource;
    public LayerMask groundLayer;
    protected virtual void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        col = this.GetComponent<Collider2D>();
        health = maxHealth;
    }
    protected virtual void Update()
    {
        CheckIfDead();
        CheckGround();
    }
    public virtual void ApplyKnockback(GameObject knockbackSource, float knockbackStrength, float upforce)
    {
        if (knockbackStrength == 0 && upforce == 0 || takeKnockback) return;
        Vector2 kbDir;
        kbDir = new Vector2(Knight.instance.directionFacing, 0).normalized;
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2((kbDir.x * knockbackStrength), (upforce));
    }
    public virtual void ApplyKnockbackByPosition(GameObject knockbackSource, float knockbackStrength, float upforce)
    {
        if (knockbackStrength == 0 && upforce == 0 || takeKnockback) return;
        float kbDir;
        kbDir = Mathf.Clamp((this.gameObject.transform.position.x - knockbackSource.transform.position.x), -1, 1);
        Debug.Log(kbDir);
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2((kbDir * knockbackStrength), (upforce));
    }
    public void TakeDamage(GameObject kbSource, Hurtbox hurtbox, bool applyKnockback)
    {
        health -= hurtbox.damage;
        if(applyKnockback)
        ApplyKnockback(Knight.instance.gameObject, hurtbox.kbStrength, hurtbox.upForce);
        AudioManager.instance.Play("Hit1", 0.85f, 1.15f);
    }
    public void TakeDamageByPosition(GameObject kbSource, Hurtbox hurtbox, bool applyKnockback)
    {
        health -= hurtbox.damage;
        if (applyKnockback)
        {
            ApplyKnockbackByPosition(kbSource, hurtbox.kbStrength, hurtbox.upForce);
        }
        AudioManager.instance.Play("Hit1", 0.85f, 1.15f);
    }
    public IEnumerator Die()
    {
        this.anim.SetBool("Dead", true);
        dead = true;
        spawnBits();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
    protected void spawnBits()
    {
        float bitsToDrop = Random.Range(bitsToDropRange.x, bitsToDropRange.y);
        for(int i = 0; i < bitsToDrop; i++)
        {
            Instantiate(bit,this.transform.position,Quaternion.identity);
        }
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
    void PlayHurtSound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(hurtSound, 0.5f);
    }
    void CheckGround()
    {
        //Boxcast under player to detect ground
        float boxHeight = 0.1f;

        grounded = Physics2D.BoxCast(new Vector2(col.bounds.center.x, col.bounds.center.y - col.bounds.extents.y), new Vector2(col.bounds.extents.x * 2, 0.02f), 0f, Vector2.down, boxHeight, groundLayer);
    }

}
