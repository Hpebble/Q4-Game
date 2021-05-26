using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEye : MonoBehaviour
{
    public Boss boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            boss.TakeDamage(collision.GetComponent<Hurtbox>());
        }
    }
}
