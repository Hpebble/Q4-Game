using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private float baseValue = 0;

    public float GetValue()
    {
        return baseValue;
    }
    public void IncreaseValue(float value)
    {
        baseValue += value;
    }
}
