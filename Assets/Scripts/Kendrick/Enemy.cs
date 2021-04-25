using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;

    public float playerDamage;

    public abstract void Attack();
}
