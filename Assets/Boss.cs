using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Boss instance;
    public float maxHealth;
    public float currentHealth;
    public float laserSpeed;
    private bool laserOn;
    public bool attacking;
    public enum BossState { idle, tentacleBarrage, laserBeam, particleAttack, enemyPhase, damagePhase};
    public BossState currentState;

    public int attackCounter;

    //Timings
    public float timeBeforeNextState;
    [SerializeField]
    private float timeBeforeNextStateCD;
    public float damagePhasetime;

    public GameObject eye;
    public GameObject tentacle1;
    public GameObject tentacle2;
    public GameObject tentacle3;
    public bool dead;
    private Animator anim;
    private LineRenderer laserLr;
    private Vector3 directionToPlayer;
    int attackToPlay;
    void Start()
    {
        instance = this.GetComponent<Boss>();
        anim = this.GetComponent<Animator>();
        laserLr = this.GetComponentInChildren<LineRenderer>();
        timeBeforeNextStateCD = timeBeforeNextState;
    }
    void Update()
    {
        if (!dead)
        {
            if (attacking)
            {
                switch (currentState)
                {
                    case BossState.idle:
                        if (attackCounter < 2)
                        {
                            if (timeBeforeNextStateCD <= 0)
                            {
                                RandomAttack();
                                timeBeforeNextStateCD = timeBeforeNextState;
                            }
                            else
                            {
                                timeBeforeNextStateCD -= Time.deltaTime;
                            }
                        }
                        else
                        {
                            StartCoroutine(DamagePhase());
                        }

                        break;

                    case BossState.tentacleBarrage:
                        break;

                    case BossState.laserBeam:
                        break;

                    case BossState.particleAttack:
                        break;

                    case BossState.enemyPhase:
                        break;

                    case BossState.damagePhase:
                        break;
                }

                directionToPlayer = Knight.instance.GetCenter() - eye.transform.position;

                Quaternion playerDir = Quaternion.LookRotation(directionToPlayer, Vector3.left);

                float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

                if (laserOn)
                {
                    anim.SetBool("LaserOn", true);
                    laserLr.transform.rotation = Quaternion.RotateTowards(laserLr.transform.rotation, q, Time.deltaTime * laserSpeed);
                }
                else
                {
                    anim.SetBool("LaserOn", false);
                }
            }
            if(currentHealth <= 0)
            {
                dead = true; GameManager.instance.ToggleKnightGame();
            }
        }
    }
    IEnumerator TentacleAttack()
    {
        currentState = BossState.tentacleBarrage;

        StartCoroutine(TripleBarrage());
        yield return new WaitForSeconds(2f);
        StartCoroutine(TripleBarrage());
        yield return new WaitForSeconds(2f);
        StartCoroutine(TripleBarrage());
        yield return new WaitForSeconds(2f);

        attackCounter++;
        currentState = BossState.idle;
    }
    IEnumerator TripleBarrage()
    {
        tentacle1.transform.position = new Vector2(Knight.instance.transform.position.x, tentacle1.transform.position.y);
        yield return new WaitForSeconds(0.25f);
        anim.SetTrigger("tentacle1");

        tentacle2.transform.position = new Vector2(Knight.instance.transform.position.x, tentacle2.transform.position.y);
        yield return new WaitForSeconds(0.25f);
        anim.SetTrigger("tentacle2");

        tentacle3.transform.position = new Vector2(Knight.instance.transform.position.x, tentacle3.transform.position.y);
        yield return new WaitForSeconds(0.25f);
        anim.SetTrigger("tentacle3");
    }

    IEnumerator LaserBeamAttack()
    {
        currentState = BossState.laserBeam;
        anim.SetBool("eyeOpen", true);

        StartCoroutine(LaserBurst(3f));
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(LaserBurst(3f));
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(LaserBurst(3f));
        yield return new WaitForSeconds(3.5f);

        anim.SetBool("eyeOpen", false);
        yield return new WaitForSeconds(1f);

        attackCounter++;
        currentState = BossState.idle;
    }
    IEnumerator LaserBurst(float burstLength)
    {
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        laserLr.transform.rotation = q;
        yield return new WaitForSeconds(0f);
        laserOn = true;
        yield return new WaitForSeconds(burstLength);
        laserOn = false;
    }
    IEnumerator DamagePhase()
    {
        anim.SetBool("eyeOpen", true);
        currentState = BossState.damagePhase;
        yield return new WaitForSeconds(damagePhasetime);
        attackCounter = 0;
        anim.SetBool("eyeOpen", false);
        currentState = BossState.idle;
    }
    public void RandomAttack()
    {
        int attackToPlay = Random.Range(0, 2);
        Debug.Log("Played" + attackToPlay);
        switch (attackToPlay)
        {
            case 1:
                StartCoroutine(TentacleAttack());
                break;

            case 0:
                StartCoroutine(LaserBeamAttack());
                break;
        }
    }
    public void TakeDamage(Hurtbox hurtbox)
    {
        currentHealth -= hurtbox.damage;
        AudioManager.instance.Play("Hit1", 0.85f, 1.15f);
    }
}
