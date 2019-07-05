using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
	private Enemy parent;

	public void Enter(Enemy parent)
	{
		this.parent = parent;
	}

	public void Exit()
	{
		
	}

	public void Update()
	{
		if (parent.MyTarget != null)
		{
			float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

			if (distance >= parent.MyAttackRange)
			{
				parent.ChangeState(new FollowState());
			}
		}
		else
		{
			parent.ChangeState(new IdleState());
		}
	}
}
