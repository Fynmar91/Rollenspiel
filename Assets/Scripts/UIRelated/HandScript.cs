using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
	[SerializeField]
	private Vector3 offset;

	private Image icon;
	private static HandScript instance;

	public IMoveable MyMoveable { get; set; }

	public static HandScript MyInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<HandScript>();
			}
			return instance;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		icon = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		icon.transform.position = Input.mousePosition + offset;
	}

	public void TakeMovable(IMoveable moveable)
	{
		this.MyMoveable = moveable;
		icon.sprite = moveable.MyIcon;
		icon.color = Color.white;
	}
}
