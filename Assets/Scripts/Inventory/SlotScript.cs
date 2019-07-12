using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
	[SerializeField]
	private Image icon;

	private Stack<Item> items = new Stack<Item>();

	public bool IsEmpty
	{
		get
		{
			return items.Count == 0;
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
			UIManager.MyInstance.UpdateStackSize(this);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
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
}
