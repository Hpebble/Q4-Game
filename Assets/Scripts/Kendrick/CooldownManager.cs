using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    public static CooldownManager instance;
    public List<Cooldown> abilityOnCooldown = new List<Cooldown>();
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
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        abilityOnCooldown.Add(new Cooldown("Dash", 0.5f));
        abilityOnCooldown.Add(new Cooldown("BasicAttack", 0.1f));
    }
    private void Update()
    {
        for (int i = 0; i < abilityOnCooldown.Count; i++)
        {
            abilityOnCooldown[i].timer -= Time.deltaTime;

            if (abilityOnCooldown[i].timer <= 0)
            {
                abilityOnCooldown[i].timer = 0;
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
}
