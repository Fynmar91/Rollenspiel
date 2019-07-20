using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private Sprite full;

	private Bag bag;

	public Bag MyBag
	{
		get { return bag; }
		set
		{
			if (value != null)
			{
				GetComponent<Image>().sprite = value.MyIcon;
				GetComponent<Image>().color = new Color(255, 255, 255, 255);
			}
			else
			{
				GetComponent<Image>().color = new Color(0,0,0,0);
			}
			bag = value;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (InventoryScript.MyInstance.MySourceSlot != null && HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
			{
				if (MyBag != null)
				{
					InventoryScript.MyInstance.SwapBags(MyBag, HandScript.MyInstance.MyMoveable as Bag);
				}
				else
				{
					Bag tmp = (Bag)HandScript.MyInstance.MyMoveable;
					tmp.MyBagButton = this;
					tmp.Use();
					MyBag = tmp;
					HandScript.MyInstance.Drop();
					InventoryScript.MyInstance.MySourceSlot = null;
				}
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				HandScript.MyInstance.TakeMovable(MyBag);
			}
			else if (bag != null)
			{
				bag.MyBagScript.Toggle();
			}
		}
	}

	public void RemoveBag()
	{
		InventoryScript.MyInstance.RemoveBag(MyBag);
		MyBag.MyBagButton = null;

		foreach (Item item in MyBag.MyBagScript.GetItems())
		{
			InventoryScript.MyInstance.AddItem(item);
		}

		MyBag = null;
	}
}
