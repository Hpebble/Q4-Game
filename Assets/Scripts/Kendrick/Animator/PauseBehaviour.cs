using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehaviour : StateMachineBehaviour
{
    public bool pause;
    public bool abrupt;
    public bool enteringGame;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (pause)
        {
            GameManager.instance.paused = true;
        }
        else
        {
            GameManager.instance.paused = false;
        }
        if (abrupt)
        {
           // GameManager.instance.paused = true;
        }
        if (enteringGame)
        {
            GameManager.instance.paused = false;
        }
    }

    //nStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (abrupt)
        {
            Time.timeScale = 0f;
        }
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (abrupt)
        {
            Time.timeScale = 1f;
            GameManager.instance.paused = false;
            //GameManager.instance.StartCoroutine(GameManager.instance.ToggleKnightGame());
            //GameManager.instance.ToggleKnightGame();
        }
        else if (enteringGame)
        {
            //GameManager.instance.StartCoroutine(GameManager.instance.ToggleKnightGame());
            //GameManager.instance.ToggleKnightGame();
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
