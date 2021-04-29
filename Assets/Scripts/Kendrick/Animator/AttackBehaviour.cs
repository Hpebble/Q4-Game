using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    public float forceForward;
    public float forceLength;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("ExitTransition");
        CooldownManager.instance.ResetCooldown("BasicAttack");
        CombatManager.instance.inputReceived = false;
        Knight.instance.rb.velocity = Vector2.zero;
        Knight.instance.disableMovement = true;
        Knight.instance.attacking = true;
        Knight.instance.rb.gravityScale = 0;
        Knight.instance.StartCoroutine(ForcePush(animator));
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Knight.instance.disableMovement = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //CombatManager.instance.inputReceived = false;
        Knight.instance.disableMovement = false;
        Knight.instance.attacking = false;
        Knight.instance.rb.gravityScale = 5;
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
    IEnumerator ForcePush(Animator animator)
    {
        Knight.instance.rb.AddForce(new Vector2((Knight.instance.directionFacing * forceForward), 0));
        yield return new WaitForSeconds(forceLength);
        if (animator.GetBool("Attacking") && !animator.GetBool("TakingDamage"))
        {
            Knight.instance.rb.velocity = Vector2.zero;
        }
    }
}
