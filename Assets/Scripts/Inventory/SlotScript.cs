﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
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

	public bool AddItem(Item item)
	{
		items.Push(item);
		icon.sprite = item.MyIcon;
		icon.color = Color.white;

		return true;
	}
}
