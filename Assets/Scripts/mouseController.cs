using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseController : MonoBehaviour
{
    public static GameObject mouseCursor;
    public Camera cam;
    public static RectTransform dragTransform;
    public LayerMask layermask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.position = hit.point;

            

        }

      
            
       
    }
}
