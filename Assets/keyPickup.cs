using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyPickup : MonoBehaviour
{
    public GameObject key;
    public GameObject Knight;
    public GameObject Door;
    public ParticleSystem keyParticle;
    // Start is called before the first frame update
    void Start()
    {
        key.SetActive(true);
        Door.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Player has touched the key");
            key.SetActive(false);
            Door.SetActive(false);
            keyParticle.Play();
        }
    }
}
