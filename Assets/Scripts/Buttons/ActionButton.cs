using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable
{
	[SerializeField]
	private Image icon;

	[SerializeField]
	private Text stackSize;

	public IUseable MyUseable { get; set; }
	public Button MyButton { get; private set; }
	public Image MyIcon { get => icon; set => icon = value; }

	private int count;
	public int MyCount { get => count; }


	public Text MyStackText { get => stackSize; }


	private Stack<IUseable> useables = new Stack<IUseable>();

	// Start is called before the first frame update
	void Start()
	{
		MyButton = GetComponent<Button>();
		MyButton.onClick.AddListener(OnClick);
		InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnClick()
	{
		if (HandScript.MyInstance.MyMoveable == null)
		{
			if (MyUseable != null)
			{
				MyUseable.Use();
			}
			if (useables != null && useables.Count > 0)
			{
				useables.Peek().Use();
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
			{
				SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
			}
		}
	}

	public void SetUseable(IUseable useable)
	{
		if (useable is Item)
		{
			useables = InventoryScript.MyInstance.GetUseables(useable);
			count = useables.Count;
			InventoryScript.MyInstance.MySourceSlot.MyIcon.color = Color.white;
			InventoryScript.MyInstance.MySourceSlot = null;
		}
		else
		{
			this.MyUseable = useable;
		}

		UpdateVisual();
	}

	public void UpdateVisual()
	{
		MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
		MyIcon.color = Color.white;

		if (MyCount > 1)
		{
			UIManager.MyInstance.UpdateStackSize(this);
		}
	}

	public void UpdateItemCount(Item item)
	{
		if (item is IUseable && useables.Count > 0)
		{
			if (useables.Peek().GetType() == item.GetType())
			{
				useables = InventoryScript.MyInstance.GetUseables(item as IUseable);
				count = useables.Count;
				UIManager.MyInstance.UpdateStackSize(this);
			}
		}
	}
}
