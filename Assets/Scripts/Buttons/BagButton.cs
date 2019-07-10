using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
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

	public void OnPointerClick(PointerEventData eventData)
	{
		if (bag != null)
		{
			bag.MyBagScript.Toggle();
		}
	}

}
