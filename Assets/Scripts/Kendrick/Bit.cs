using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour
{
    bool collected;
    public float slerpSpeed;
    public float deleteDistance;
    private bool delete;
    private TrailRenderer tr;
    private Animation anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collected = true;
        }
    }
    private void Start()
    {
        tr = this.GetComponentInChildren<TrailRenderer>();
        anim = this.GetComponentInChildren<Animation>();
    }
    void FixedUpdate()
    {
        if (collected)
        {
            if (Vector3.Distance(Knight.instance.GetCenter(), this.transform.position) < deleteDistance && delete == false)
            {
                delete = true;
                if (delete == true)
                {
                    anim.Play();
                    StartCoroutine(DestroyThis());
                }
                //Destroy(this.gameObject);
            }
            if (!delete)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, Knight.instance.GetCenter(), slerpSpeed * Time.fixedDeltaTime);
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, Knight.instance.GetCenter(), slerpSpeed * 3 * Time.fixedDeltaTime);
            }
        }
    }
    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(0.06f);
        Knight.instance.stats.bitCount++;
        tr.transform.SetParent(null);
        Destroy(this.gameObject);
    }
    public static float EaseIn(float t)
    {
        return t * t;
    }
}
