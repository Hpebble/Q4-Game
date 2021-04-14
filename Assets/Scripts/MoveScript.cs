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
    public float dlx;
    public bool isCollide;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");


        



        if (x > 0 && !isCollide) 
        {
            dlx += rotateSpeed;


            DirectionalLight.rotation = Quaternion.Euler(-1038, dlx, DirectionalLight.rotation.z);

            Debug.Log(dlx += rotateSpeed);

        }

        if (x == -1 && !isCollide) 
        {
            dlx -= rotateSpeed;


            DirectionalLight.rotation = Quaternion.Euler(-1038, dlx, DirectionalLight.rotation.z);
            Debug.Log(dlx -= rotateSpeed);
        }


        rb.velocity = new Vector3(x * speed, rb.velocity.y, z * speed);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("wall"))
        {
            isCollide = true;
        }
        else if (collision.gameObject.tag.Equals("floor") || collision.gameObject.tag != ("wall"))
        {
            isCollide = false;
        }
    }
  
}

