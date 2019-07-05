using System.Collections;
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

	private Rigidbody2D myRigidbody;
	protected Animator myAnimator;
	protected bool isAttacking = false;
	protected Coroutine attackRoutine;

	private Vector2 direction;

	public Stat MyHealth
	{
		get { return health; }
	}

	public bool isMoving
	{
		get	{return direction.x != 0 || direction.y != 0;}
	}

	public Vector2 MyDirection { get => direction; set => direction = value; }

	public float MySpeed { get => speed; set => speed = value; }

	// Start is called before the first frame update
	protected virtual void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
		health.Initialize(initHealth, initHealth);
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		HandleLayers();
	}

	private void FixedUpdate()
	{
		Move();
	}

	public void Move()
	{
		myRigidbody.velocity = MyDirection.normalized * MySpeed;
	}

	public void HandleLayers()
	{
		if (isMoving)
		{
			StopAttack();
			ActivateLayer("WalkLayer");
			myAnimator.SetFloat("x", MyDirection.x);
			myAnimator.SetFloat("y", MyDirection.y);
		}
		else if (isAttacking)
		{
			ActivateLayer("AttackLayer");
		}
		else
		{
			ActivateLayer("IdleLayer");
		}
	}

	public void ActivateLayer(string layerName)
	{
		for (int i = 0; i < myAnimator.layerCount; i++)
		{
			myAnimator.SetLayerWeight(i, 0);
		}
		myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
	}

	virtual public void StopAttack()
	{
		if (isAttacking)
		{
			StopCoroutine(attackRoutine);
			isAttacking = false;
			myAnimator.SetBool("attack", isAttacking);
		}
	}

	public virtual void TakeDamage(float damage)
	{
		health.MyCurrentValue -= damage;

		if (health.MyCurrentValue <= 0)
		{
			myAnimator.SetTrigger("die");
			ActivateLayer("DeathLayer");
		}
	}
}
