using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour
{
    bool collected;
    public float slerpSpeed;
    public float deleteDistance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collected = true;
        }
    }
    void Update()
    {
        if (collected)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, Knight.instance.GetCenter(), slerpSpeed * Time.deltaTime);
            if(Vector3.Distance(Knight.instance.GetCenter(), this.transform.position) < deleteDistance)
            {
                Knight.instance.stats.bitCount++;
                Destroy(this.gameObject);
            }
        }
    }
    public static float EaseIn(float t)
    {
        return t * t;
    }
}
