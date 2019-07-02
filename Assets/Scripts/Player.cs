using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	[SerializeField]
	private Stat health;
	[SerializeField]
	private Stat mana;
	private float initHealth = 100f;
	private float initMana = 100f;

	// Start is called before the first frame update
	protected override void Start()
	{
		health.Initialize(initHealth, initHealth);
		mana.Initialize(initMana, initMana);
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		GetInput();
		base.Update();
	}

	private void GetInput()
	{
		direction = Vector2.zero;

		// DEBUG
		if (Input.GetKeyDown(KeyCode.I))
		{
			health.MyCurrentValue -= 8;
			mana.MyCurrentValue -= 4;
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			health.MyCurrentValue += 10;
			mana.MyCurrentValue += 5;
		}
		// DEBUG

		if (Input.GetKey(KeyCode.W))
		{
			direction += Vector2.up;
		}
		if (Input.GetKey(KeyCode.A))
		{
			direction += Vector2.left;
		}
		if (Input.GetKey(KeyCode.S))
		{
			direction += Vector2.down;
		}
		if (Input.GetKey(KeyCode.D))
		{
			direction += Vector2.right;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			attackRoutine = StartCoroutine(Attack());
		}
	}

	private IEnumerator Attack()
	{
		if (!isAttacking && !isMoving)
		{
			isAttacking = true;
			myAnimator.SetBool("attack", isAttacking);
			yield return new WaitForSeconds(3);
			Debug.Log("Attack done");
			StopAttack();
		}
	}
}
