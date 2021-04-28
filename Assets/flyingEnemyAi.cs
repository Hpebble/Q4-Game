using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEnemyAi : MonoBehaviour
{
    public GameObject player;
    public float batSpeed;
    public Vector2 offset;
    public bool attackState;
    private float playPosX ;
    private float enemPosX;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playPosX = player.transform.localPosition.x;
        float enemPosX = transform.localPosition.x;
        if (player.transform.position.x != transform.localPosition.x && attackState == false) 
        {

            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y) + offset;
            Vector2 smoothMove = Vector2.Lerp(transform.position, playerPos, batSpeed);
            Debug.Log("tracking player");

            
            

            Debug.Log("Player's x = " + Mathf.Round(playPosX));
            Debug.Log("Enemy's x = " +  Mathf.Round(enemPosX));

            transform.position = smoothMove;


        }

        if (Mathf.Round(playPosX) == Mathf.Round(enemPosX))
        {
            attackState = true;
        }
        else
        {
            attackState = false;
        }

        if(attackState == true)
        {
            Debug.Log("Enemy Attacking");
        }

       

        
        

    }
}
