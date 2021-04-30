using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    EdgeCollider2D playerCol;
    Collider2D platformCol;
    bool dropping;
    // Start is called before the first frame update
    void Start()
    {
        playerCol = Knight.instance.GetComponentInChildren<EdgeCollider2D>();
        platformCol = this.gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Knight.instance.IsDownPressed() && !Knight.instance.CheckIfActionCurrentlyTaken() && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(IgnoreCol());
        }
        else
        {
            if(!dropping)
            Physics2D.IgnoreCollision(playerCol, platformCol, false);
        }
    }
    IEnumerator IgnoreCol()
    {
        dropping = true;
        Physics2D.IgnoreCollision(playerCol, platformCol, true);
        yield return new WaitForSeconds(0.1f);
        Physics2D.IgnoreCollision(playerCol, platformCol, false);
        dropping = false;
    }
}
