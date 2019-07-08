using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
	//DEBUG
	[SerializeField]
	private Item[] items;
	//DEBUG

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
		//DEBUG
		Bag bag = (Bag)Instantiate(items[0]);
		bag.Initialize(16);
		bag.Use();
		//DEBUG
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

// Update is called once per frame
void Update()
	{

	}
}
