using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float cameraSpeed;
    public bool onMonitor = false;
    public Transform monitorPos;
    public float moveSpeed;
    public Vector3 offset2;
    public Quaternion offsetRotate;
    public Transform CameraOrigin;
    public Quaternion offsetRotate2;
    public Vector3 mousePos;
    public Vector3 MouseOffset;
    public GameObject mouseOffsetVis;
    public Canvas OSCanvas;
    public Camera cam;
   
    // Start is called before the first frame update
    void Start()
    {
        onMonitor = false;
    }

    // Update is called once per frame
    void Update()
    {
       

        if (!onMonitor)
        {
            Vector3 DesiredPosition = new Vector3(player.position.x, player.position.y, 0) + offset;
            Vector3 SmoothedPos = Vector3.Lerp(transform.position, DesiredPosition, cameraSpeed);

            Quaternion orignalRotation = Quaternion.Euler(CameraOrigin.rotation.x + offsetRotate2.x, CameraOrigin.rotation.y + offsetRotate2.y, CameraOrigin.rotation.z + offsetRotate2.z);

            transform.position = SmoothedPos;
            transform.rotation = orignalRotation;
        }

        if (onMonitor)
        {
            Vector3 MonitorPosXYZ = new Vector3(monitorPos.position.x, monitorPos.position.y, 0) + offset2;
            Vector3 MonitorPos = Vector3.Lerp(transform.position, MonitorPosXYZ, moveSpeed);
            
            Quaternion MonitorRotationXYZ = Quaternion.Euler(monitorPos.rotation.x + offsetRotate.x, monitorPos.rotation.y + offsetRotate.y, monitorPos.rotation.z + offsetRotate.z);
            Quaternion MonitorRota = Quaternion.Slerp(transform.rotation, MonitorRotationXYZ, moveSpeed);
            
            transform.position = MonitorPos;
            transform.rotation = MonitorRota;


            

        }

       
        
    }
    public void onMonitorButton()
    {
        onMonitor = true;
    }

    public void onMonitorButtonLeave()
    {
        onMonitor = false;
    }
}
