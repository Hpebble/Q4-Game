using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public float maxhealth;
    public float health;
    public float shakeTime;
    public float shakeSpeed;
    public float shakeAmount;
    public GameObject sr;
    Vector2 startingPos;
    private bool takingDamage;
    private void Awake()
    {
        startingPos.x = sr.transform.position.x;
        startingPos.y = sr.transform.position.y;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If you touch hurtbox take damage
        Hurtbox hurtbox = collision.GetComponent<Hurtbox>();
        if (collision.tag == "PlayerHurtbox")
        {
            TakeDamage(hurtbox.damage);
            StartCoroutine(DamageAnim());
        }
        CheckHealth();
    }
    private void Update()
    {
        if (takingDamage)
        {
            float x = startingPos.x + (Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);
            float y = startingPos.y + (Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);
            sr.transform.position = new Vector2(x, y);
        }
    }
    void TakeDamage(float damage)
    {
        health -= damage;
    }
    void CheckHealth()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator DamageAnim()
    {
        takingDamage = true;
        yield return new WaitForSeconds(shakeTime);
        takingDamage = false;
        sr.transform.position = startingPos;
    }
}
