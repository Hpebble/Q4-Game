using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] positions;
    public float speed;
    public Transform startPos;
    public float idleTime;
    public float idleCD;
    public bool posReached;

    Vector3 nextPos;
    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        idleCD -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        MovePlatform();
    }
    void MovePlatform()
    {
        if(transform.position == positions[0].position)
        {
            nextPos = positions[1].position;
            if (!posReached)
            {
                idleCD = idleTime;
                posReached = true;
            }
        }
        if (transform.position == positions[1].position)
        {
            nextPos = positions[0].position;
            if (!posReached)
            {
                idleCD = idleTime;
                posReached = true;
            }
        }
        if (idleCD <= 0)
        {
            posReached = false;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.fixedDeltaTime);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(positions[0].position, positions[1].position);
    }
}
