using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);
public delegate void CharacterRemoved();

public class NPC : Character
{
	[SerializeField]
	private Sprite portrait;

	public event HealthChanged healthchanged;
	public event CharacterRemoved characterRemoved;

	public Sprite MyPortrait { get => portrait; }
	

	public virtual Transform Select()
	{
		return hitBox;
	}

	public virtual void Deselect()
	{
		healthchanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
		characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
	}

	public void OnHealthChanged(float health)
	{
		if (healthchanged != null)
		{
			healthchanged(health);
		}
	}

	public void OnCharacterRemoved()
	{
		if (characterRemoved != null)
		{
			characterRemoved();
		}
		Destroy(gameObject);
	}
}
