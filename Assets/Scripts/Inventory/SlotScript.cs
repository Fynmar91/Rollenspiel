using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
	[SerializeField]
	private Image icon;

	[SerializeField]
	private Text stackSize;

	public Image MyIcon { get => icon; set => icon = value; }
	public int MyCount { get => MyItems.Count; }
	public Text MyStackText { get => stackSize; }
	public BagScript MyBag { get; set; }

	public bool IsEmpty
	{
		get
		{
			return MyItems.Count == 0;
		}
	}

	public bool IsFull
	{
		get
		{
			if (IsEmpty || MyCount < MyItem.MyStackSize)
			{
				return false;
			}

			return true;
		}
	}

	public Item MyItem
	{
		get
		{
			if (!IsEmpty)
			{
				return MyItems.Peek();
			}

			return null;
		}
	}

	private ObservableStack<Item> items = new ObservableStack<Item>();
	public ObservableStack<Item> MyItems { get => items; }

	private void Awake()
	{
		MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
		MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
		MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
	}

	public bool AddItem(Item item)
	{
		MyItems.Push(item);
		icon.sprite = item.MyIcon;
		icon.color = Color.white;
		item.MySlot = this;
		return true;
	}

	public void RemoveItem(Item item)
	{
		if (!IsEmpty)
		{
			MyItems.Pop();
		}
	}

	public void Clear()
	{
		if (MyItems.Count > 0)
		{
			MyItems.Clear();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (InventoryScript.MyInstance.MySourceSlot == null && !IsEmpty)
			{
				if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
				{
					if (MyItem is Bag)
					{
						InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
					}
				}
				else
				{
					HandScript.MyInstance.TakeMovable(MyItem as IMoveable);
					InventoryScript.MyInstance.MySourceSlot = this;
				}				
			}
			else if (InventoryScript.MyInstance.MySourceSlot == null && IsEmpty && (HandScript.MyInstance.MyMoveable is Bag))
			{
				Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

				if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.MySlots > 0)
				{
					AddItem(bag);
					bag.MyBagButton.RemoveBag();
					HandScript.MyInstance.Drop();
				}
			}
			else if (InventoryScript.MyInstance.MySourceSlot != null)
			{
				if (PutItemBack() || MergeItems(InventoryScript.MyInstance.MySourceSlot) || SwapItems(InventoryScript.MyInstance.MySourceSlot) || AddItems(InventoryScript.MyInstance.MySourceSlot.MyItems))
				{
					HandScript.MyInstance.Drop();
					InventoryScript.MyInstance.MySourceSlot = null;
				}
			}
		}
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			UseItem();
		}
	}

	public void UseItem()
	{
		if (MyItem is IUseable)
		{
			(MyItem as IUseable).Use();
		}
	}

	public bool StackItem(Item item)
	{
		if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
		{
			MyItems.Push(item);
			item.MySlot = this;
			return true;
		}

		return false;
	}

	private bool AddItems(ObservableStack<Item> newItems)
	{
		if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
		{
			int count = newItems.Count;

			for (int i = 0; i < count; i++)
			{
				if (IsFull)
				{
					return false;
				}

				AddItem(newItems.Pop());
			}

			return true;
		}

		return false;
	}

	private bool PutItemBack()
	{
		if (InventoryScript.MyInstance.MySourceSlot == this)
		{
			InventoryScript.MyInstance.MySourceSlot.MyIcon.color = Color.white;
			return true;
		}

		return false;
	}

	private bool SwapItems(SlotScript source)
	{
		if (IsEmpty)
		{
			return false;
		}
		if (source.MyItem.GetType() != MyItem.GetType() || source.MyCount + MyCount > MyItem.MyStackSize)
		{
			ObservableStack<Item> tmpSource = new ObservableStack<Item>(source.MyItems);

			source.MyItems.Clear();
			source.AddItems(MyItems);
			MyItems.Clear();
			AddItems(tmpSource);

			return true;
		}
		return false;
	}

	private bool MergeItems(SlotScript source)
	{
		if (IsEmpty)
		{
			return false;
		}
		if (source.MyItem.GetType() == MyItem.GetType() && !IsFull)
		{
			int free = source.MyCount < MyItem.MyStackSize - MyCount ? source.MyCount : MyItem.MyStackSize - MyCount;

			for (int i = 0; i < free; i++)
			{
				AddItem(source.MyItems.Pop());
			}

			return true;
		}

		return false;
	}

	private void UpdateSlot()
	{
		UIManager.MyInstance.UpdateStackSize(this);
	}
}
