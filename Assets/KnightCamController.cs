using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCamController : MonoBehaviour
{
    [Header("CamFollow")]
    public float followSpeed = 2f;
    public GameObject followTarget;
    public float camZoom;
    public float camZoomSpeed;
    public GameObject defaultFollowTarget;
    public float defaultZoom;
    private Vector3 TargetPos;
    public Camera cam;
    private void Awake()
    {
        defaultZoom = camZoom;
        defaultFollowTarget = followTarget;
    }
    private void Start()
    {
    }
    void FixedUpdate()
    {
        if (followTarget == defaultFollowTarget)
        {
            //transform.position = Vector3.Slerp(transform.position, newPosition, (FollowSpeed * Time.deltaTime)/2);
            TargetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
            cam.transform.position = Vector3.Lerp(cam.transform.position, TargetPos, followSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camZoom, camZoomSpeed * Time.deltaTime);
        }
        else if (followTarget != null)
        {
            TargetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
            cam.transform.position = Vector3.Lerp(cam.transform.position, TargetPos, followSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camZoom, camZoomSpeed * Time.deltaTime);
        }
        else if (followTarget == null)
        {
            followTarget = defaultFollowTarget;
        }
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -12f);
    }
}
