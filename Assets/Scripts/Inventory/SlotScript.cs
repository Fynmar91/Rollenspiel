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

	private ObservableStack<Item> items = new ObservableStack<Item>();

	public bool IsEmpty
	{
		get
		{
			return items.Count == 0;
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
				return items.Peek();
			}

			return null;
		}
	}

	public Image MyIcon { get => icon; set => icon = value; }
	public int MyCount { get => items.Count; }
	public Text MyStackText { get => stackSize; }

	private void Awake()
	{
		items.OnPush += new UpdateStackEvent(UpdateSlot);
		items.OnPop += new UpdateStackEvent(UpdateSlot);
		items.OnClear += new UpdateStackEvent(UpdateSlot);
	}

	public bool AddItem(Item item)
	{
		items.Push(item);
		icon.sprite = item.MyIcon;
		icon.color = Color.white;
		item.MySlot = this;
		return true;
	}

	public void RemoveItem(Item item)
	{
		if (!IsEmpty)
		{
			items.Pop();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (InventoryScript.MyInstance.MySourceSlot == null && !IsEmpty)
			{
				HandScript.MyInstance.TakeMovable(MyItem as IMoveable);
				InventoryScript.MyInstance.MySourceSlot = this;
			}
			else if (InventoryScript.MyInstance.MySourceSlot != null)
			{
				if (PutItemBack() || SwapItems(InventoryScript.MyInstance.MySourceSlot) || AddItems(InventoryScript.MyInstance.MySourceSlot.items))
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
		if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
		{
			items.Push(item);
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
			ObservableStack<Item> tmpSource = new ObservableStack<Item>(source.items);
			source.items.Clear();
			source.AddItems(items);
			items.Clear();
			AddItems(tmpSource);

			return true;
		}
		return false;
	}

	private void UpdateSlot()
	{
		UIManager.MyInstance.UpdateStackSize(this);
	}
}
