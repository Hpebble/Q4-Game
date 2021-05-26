using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour
{
    bool collected;
    public float slerpSpeed;
    public float deleteDistance;
    public bool collectable;
    public float timeTillCollectable;
    public Vector2 speedOnSpawnRange;
    private bool delete;
    private TrailRenderer tr;
    private Animation anim;
    private Rigidbody2D rb;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && collectable)
        {
            collected = true;
        }
    }
    private void Start()
    {
        tr = this.GetComponentInChildren<TrailRenderer>();
        anim = this.GetComponentInChildren<Animation>();
        rb = this.GetComponent<Rigidbody2D>();
        if (speedOnSpawnRange.y != 0)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            float speedOnSpawn = Random.Range(speedOnSpawnRange.x, speedOnSpawnRange.y);
            Vector2 veloOnSpawn = new Vector2(dir.x * speedOnSpawn, dir.y * speedOnSpawn);
            rb.velocity = veloOnSpawn;
            
        }
    }
    private void Update()
    {
        if(!collected && timeTillCollectable > 0)
        {
            tr.autodestruct = false;
            timeTillCollectable -= Time.deltaTime;
        }
        else if (timeTillCollectable <= 0)
        {
            collectable = true;
        }
    }
    void FixedUpdate()
    {
        if (collected )
        {
            tr.autodestruct = true;
            if (Vector3.Distance(Knight.instance.GetCenter(), this.transform.position) < deleteDistance && delete == false)
            {
                delete = true;
                if (delete == true)
                {
                    AudioManager.instance.PlayOneshot("OrbPickup", 0.8f, 1f);
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
        GameManager.instance.currentBits++;
        tr.transform.SetParent(null);
        Destroy(this.gameObject);
    }
    public static float EaseIn(float t)
    {
        return t * t;
    }
}
