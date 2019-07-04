using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Button[] actionButtons;

	[SerializeField]
	private GameObject targetFrame;

	[SerializeField]
	private Image portraiFrame;

	private KeyCode action1, action2, action3;
	private Stat healthStat;

	private static UIManager instance;

	public static UIManager MyInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<UIManager>();
			}
			return instance;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		action1 = KeyCode.Alpha1;
		action2 = KeyCode.Alpha2;
		action3 = KeyCode.Alpha3;

		healthStat = targetFrame.GetComponentInChildren<Stat>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(action1))
		{
			ActionButtonOnClick(0);
		}
		if (Input.GetKeyDown(action2))
		{
			ActionButtonOnClick(1);
		}
		if (Input.GetKeyDown(action3))
		{
			ActionButtonOnClick(2);
		}
	}

	private void ActionButtonOnClick(int buttonIndex)
	{
		actionButtons[buttonIndex].onClick.Invoke();
	}

	public void ShowTargetFrame(NPC target)
	{
		targetFrame.SetActive(true);
		healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);
		portraiFrame.sprite = target.MyPortrait;
		target.healthchanged += new HealthChanged(UpdateTargetFrame);
		target.characterRemoved += new CharacterRemoved(HideTargetFrame);
	}

	public void HideTargetFrame()
	{
		targetFrame.SetActive(false);
	}

	public void UpdateTargetFrame(float health)
	{
		healthStat.MyCurrentValue = health;
	}
}
