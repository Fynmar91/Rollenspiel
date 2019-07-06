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

	private SpellBook spellBook;
	private float initMana = 100f;

	private Vector3 min, max;

	public int MyExitIndex { get; set; }

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		MyExitIndex = 2;
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
			//MyExitIndex = 0;
		}
		if (Input.GetKey(KeyCode.A))
		{
			MyDirection += Vector2.left;
			//MyExitIndex = 3;
		}
		if (Input.GetKey(KeyCode.S))
		{
			MyDirection += Vector2.down;
			//MyExitIndex = 2;
		}
		if (Input.GetKey(KeyCode.D))
		{
			MyDirection += Vector2.right;
			//MyExitIndex = 1;
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
		if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !isMoving && InLineOfSight(MyTarget))
		{
			IsAttacking = true;
			TurnPlayer();
			MyAnimator.SetBool("attack", IsAttacking);
			attackRoutine = StartCoroutine(Attack(spellIndex));
			HandleLayers();
		}
	}

	private void TurnPlayer()
	{
		MyDirection = (MyTarget.transform.position - transform.position).normalized;
		MyAnimator.SetFloat("x", Mathf.RoundToInt(MyDirection.x));
		MyAnimator.SetFloat("y", Mathf.RoundToInt(MyDirection.y));
		MyDirection = Vector2.zero;
	}

	private IEnumerator Attack(int spellIndex)
	{
		Transform currentTarget = MyTarget;
		Spell newSpell = spellBook.CastSpell(spellIndex);
		float animatorSpeed = MyAnimator.speed;

		MyAnimator.speed = 2 / newSpell.MyCastTime;
		
		yield return new WaitForSeconds(newSpell.MyCastTime);

		if (currentTarget != null && InLineOfSight(currentTarget))
		{
			SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[MyExitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
			s.Initialize(currentTarget, newSpell.MyDamage, newSpell.MySpeed, transform);
		}
		MyAnimator.speed = animatorSpeed;
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
		blocks[MyExitIndex].Activate();
	}

	public void StopAttack()
	{
		spellBook.StopCasting();
		IsAttacking = false;
		MyAnimator.SetBool("attack", IsAttacking);

		if (attackRoutine != null)
		{
			StopCoroutine(attackRoutine);
		}
	}
}
