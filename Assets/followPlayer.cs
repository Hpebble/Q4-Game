using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform bg;
    public Transform Knight;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bg.transform.position = new Vector3 (Knight.position.x, bg.position.y, Knight.position.z);
    }
}
