﻿using System;
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

			float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

			if (distance <= parent.MyAttackRange)
			{
				parent.ChangeState(new AttackState());
			}
		}
		else
		{
			parent.ChangeState(new IdleState());
		}
		
	}
}
