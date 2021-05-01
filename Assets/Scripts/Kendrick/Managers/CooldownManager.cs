﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : MonoBehaviour
{
    public static CooldownManager instance;
    public List<Cooldown> abilityOnCooldown = new List<Cooldown>();
    public Image UpSlashRadial;
    public Image UpSlashManaWarn;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        //DontDestroyOnLoad(this);
    }
    private void Start()
    {
        if (!GameManager.instance.inKnightGame)
        {
            this.enabled = false;
        }
            abilityOnCooldown.Add(new Cooldown("Dash", 0.5f));
            abilityOnCooldown.Add(new Cooldown("BasicAttack", 0.16f));
            abilityOnCooldown.Add(new Cooldown("UpSlash", Knight.instance.stats.UpSlashCooldown));
            abilityOnCooldown.Add(new Cooldown("GroundPound", 0.75f));
    }
    private void Update()
    {
        if (GameManager.instance.inKnightGame)
        {
            UpdateCooldownUI();
            for (int i = 0; i < abilityOnCooldown.Count; i++)
            {
                abilityOnCooldown[i].timer -= Time.deltaTime;

                if (abilityOnCooldown[i].timer <= 0)
                {
                    abilityOnCooldown[i].timer = 0;
                }
            }
        }
    }
    public bool  CheckOnCooldown(string cooldownName)
    {
        int i = abilityOnCooldown.FindIndex(d => d.cooldownName == cooldownName);
        Debug.Log(i);
        Debug.Log(abilityOnCooldown[0].name);
        if (i != -1 && abilityOnCooldown[i].cooldownName == cooldownName)
        {
            if(abilityOnCooldown[i].timer <= 0)
            {
                abilityOnCooldown[i].timer = abilityOnCooldown[i].length;
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("No cooldown name of '" + cooldownName + "' found");
            return true;
        }
    }
    public void StartCooldown(string cooldownName)
    {
        int i = abilityOnCooldown.FindIndex(d => d.cooldownName == cooldownName);
        abilityOnCooldown[i].timer = abilityOnCooldown[i].length;
    }
    public void ResetCooldown(string cooldownName)
    {
        int i = abilityOnCooldown.FindIndex(d => d.cooldownName == cooldownName);
        abilityOnCooldown[i].timer = 0;
    }
    void UpdateCooldownUI()
    {
        if (!Knight.instance.stats.CheckEnoughMana(Knight.instance.stats.UpSlashCost,false))
        {
            UpSlashManaWarn.enabled = true;
        }
        else UpSlashManaWarn.enabled = false;
        UpSlashRadial.fillAmount = abilityOnCooldown[2].timer / Knight.instance.stats.UpSlashCooldown;
        //Debug.Log(abilityOnCooldown[2].timer / Knight.instance.stats.UpSlashCooldown);
    }
}