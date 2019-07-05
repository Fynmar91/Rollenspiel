using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
	[SerializeField]
	private CanvasGroup healthGroup;

	private IState currentState;

	private Transform target;

	public Transform MyTarget { get => target; set => target = value; }

	public float MyAttackRange { get; set; }

	protected void Awake()
	{
		MyAttackRange = 1;
		ChangeState(new IdleState());
	}

	protected override void Update()
	{
		currentState.Update();
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

	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
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
