using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
	private Color defaultColor;
	private Color fadedColor;
	private Color currentColor;
	private float currentAlpha;
	private Coroutine fadeRoutine;

	public SpriteRenderer MySpriteRenderer { get; set; }

	public int CompareTo(Obstacle other)
	{
		if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
		{
			return 1;
		}
		else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
		{
			return -1;
		}
		else
		{
			return 0;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		MySpriteRenderer = GetComponent<SpriteRenderer>();
		defaultColor = MySpriteRenderer.color;
		currentColor = MySpriteRenderer.color;
		fadedColor = defaultColor;
		fadedColor.a = 0.7f;
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void FadeOut()
	{
		if (MySpriteRenderer != null)
		{
			currentColor = fadedColor;

			if (fadeRoutine != null)
			{
				StopCoroutine(fadeRoutine);
				fadeRoutine = StartCoroutine(Fade());
			}
			else
			{
				fadeRoutine = StartCoroutine(Fade());
			}
		}
		
	}

	public void FadeIn()
	{
		currentColor = defaultColor;

		if (fadeRoutine != null)
		{
			StopCoroutine(fadeRoutine);
			fadeRoutine = StartCoroutine(Fade());
		}
		else
		{
			fadeRoutine = StartCoroutine(Fade());
		}
	}

	private IEnumerator Fade()
	{
		float progress = 0.0f;
		float rate = 1.0f / 4;

		while (currentColor.a != MySpriteRenderer.color.a)
		{
			progress += rate * Time.deltaTime;
			currentAlpha = Mathf.Lerp(MySpriteRenderer.color.a, currentColor.a, progress);
			MySpriteRenderer.color = new Vector4(defaultColor.r, defaultColor.g, defaultColor.b, currentAlpha);

			yield return null;
		}
		

	}
}
