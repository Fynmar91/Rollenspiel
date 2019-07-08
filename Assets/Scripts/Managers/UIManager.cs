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

	private Stat healthStat;
	private GameObject[] keybindButtons;

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
		healthStat = targetFrame.GetComponentInChildren<Stat>();
		SetUseable(actionButtons[0], SpellBook.MyInstance.GetSpell("FIREBALL"));
		SetUseable(actionButtons[1], SpellBook.MyInstance.GetSpell("FROSTBOLT"));
		SetUseable(actionButtons[2], SpellBook.MyInstance.GetSpell("LIGHTNING ARC"));
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleMenu();
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

	public void ToggleMenu()
	{
		keybindMenu.alpha = keybindMenu.alpha > 0 ? 0 : 1;
		keybindMenu.blocksRaycasts = keybindMenu.blocksRaycasts == true ? false : true;
		Time.timeScale = Time.timeScale > 0 ? 0 : 1;
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

	public void SetUseable(ActionButton btn, IUseable useable)
	{
		btn.MyIcon.sprite = useable.MyIcon;
		btn.MyIcon.color = Color.white;
		btn.MyUseable = useable;
	}
}
