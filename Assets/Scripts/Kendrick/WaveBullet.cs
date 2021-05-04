using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBullet : MonoBehaviour
{
    public float speed;
    public float dir;
    public float lifeTime;
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (dir == 0)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
            if (dir == -1)
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            if (dir == 1)
            {
                this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            rb.velocity = new Vector2(speed * dir, 0);
        }
    }
}
