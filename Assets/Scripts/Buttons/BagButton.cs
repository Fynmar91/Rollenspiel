using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagButton : MonoBehaviour
{
	[SerializeField]
	private Sprite full, empty;

	private Bag bag;

	public Bag MyBag
	{
		get { return bag; }
		set
		{
			if (value != null)
			{
				GetComponent<Image>().sprite = full;
			}
			else
			{
				GetComponent<Image>().sprite = empty;
			}
			bag = value;
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
