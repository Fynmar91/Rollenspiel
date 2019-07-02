using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
	[SerializeField]
	private float speed = 5;
	protected Vector2 direction;
	private Rigidbody2D myRigidbody;
	protected Animator myAnimator;
	protected bool isAttacking = false;
	protected Coroutine attackRoutine;
	public bool isMoving
	{
		get
		{
			return direction.x != 0 || direction.y != 0;
		}
	}

	// Start is called before the first frame update
	protected virtual void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
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
		myRigidbody.velocity = direction.normalized * speed;
	}

	public void HandleLayers()
	{
		if (isMoving)
		{
			StopAttack();
			ActivateLayer("WalkLayer");
			myAnimator.SetFloat("x", direction.x);
			myAnimator.SetFloat("y", direction.y);
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

	public void StopAttack()
	{
		if (isAttacking)
		{
			StopCoroutine(attackRoutine);
			Debug.Log("Attack stopped");
			isAttacking = false;
			myAnimator.SetBool("attack", isAttacking);
		}		
	}
}
