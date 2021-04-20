using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    private Vector3 TargetPos;
    public GameObject FollowTarget;
    void FixedUpdate()
    {
        //transform.position = Vector3.Slerp(transform.position, newPosition, (FollowSpeed * Time.deltaTime)/2);
        TargetPos = new Vector3(FollowTarget.transform.position.x, FollowTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, TargetPos, FollowSpeed * Time.deltaTime);
    }
}
