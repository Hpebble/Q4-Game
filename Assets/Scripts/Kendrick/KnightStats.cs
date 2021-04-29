using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightStats : MonoBehaviour
{
    public float currentHealth;
    public float currentMana;
    public float bitCount;
    public float naturalManaRegenSpeed;
    public Stat maxHealth;
    public Stat maxMana;
    public Stat Defence;
    public Stat damage;


    public Slider healthSlider;
    public Slider manaSlider;
    public Image DeathFade;
    public Text bitAmountText;
    public bool dead;
    //float t = 0;
    private bool startDeathFade;
    public Animator UIanim;
    private void Awake()
    {
        currentHealth = maxHealth.GetValue();
        currentMana = maxMana.GetValue();
    }
    void Start()
    {
        
    }
    void Update()
    {
        currentMana += naturalManaRegenSpeed * Time.deltaTime;
        CheckIfDead();
        LimitStats();
        UpdateUI();
    }
    public void TakeDamage(GameObject kbSource, Hurtbox hurtbox)
    {
        currentHealth -= hurtbox.damage;
        Knight.instance.ApplyKnockback(kbSource, hurtbox.kbStrength, hurtbox.upForce);
    }
    public void UseMana(float mana)
    {
        currentMana -= mana;
    }
    public bool CheckEnoughMana(float mana)
    {
        if (currentMana >= mana)
        {
            return true;
        }
        else
        {
            UIanim.SetTrigger("ManaNotif");
            return false;
        }
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
        currentMana = Mathf.Clamp(currentMana, 0, maxMana.GetValue());
    }
    private void UpdateUI()
    {
        float curHealthPercent = currentHealth / maxHealth.GetValue();
        float curManaPercent = currentMana / maxMana.GetValue();
        healthSlider.value = Mathf.Lerp(healthSlider.value, curHealthPercent, 10 * Time.deltaTime);
        manaSlider.value = Mathf.Lerp(manaSlider.value, curManaPercent, 10 * Time.deltaTime);
        bitAmountText.text = bitCount.ToString();
    }
}
