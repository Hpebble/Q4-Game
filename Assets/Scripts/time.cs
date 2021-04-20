using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class time : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public int hour;
    public int minute;
    public int Seconds;
    public float timeAMPM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;
        Seconds = System.DateTime.Now.Second;
       


        timeText.text = (" " + hour + ":" + minute + ":" + Seconds);
    }
}
