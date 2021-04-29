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
    public bool isAttacking;
    public float waitTime;


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

            if (isAttacking == false) 
            {
                Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y) + offset;
                Vector2 smoothMove = Vector2.Lerp(transform.position, playerPos, batSpeed * Time.deltaTime);
                //Debug.Log("tracking player");

                // Debug.Log("Player's x = " + Mathf.Round(playPosX));
                // Debug.Log("Enemy's x = " +  Mathf.Round(enemPosX));

                transform.position = smoothMove;
            }
            



            if (attackState == true && isCoolDown == false) 
            {

                StartCoroutine(AttackAnim());
                Debug.Log("Test");

            }

            if (isCoolDown == true)
            {
                Attackcooldown += Time.deltaTime;
            }
            if (Attackcooldown >= 1.5f)
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
        //Swipe Attack
        Debug.Log("Enemy attack");
        
        SwipePosition1 = transform.position;
        SwipePosition2 = Knight.instance.transform.position;

        Vector2 DesiredPos = new Vector2(SwipePosition2.x, SwipePosition2.y);
        Vector2 BatPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 BatPosLerp1 = Vector2.Lerp(SwipePosition1, DesiredPos, AttackSpeed * Time.deltaTime);

        Debug.Log("Test2");
        isAttacking = true;
        transform.position = BatPosLerp1;

        yield return new WaitForSeconds(waitTime);




        isAttacking = false;
        isCoolDown = true;
    }

    public void DropKnife()
    {

    }

}
