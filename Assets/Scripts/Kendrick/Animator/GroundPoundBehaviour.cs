using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundBehaviour : StateMachineBehaviour
{
    public float initUpForce;
    public float timeTillFall;
    public bool landState;
    private bool fall;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (landState)
        {
            Knight.instance.disableMovement = true;
            Knight.instance.isGroundPounding = true;
            animator.SetBool("GroundPounding", true);
        }
        else
        {
            fall = false;
            CombatManager.instance.canReceiveInput = false;
            Knight.instance.isGroundPounding = true;
            Knight.instance.StartCoroutine(UpThenFall());
            animator.SetBool("GroundPounding", true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (landState)
        {
        }
        else
        {
            if (fall)
            {
                Knight.instance.rb.velocity = new Vector2(0, -Knight.instance.stats.groundpoundSpeed);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (landState)
        {
            animator.SetBool("GroundPounding", false);
            Knight.instance.isGroundPounding = false;
            Knight.instance.disableMovement = false;
        }
        else
        {
            Knight.instance.StopCoroutine(UpThenFall());
            CombatManager.instance.canReceiveInput = false;
            animator.SetBool("GroundPounding", false);
            Knight.instance.disableMovement = false;
            Knight.instance.isGroundPounding = false;
        }
    }
    IEnumerator UpThenFall()
    {
        Knight.instance.rb.velocity = new Vector2(0, initUpForce);
        yield return new WaitForSeconds(timeTillFall);
        fall = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
