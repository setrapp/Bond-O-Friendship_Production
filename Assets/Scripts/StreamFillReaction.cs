using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamFillReaction : StreamReaction {

	public float fillProgress = 0;
	public float fillRate = 1;

	[SerializeField]
	public List<Renderer> fillTargetRenderers;
	public Color unfilledTint = Color.grey;
	public Color filledTint = Color.white;
	private List<Color> baseColors;

	void Start()
	{
		baseColors = new List<Color>();
		for (int i = 0; i < fillTargetRenderers.Count; i++)
		{
			baseColors.Add(fillTargetRenderers[i].material.color);
		}
			
		ApplyTint();
	}

	public override void React(Stream stream)
	{
		if (fillProgress < 1)
		{
			fillProgress = Mathf.Clamp01(fillProgress + fillRate * Time.deltaTime);

			if (fillProgress >= 1)
			{
				if (reactionCollider != null)
				{
					reactionCollider.enabled = false;
				}
				if (reactionBody != null)
				{
					reactionBody.isKinematic = false;
				}
			}

			ApplyTint();
		}
	}

	private void ApplyTint()
	{
		Color tint = (unfilledTint * (1 - fillProgress)) + (filledTint * fillProgress);
		for (int i = 0; i < fillTargetRenderers.Count && i < baseColors.Count; i++)
		{
			fillTargetRenderers[i].material.color = baseColors[i] * tint;
		}
	}
}
