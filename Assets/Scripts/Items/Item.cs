using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private int stackSize;

	private SlotScript slot;

	public Sprite MyIcon { get => icon; }
	public int MyStackSize { get => stackSize; }
	protected SlotScript MySlot { get => slot; set => slot = value; }
}
