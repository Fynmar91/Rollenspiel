using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
	[SerializeField]
	private BagButton[] bagButtons;

	//DEBUG
	[SerializeField]
	//DEBUG
	private Item[] items;

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

	private SlotScript sourceSlot;
	public SlotScript MySourceSlot
	{
		get => sourceSlot;

		set
		{
			sourceSlot = value;

			if (value != null)
			{
				sourceSlot.MyIcon.color = Color.grey;
			}
		}
	}

	public int MyEmptySlotCount
	{
		get
		{
			int count = 0;

			foreach (Bag bag in bags)
			{
				count += bag.MyBagScript.MyEmptySlotCount;
			}

			return count;
		}
	}

	public int MyTotalSlotCount
	{
		get
		{
			int count = 0;

			foreach (Bag bag in bags)
			{
				count += bag.MyBagScript.MySlots.Count;
			}

			return count;
		}
	}

	public int MyFullSlotCount
	{
		get
		{
			return MyTotalSlotCount - MyEmptySlotCount;
		}
	}


	private void Awake()
	{
		
	}

	private void Start()
	{
		//DEBUG
		Bag bag = (Bag)Instantiate(items[2]);
		bag.Initialize(8);
		bag.Use();
		//DEBUG
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			//DEBUG
			Bag bag = (Bag)Instantiate(items[0]);
			bag.Initialize(16);
			AddItem(bag);
			//DEBUG
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			//DEBUG
			HealthPotion potion = (HealthPotion)Instantiate(items[1]);
			AddItem(potion);
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
				bag.MyBagButton = bagButton;
				break;
			}
		}
	}

	public void AddBag(Bag bag, BagButton bagButton)
	{
		bags.Add(bag);
		bagButton.MyBag = bag;
	}

	public void RemoveBag(Bag bag)
	{
		bags.Remove(bag);
		Destroy(bag.MyBagScript.gameObject);
	}

	public void SwapBags(Bag oldBag, Bag newBag)
	{
		int newSlotCount = (MyTotalSlotCount - oldBag.MySlots) + newBag.MySlots;

		if (newSlotCount - MyFullSlotCount >= 0)
		{
			List<Item> bagItems = oldBag.MyBagScript.GetItems();

			RemoveBag(oldBag);
			newBag.MyBagButton = oldBag.MyBagButton;
			newBag.Use();

			foreach (Item item in bagItems)
			{
				if (item !=newBag)
				{
					AddItem(item);
				}
			}

			AddItem(oldBag);
			HandScript.MyInstance.Drop();
			MyInstance.MySourceSlot = null;

		}
	}

	public void AddItem(Item item)
	{
		if (item.MyStackSize > 0)
		{
			if (PlaceInStack(item))
			{
				return;
			}
		}

		PlaceInEmpty(item);
	}

	private void PlaceInEmpty(Item item)
	{
		foreach (Bag bag in bags)
		{
			if (bag.MyBagScript.AddItem(item))
			{
				return;
			}
		}
	}

	private bool PlaceInStack(Item item)
	{
		foreach (Bag bag in bags)
		{
			foreach (SlotScript slot in bag.MyBagScript.MySlots)
			{
				if (slot.StackItem(item))
				{
					return true;
				}
			}
		}

		return false;
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

	public Stack<IUseable> GetUseables(IUseable type)
	{
		Stack<IUseable> useables = new Stack<IUseable>();

		foreach (Bag bag in bags)
		{
			foreach (SlotScript slot in bag.MyBagScript.MySlots)
			{
				if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
				{
					foreach (Item item in slot.MyItems)
					{
						useables.Push(item as IUseable);
					}
				}
			}
		}

		return useables;
	}
}
