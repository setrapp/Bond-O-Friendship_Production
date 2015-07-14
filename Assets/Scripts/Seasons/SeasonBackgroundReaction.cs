using UnityEngine;
using System.Collections;

public class SeasonBackgroundReaction : SeasonReaction
{
	public Color[] seasonColors = new Color[3];
	public Renderer seasonRenderer;

	void Start()
	{
		base.Start();

		if (seasonRenderer == null)
		{
			seasonRenderer = GetComponent<Renderer>();
		}

		if (seasonRenderer != null)
		{
			seasonColors[0] = seasonRenderer.material.color;
		}
	}

	override protected void ApplySeasonChanges()
	{
		base.ApplySeasonChanges();

		StartCoroutine(TransitionSeason());
	}

	private IEnumerator TransitionSeason()
	{
		float timeElapsed = 0;
		

		if (manager != null && seasonRenderer != null)
		{
			Color startColor = seasonRenderer.material.color;
			Color endColor = seasonColors[(int)season];
			endColor.a = startColor.a;

			if (manager.transitionTime <= 0)
			{
				seasonRenderer.material.color = endColor;
			}
			else
			{
				while(timeElapsed < manager.transitionTime)
				{
					timeElapsed += Time.deltaTime;
					float progress = timeElapsed / manager.transitionTime;
					seasonRenderer.material.color = ((1 - progress) * startColor) + (progress * endColor);

					yield return null;
				}
			}
		}
		
	}
}
