using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSourceBehaviour : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		int x = Mathf.RoundToInt(animator.GetFloat("x"));
		int y = Mathf.RoundToInt(animator.GetFloat("y"));

		if (x == 0 && y == -1)
		{
			animator.GetComponent<Player>().MyExitIndex = 2;
		}
		else if (x == 0 && y == 1)
		{
			animator.GetComponent<Player>().MyExitIndex = 0;
		}
		else if (x == 1 && y == 0)
		{
			animator.GetComponent<Player>().MyExitIndex = 1;
		}
		else if (x == -1 && y == 0)
		{
			animator.GetComponent<Player>().MyExitIndex = 3;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
		
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//	// Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//	// Implement code that sets up animation IK (inverse kinematics)
	//}
}
