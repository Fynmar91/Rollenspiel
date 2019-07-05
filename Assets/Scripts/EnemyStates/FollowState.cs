using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FollowState : IState
{
	private Enemy parent;

	public void Enter(Enemy parent)
	{
		this.parent = parent;
	}

	public void Exit()
	{
		parent.MyDirection = Vector2.zero;
	}

	public void Update()
	{
		if (parent.MyTarget != null)
		{
			parent.MyDirection = (parent.MyTarget.transform.position - parent.transform.position).normalized;
			parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyTarget.position, parent.MySpeed * Time.deltaTime);
		}
		else
		{
			parent.ChangeState(new IdleState());
		}
		
	}
}
