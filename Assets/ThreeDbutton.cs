using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThreeDbutton : MonoBehaviour
{
    public UnityEvent unityEvent = new UnityEvent();
    public GameObject Button;
    // Start is called before the first frame update
    void Start()
    {
        Button = gameObject;
    }

    // Update is called once per frame
    void Update()
    { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                unityEvent.Invoke();
            }
        }
    }
}
