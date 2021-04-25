using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : ScriptableObject
{
    public string cooldownName;
    public float timer;
    public float length;

    public Cooldown(string CooldownName, float TimerLength)
    {
        cooldownName = CooldownName;
        timer = 0;
        length = TimerLength;
    }
}
