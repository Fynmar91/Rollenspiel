﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
	[SerializeField]
	private float speed;

	[SerializeField]
	protected Stat health;

	[SerializeField]
	protected Transform hitBox;

	[SerializeField]
	private float initHealth = 100f;

	private Rigidbody2D rigidbody;
	protected Coroutine attackRoutine;

	public Transform MyTarget { get; set; }

	public Stat MyHealth
	{
		get { return health; }
	}

	private Vector2 direction;

	public bool isMoving
	{
		get	{return direction.x != 0 || direction.y != 0;}
	}

	public Animator MyAnimator { get; set; }

	public bool IsAttacking { get; set; }

	public Vector2 MyDirection { get => direction; set => direction = value; }

	public float MySpeed { get => speed; set => speed = value; }

	public bool IsAlive
	{
		get { return MyHealth.MyCurrentValue > 0; }
	}

	// Start is called before the first frame update
	protected virtual void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		MyAnimator = GetComponent<Animator>();
		health.Initialize(initHealth, initHealth);
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		HandleLayers();
	}

	protected virtual void FixedUpdate()
	{
		Move();
	}

	public void Move()
	{
		if (IsAlive)
		{
			rigidbody.velocity = MyDirection.normalized * MySpeed;
		}
		
	}

	public void HandleLayers()
	{
		if (IsAlive)
		{
			if (isMoving)
			{
				ActivateLayer("WalkLayer");
				MyAnimator.SetFloat("x", MyDirection.x);
				MyAnimator.SetFloat("y", MyDirection.y);
			}
			else if (IsAttacking)
			{
				ActivateLayer("AttackLayer");
			}
			else
			{
				ActivateLayer("IdleLayer");
			}
		}		
		else
		{
			ActivateLayer("DeathLayer");
		}
	}

	public void ActivateLayer(string layerName)
	{
		for (int i = 0; i < MyAnimator.layerCount; i++)
		{
			MyAnimator.SetLayerWeight(i, 0);
		}
		MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
	}

	public virtual void TakeDamage(float damage, Transform source)
	{
		health.MyCurrentValue -= damage;

		if (health.MyCurrentValue <= 0)
		{
			MyDirection = Vector2.zero;
			rigidbody.velocity = MyDirection;
			MyAnimator.SetTrigger("die");
		}
	}
}
