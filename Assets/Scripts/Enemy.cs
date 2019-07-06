using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
	[SerializeField]
	private CanvasGroup healthGroup;

	private IState currentState;

	public float MyAttackRange { get; set; }

	public float MyAttackTime { get; set; }

	protected void Awake()
	{
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

	public override void TakeDamage(float damage, Transform source)
	{
		base.TakeDamage(damage, source);
		OnHealthChanged(health.MyCurrentValue);
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
}
