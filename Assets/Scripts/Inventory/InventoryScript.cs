using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
	[SerializeField]
	private BagButton[] bagButtons;

	//DEBUG
	[SerializeField]
	private Item[] items;
	//DEBUG

	private List<Bag> bags = new List<Bag>();

	public bool CanAddBag
	{
		get
		{
			return bags.Count < 5;
		}
	}

	private static InventoryScript instance;

	public static InventoryScript MyInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<InventoryScript>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			//DEBUG
			Bag bag = (Bag)Instantiate(items[0]);
			bag.Initialize(16);
			bag.Use();
			//DEBUG
		}
	}

	public void AddBag(Bag bag)
	{
		foreach (BagButton bagButton in bagButtons)
		{
			if (bagButton.MyBag == null)
			{
				bagButton.MyBag = bag;
				bags.Add(bag);
				break;
			}
		}
	}

	public void Toggle()
	{
		bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);

		foreach (Bag bag in bags)
		{
			if (bag.MyBagScript.IsOpen != closedBag)
			{
				bag.MyBagScript.Toggle();
			}
		}
	}
}
