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

	private Vector3 min, max;

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
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
		base.Update();
	}

	private void GetInput()
	{
		MyDirection = Vector2.zero;

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
			MyDirection += Vector2.up;
			exitIndex = 0;
		}
		if (Input.GetKey(KeyCode.A))
		{
			MyDirection += Vector2.left;
			exitIndex = 3;
		}
		if (Input.GetKey(KeyCode.S))
		{
			MyDirection += Vector2.down;
			exitIndex = 2;
		}
		if (Input.GetKey(KeyCode.D))
		{
			MyDirection += Vector2.right;
			exitIndex = 1;
		}
		if (isMoving)
		{
			StopAttack();
		}
	}

	public void SetLimits(Vector3 min, Vector3 max)
	{
		this.min = min;
		this.max = max;
	}

	public void CastSpell(int spellIndex)
	{
		if (MyTarget != null && !isAttacking && !isMoving && InLineOfSight(MyTarget))
		{
			TurnPlayer();
			attackRoutine = StartCoroutine(Attack(spellIndex));
			HandleLayers();
		}
	}

	private void TurnPlayer()
	{
		if (Mathf.Abs((MyTarget.transform.position - transform.position).normalized.y) > Mathf.Abs((MyTarget.transform.position - transform.position).normalized.x))
		{
			if (Vector2.Dot(transform.TransformDirection(Vector2.up), MyTarget.position) > 0)
			{
				exitIndex = 0;
			}
			else if (Vector2.Dot(transform.TransformDirection(Vector2.up), MyTarget.position) < 0)
			{
				exitIndex = 2;
			}
		}
		else
		{
			if (Vector2.Dot(transform.TransformDirection(Vector2.right), MyTarget.position) > 0)
			{
				exitIndex = 1;
			}
			else if (Vector2.Dot(transform.TransformDirection(Vector2.right), MyTarget.position) < 0)
			{
				exitIndex = 3;
			}
		}
		MyDirection = (MyTarget.transform.position - transform.position).normalized;
		myAnimator.SetFloat("x", MyDirection.x);
		myAnimator.SetFloat("y", MyDirection.y);
		MyDirection = Vector2.zero;
	}

	private IEnumerator Attack(int spellIndex)
	{
		Transform currentTarget = MyTarget;
		Spell newSpell = spellBook.CastSpell(spellIndex);
		float animatorSpeed = myAnimator.speed;

		isAttacking = true;
		myAnimator.speed = 2 / newSpell.MyCastTime;

		myAnimator.SetBool("attack", isAttacking);
		yield return new WaitForSeconds(newSpell.MyCastTime);
		if (currentTarget != null && InLineOfSight(currentTarget))
		{
			SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
			s.Initialize(currentTarget, newSpell.MyDamage, newSpell.MySpeed);
		}
		myAnimator.speed = animatorSpeed;
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

	public void StopAttack()
	{
		spellBook.StopCasting();
		isAttacking = false;
		myAnimator.SetBool("attack", isAttacking);

		if (attackRoutine != null)
		{
			StopCoroutine(attackRoutine);
		}
	}
}
