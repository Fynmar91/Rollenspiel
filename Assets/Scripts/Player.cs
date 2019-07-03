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

	[SerializeField]
	private GameObject[] spellPrefab;

	[SerializeField]
	private Transform[] exitPoints;

	private int exitIndex = 2;

	[SerializeField]
	private Block[] blocks;	

	public Transform MyTarget { get; set; }

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
			exitIndex = 0;
		}
		if (Input.GetKey(KeyCode.A))
		{
			direction += Vector2.left;
			exitIndex = 3;
		}
		if (Input.GetKey(KeyCode.S))
		{
			direction += Vector2.down;
			exitIndex = 2;
		}
		if (Input.GetKey(KeyCode.D))
		{
			direction += Vector2.right;
			exitIndex = 1;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			
		}
	}

	private IEnumerator Attack(int spellIndex)
	{
		if (!isAttacking && !isMoving)
		{
			isAttacking = true;
			myAnimator.SetBool("attack", isAttacking);
			yield return new WaitForSeconds(1);
			Instantiate(spellPrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity);
			StopAttack();
		}
	}

	public void CastSpell(int spellIndex)
	{
		Block();
		if (MyTarget != null && !isAttacking && !isMoving && InLineOfSight())
		{
			attackRoutine = StartCoroutine(Attack(spellIndex));
		}
	}

	private bool InLineOfSight()
	{		
		Vector3 targetDirection = (MyTarget.position - transform.position).normalized;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.position), 256);
		if (hit.collider == null)
		{
			return true;
		}
		return false;
	}

	private void Block()
	{
		foreach (Block b in blocks)
		{
			b.Deactivate();
		}
		blocks[exitIndex].Activate();
	}
}
