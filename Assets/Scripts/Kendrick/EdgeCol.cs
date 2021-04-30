using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCol : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Knight.instance.rb.velocity.y < 0.1)
        {
            Knight.instance.edgeColColliding = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Knight.instance.edgeColColliding = false;
    }
}
