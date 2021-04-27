using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightStats : MonoBehaviour
{
    public float currentHealth;
    public Stat maxHealth;
    public Stat Defence;
    public Stat damage;

    public Slider healthSlider;
    public Image DeathFade;
    public bool dead;
    //float t = 0;
    private bool startDeathFade;
    public Animator UIanim;
    private void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }
    void Start()
    {
        
    }
    void Update()
    {
        CheckIfDead();
        LimitStats();
        UpdateUI();
    }
    public void TakeDamage(GameObject kbSource, Hurtbox hurtbox)
    {
        currentHealth -= hurtbox.damage;
        Knight.instance.ApplyKnockback(kbSource, hurtbox.kbStrength, hurtbox.upForce);
    }
    IEnumerator Die()
    {
        Knight.instance.anim.SetBool("Dead", true);
        Knight.instance.disableMovement = true;
        yield return new WaitForSeconds(1f);
        UIanim.SetBool("Dead", true);
        yield return new WaitForSeconds(2f);
        GameManager.instance.RestartLevel();
    }
    private void CheckIfDead()
    {
        if (currentHealth == 0)
        {
            if (!dead)
            {
                dead = true;
                StartCoroutine(Die());
            }
        }
    }
    private void LimitStats()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
    }
    private void UpdateUI()
    {
        float curHealthPercent = currentHealth / maxHealth.GetValue();
        healthSlider.value = Mathf.Lerp(healthSlider.value, curHealthPercent, 10 * Time.deltaTime);
    }
}
