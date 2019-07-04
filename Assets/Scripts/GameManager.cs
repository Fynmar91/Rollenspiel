using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private Player player;

	private Camera camera;
	private NPC currentTarget;

	// Start is called before the first frame update
	void Start()
	{
		camera = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		ClickTarget();
	}

	private void ClickTarget()
	{
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

			if (hit.collider != null)
			{
				if (currentTarget != null)
				{
					currentTarget.Deselect();
				}
				currentTarget = hit.collider.GetComponent<NPC>();
				player.MyTarget = currentTarget.Select();
				UIManager.MyInstance.ShowTargetFrame(currentTarget);

			}
			else
			{
				if (currentTarget != null)
				{
					currentTarget.Deselect();
				}
				currentTarget = null;
				player.MyTarget = null;
				UIManager.MyInstance.HideTargetFrame();
			}
			
		}
	}
}
