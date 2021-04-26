using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class TransitionOneBehaviour : StateMachineBehaviour
{
    public string TriggerName;
    public bool BasicAttack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CombatManager.instance.canReceiveInput = true;
        if (CombatManager.instance.inputReceived && TriggerName != "none")
        {
            animator.SetTrigger(TriggerName);
            CombatManager.instance.InputManager();
            CombatManager.instance.inputReceived = false;
        }
        else
        {
            animator.SetBool("InTransition", true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (CombatManager.instance.inputReceived && TriggerName != "none")
        {
            animator.SetTrigger(TriggerName);
            CombatManager.instance.InputManager();
            CombatManager.instance.inputReceived = false;
        }
        */
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("InTransition", false);
        animator.SetTrigger("ExitTransition");
        if (BasicAttack)
        {
            CooldownManager.instance.StartCooldown("BasicAttack");
        }
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
