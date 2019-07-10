using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private ActionButton[] actionButtons;

	[SerializeField]
	private GameObject targetFrame;

	[SerializeField]
	private Image portraiFrame;

	[SerializeField]
	private CanvasGroup keybindMenu;

	[SerializeField]
	private CanvasGroup spellBook;

	private Stat healthStat;
	private GameObject[] keybindButtons;
	private bool menuActive = false;

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

	private void Awake()
	{
		keybindButtons = GameObject.FindGameObjectsWithTag("Keybinds");
	}

	// Start is called before the first frame update
	void Start()
	{
		//DEBUG
		HandScript.MyInstance.MyMoveable = SpellBook.MyInstance.GetSpell("FROSTBOLT");
		actionButtons[0].SetUseable(SpellBook.MyInstance.GetSpell("FROSTBOLT"));
		//DEBUG

		healthStat = targetFrame.GetComponentInChildren<Stat>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleMenu(keybindMenu);
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			ToggleMenu(spellBook);
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			InventoryScript.MyInstance.Toggle();
		}
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

	public void ToggleMenu(CanvasGroup canvasGroup)
	{
		if (menuActive == true)
		{
			keybindMenu.alpha = 0;
			keybindMenu.blocksRaycasts = false;

			spellBook.alpha = 0;
			spellBook.blocksRaycasts = false;

			menuActive = false;
		}
		else
		{
			canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
			canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;

			menuActive = true;
		}		
		//Time.timeScale = Time.timeScale > 0 ? 0 : 1;
	}

	public void UpdateKeyText(string key, KeyCode code)
	{
		Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();

		tmp.text = code.ToString().Replace("Alpha", "");
	}

	public void ClickActionButton(string buttonName)
	{
		Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
	}
}
