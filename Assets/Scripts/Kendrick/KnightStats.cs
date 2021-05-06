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
    public float UpSlashCost;
    public float UpSlashCooldown;
    public float WaveCost;
    public float WaveCooldown;
    public float DashBarReplenishCooldown;
    public float groundpoundSpeed;
    private float DashBarCD;
    public Image[] dashBarImage;
    public int DashBars;
    private float DashLerp = 25.1f;
    public Stat maxHealth;
    public Stat maxMana;
    public Stat Defence;
    public Stat damage;

    public Slider healthSlider;
    public Slider manaSlider;
    public Image DeathFade;
    public Text bitAmountText;
    public Text bitAmountTextShop;
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
        if (GameManager.instance.inKnightGame)
        {
            currentMana += naturalManaRegenSpeed * Time.deltaTime;
            CheckIfDead();
            LimitStats();
            UpdateUI();
            GameManager.instance.currentBits = bitCount; ;
        }
        else
        {
        }
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
    public bool CheckEnoughMana(float mana,bool showWarning)
    {
        if (currentMana >= mana)
        {
            return true;
        }
        else
        {
            if (showWarning)
            {
                UIanim.SetTrigger("ManaNotif");
            }
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
        DashBars = Mathf.Clamp(DashBars, 0, 3);
    }
    private void UpdateDashbar()
    {
        if(DashBars == 3)
        {
            DashBarCD = DashBarReplenishCooldown;
        }
        else
        {
            DashBarCD -= Time.deltaTime;
            if(DashBarCD <= 0)
            {
                DashBarCD = DashBarReplenishCooldown;
                DashBars++;
            }
        }
        switch (DashBars)
        {
            case 0:
                dashBarImage[0].color = Color.Lerp(dashBarImage[0].color, new Color(dashBarImage[0].color.r, dashBarImage[0].color.g, dashBarImage[0].color.b, 16f / 255f), DashLerp * Time.deltaTime) ;
                dashBarImage[1].color = Color.Lerp(dashBarImage[1].color, new Color(dashBarImage[1].color.r, dashBarImage[1].color.g, dashBarImage[1].color.b, 16f / 255f), DashLerp * Time.deltaTime);
                dashBarImage[2].color = Color.Lerp(dashBarImage[2].color, new Color(dashBarImage[2].color.r, dashBarImage[2].color.g, dashBarImage[2].color.b, 16f / 255f), DashLerp * Time.deltaTime);
                break;

            case 1:
                dashBarImage[0].color = Color.Lerp(dashBarImage[0].color, new Color(dashBarImage[0].color.r, dashBarImage[0].color.g, dashBarImage[0].color.b, 255f), 24.5f * Time.deltaTime);
                dashBarImage[1].color = Color.Lerp(dashBarImage[1].color, new Color(dashBarImage[1].color.r, dashBarImage[1].color.g, dashBarImage[1].color.b, 16f / 255f), DashLerp * Time.deltaTime);
                dashBarImage[2].color = Color.Lerp(dashBarImage[2].color, new Color(dashBarImage[2].color.r, dashBarImage[2].color.g, dashBarImage[2].color.b, 16f / 255f), DashLerp * Time.deltaTime);
                break;

            case 2:
                dashBarImage[0].color = Color.Lerp(dashBarImage[0].color, new Color(dashBarImage[0].color.r, dashBarImage[0].color.g, dashBarImage[0].color.b, 255f), DashLerp * Time.deltaTime);
                dashBarImage[1].color = Color.Lerp(dashBarImage[1].color, new Color(dashBarImage[1].color.r, dashBarImage[1].color.g, dashBarImage[1].color.b, 255f), DashLerp * Time.deltaTime);
                dashBarImage[2].color = Color.Lerp(dashBarImage[2].color, new Color(dashBarImage[2].color.r, dashBarImage[2].color.g, dashBarImage[2].color.b, 16f / 255f), DashLerp * Time.deltaTime);
                break;

            case 3:
                dashBarImage[0].color = Color.Lerp(dashBarImage[0].color, new Color(dashBarImage[0].color.r, dashBarImage[0].color.g, dashBarImage[0].color.b, 255f), DashLerp * Time.deltaTime);
                dashBarImage[1].color = Color.Lerp(dashBarImage[1].color, new Color(dashBarImage[1].color.r, dashBarImage[1].color.g, dashBarImage[1].color.b, 255f), DashLerp * Time.deltaTime);
                dashBarImage[2].color = Color.Lerp(dashBarImage[2].color, new Color(dashBarImage[2].color.r, dashBarImage[2].color.g, dashBarImage[2].color.b, 255f), DashLerp * Time.deltaTime);
                break;
        }
    }
    private void UpdateUI()
    {
        float curHealthPercent = currentHealth / maxHealth.GetValue();
        float curManaPercent = currentMana / maxMana.GetValue();
        healthSlider.value = Mathf.Lerp(healthSlider.value, curHealthPercent, 10 * Time.deltaTime);
        manaSlider.value = Mathf.Lerp(manaSlider.value, curManaPercent, 10 * Time.deltaTime);
        bitAmountText.text = bitCount.ToString();
        if (!GameManager.instance.inKnightGame)
        {
            bitAmountTextShop.text = bitCount.ToString();
        }
        UpdateDashbar();
    }
}
