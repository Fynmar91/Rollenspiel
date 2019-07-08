using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
	[SerializeField]
	private float initSpeed;

	[SerializeField]
	protected Stat health;

	[SerializeField]
	protected Transform hitBox;

	[SerializeField]
	private float initHealth = 100f;

	private Rigidbody2D myRigidbody;
	protected Coroutine attackRoutine;
	private bool isSlowed = false;
	private float slowTime = 0;
	private float speed;

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
		myRigidbody = GetComponent<Rigidbody2D>();
		MyAnimator = GetComponent<Animator>();
		health.Initialize(initHealth, initHealth);
		speed = initSpeed;
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		HandleLayers();

		if (isSlowed)
		{
			slowTime += Time.deltaTime;
			if (slowTime > 6)
			{
				isSlowed = false;
				slowTime = 0;
				speed = initSpeed;
			}
		}
	}

	protected virtual void FixedUpdate()
	{
		Move();
	}

	public void Move()
	{
		if (IsAlive)
		{
			myRigidbody.velocity = MyDirection.normalized * MySpeed;
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

	public virtual void TakeDamage(float damage, float slowEffect, Transform source)
	{
		health.MyCurrentValue -= damage;

		if (slowEffect > 0)
		{
			speed = initSpeed * (1 - slowEffect);
			isSlowed = true;
			slowTime = 0;
		}
		

		if (health.MyCurrentValue <= 0)
		{
			MyDirection = Vector2.zero;
			myRigidbody.velocity = MyDirection;
			transform.GetComponent<BoxCollider2D>().enabled = false;
			MyAnimator.SetTrigger("die");
		}
	}
}
