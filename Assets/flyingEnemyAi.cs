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
    public Vector2 SwipePosition1;
    public Vector2 SwipePosition2;
    public float AttackSpeed;
    public bool isCoolDown;
    public float Attackcooldown;
    public float AttackSpeed2;
    public bool attackGoback;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playPosX = player.transform.localPosition.x;
        float enemPosX = transform.localPosition.x;

        {


            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y) + offset;
            Vector2 smoothMove = Vector2.Lerp(transform.position, playerPos, batSpeed);
            //Debug.Log("tracking player");

            // Debug.Log("Player's x = " + Mathf.Round(playPosX));
            // Debug.Log("Enemy's x = " +  Mathf.Round(enemPosX));

            transform.position = smoothMove;



            if (attackState == true && isCoolDown == false) 
            {

                StartCoroutine(AttackAnim());
                Debug.Log("Test");

            }

            if (isCoolDown == true)
            {
                Attackcooldown += Time.deltaTime;
            }
            if (Attackcooldown >= 4)
            {
                isCoolDown = false;
                Attackcooldown = 0;
            }

            if (Mathf.Round(enemPosX) == Mathf.Round(playPosX))
            {
                Debug.Log("Dropping Knife.");
            }

        }
    }

    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            attackState = true;
            Debug.Log("In radius");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            attackState = false;
            Debug.Log("Not in radius");
        }
    }

    IEnumerator AttackAnim()
    {
        Debug.Log("Enemy attack lol");
        
        SwipePosition1 = transform.position;
        SwipePosition2 = Knight.instance.transform.position;

       
        Vector2 BatPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 BatPosLerp1 = Vector2.Lerp(SwipePosition1, SwipePosition2, AttackSpeed);

        Debug.Log("Test2");
        transform.position = BatPosLerp1;

        yield return new WaitForSeconds(1f);

        SwipePosition1 = transform.position;
        Vector2 BatPosLerp2 = Vector2.Lerp(SwipePosition2, SwipePosition1, AttackSpeed2);
        transform.position = BatPosLerp2;
        Debug.Log("Test2(2)");
        attackGoback = false;
        
       
        

        isCoolDown = true;




    }

}
