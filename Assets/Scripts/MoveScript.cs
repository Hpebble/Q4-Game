using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public float speed;
    float x;
    float z;
    public Rigidbody rb;
    public Transform DirectionalLight;
    public float rotateSpeed;
    public float goingRight;
    public float goingLeft;
    public float dlxx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        dlxx += rotateSpeed;
        DirectionalLight.rotation = Quaternion.Euler(dlxx, DirectionalLight.rotation.y, DirectionalLight.rotation.x);
        Debug.Log(DirectionalLight.rotation);
        
        
        if (x > 0)
        {
           
          
        }

        if (x == -1)
        { 
                       
        }

        rb.velocity = new Vector3(x * speed, rb.velocity.y, z * speed);
    }
}
