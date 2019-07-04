using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
	private Rigidbody2D myRigidbody;
	private float speed;
	private int damage;

	public Transform MyTarget { get; private set; }


	// Start is called before the first frame update
	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	public void Initialize(Transform target, int damage, float speed)
	{
		this.MyTarget = target;
		this.damage = damage;
		this.speed = speed;
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
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "HitBox" && collision.transform == MyTarget)
		{
			speed = 0;
			collision.GetComponentInParent<Enemy>().TakeDamage(damage);
			GetComponent<Animator>().SetTrigger("impact");
			myRigidbody.velocity = Vector2.zero;
			MyTarget = null;
		}
	}
}
