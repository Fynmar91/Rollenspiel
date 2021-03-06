﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
	[SerializeField]
	private CanvasGroup healthGroup;

	[SerializeField]
	private float initAggroRange;

	private IState currentState;

	public float MyAttackRange { get; set; }

	public float MyAttackTime { get; set; }

	public float MyAggroRange { get; set; }

	public Vector3 MyStartPosition { get; set; }

	public bool InRange
	{
		get
		{
			return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
		}
	}

	protected void Awake()
	{
		MyStartPosition = transform.position;
		MyAggroRange = initAggroRange;
		MyAttackRange = 1.0f;
		ChangeState(new IdleState());
	}

	protected override void Update()
	{
		if (IsAlive)
		{
			if (!IsAttacking)
			{
				MyAttackTime += Time.deltaTime;
			}

			currentState.Update();
		}
			base.Update();
	}

	public override Transform Select()
	{
		healthGroup.alpha = 1;
		return base.Select();
	}

	public override void Deselect()
	{
		base.Deselect();
		healthGroup.alpha = 0;
	}

	public override void TakeDamage(float damage, float slowEffect, Transform source)
	{
		if (!(currentState is EvadeState))
		{
			SetTarget(source);
			base.TakeDamage(damage, slowEffect, source);
			OnHealthChanged(health.MyCurrentValue);
		}
	}

	public void ChangeState(IState newState)
	{
		if (currentState != null)
		{
			currentState.Exit();
		}

		currentState = newState;

		currentState.Enter(this);
	}

	public void SetTarget(Transform target)
	{
		if (MyTarget == null && !(currentState is EvadeState))
		{
			float distance = Vector2.Distance(transform.position, target.position);
			MyAggroRange = initAggroRange;
			MyAggroRange += distance;
			MyTarget = target;
		}
	}

	public void Reset()
	{
		this.MyTarget = null;
		this.MyAggroRange = initAggroRange;
		this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
		OnHealthChanged(health.MyCurrentValue);
	}
}
