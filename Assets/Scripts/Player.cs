using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	[SerializeField]
	private Stat mana;

	[SerializeField]
	private Transform[] exitPoints;

	[SerializeField]
	private Block[] blocks;

	private int exitIndex = 2;
	private SpellBook spellBook;
	private float initMana = 100f;

	public Transform MyTarget { get; set; }

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		mana.Initialize(initMana, initMana);
		spellBook = GetComponent<SpellBook>();
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
	}

	public void CastSpell(int spellIndex)
	{
		Block();
		if (MyTarget != null && !isAttacking && !isMoving && InLineOfSight(MyTarget))
		{
			attackRoutine = StartCoroutine(Attack(spellIndex));
		}
	}

	private IEnumerator Attack(int spellIndex)
	{
		Transform currentTarget = MyTarget;
		Spell newSpell = spellBook.CastSpell(spellIndex);

		isAttacking = true;
		myAnimator.SetBool("attack", isAttacking);
		yield return new WaitForSeconds(newSpell.MyCastTime);
		if (currentTarget != null && InLineOfSight(currentTarget))
		{
			SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
			s.Initialize(currentTarget, newSpell.MyDamage, newSpell.MySpeed);
		}
		StopAttack();

	}	

	private bool InLineOfSight(Transform currentTarget)
	{
		Vector3 targetDirection = (currentTarget.position - transform.position).normalized;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, currentTarget.position), 256);
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

	public override void StopAttack()
	{
		base.StopAttack();
		spellBook.StopCasting();
	}
}
