using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
	[SerializeField]
	private GameObject slotPrefab;

	private CanvasGroup canvasGroup;

	private List<SlotScript> slots = new List<SlotScript>();
	public List<SlotScript> MySlots { get => slots; }

	public bool IsOpen
	{
		get
		{
			return canvasGroup.alpha > 0;
		}
	}

	public int MyEmptySlotCount
	{
		get
		{
			int count = 0;
			foreach (SlotScript slot in MySlots)
			{
				if (slot.IsEmpty)
				{
					count++;
				}
			}
			return count;
		}
	}

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void AddSlots(int slotCount)
	{
		for (int i = 0; i < slotCount; i++)
		{
			SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
			slot.MyBag = this;
			MySlots.Add(slot);
		}
	}

	public bool AddItem(Item item)
	{
		foreach (SlotScript slot in MySlots)
		{
			if (slot.IsEmpty)
			{
				slot.AddItem(item);

				return true;
			}
		}

		return false;
	}

	public void Toggle()
	{
		canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
		canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
	}
}
