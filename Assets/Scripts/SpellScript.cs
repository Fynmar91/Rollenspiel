using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
	private Rigidbody2D myRigidbody;
	private Transform source;
	private float speed;
	private int damage;
	private float maxLifetime = 1.0f;
	private float aliveTime = 0;

	public Transform MyTarget { get; private set; }


	// Start is called before the first frame update
	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	public void Initialize(Transform target, int damage, float speed, Transform source)
	{
		this.MyTarget = target;
		this.damage = damage;
		this.speed = speed;
		this.source = source;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FixedUpdate()
	{
		if (MyTarget != null)
		{
			Vector2 direction = MyTarget.position - transform.position;

			myRigidbody.velocity = direction.normalized * speed;

			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		else
		{
			aliveTime += Time.deltaTime;
			if (aliveTime > maxLifetime)
			{
				GetComponent<Animator>().SetTrigger("impact");
				myRigidbody.velocity = Vector2.zero;
			}
		}
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "HitBox" && collision.transform == MyTarget)
		{
			Character c = collision.GetComponentInParent<Character>();
			speed = 0;
			c.TakeDamage(damage, source);
			GetComponent<Animator>().SetTrigger("impact");
			myRigidbody.velocity = Vector2.zero;
			MyTarget = null;
		}
	}
}
