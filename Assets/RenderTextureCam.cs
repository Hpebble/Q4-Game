using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureCam : MonoBehaviour
{
    Camera rendCam;
    public RenderTexture rendTex;
    // Start is called before the first frame update
    void Start()
    {
        rendCam = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
