using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamFillReaction : StreamReaction {

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

	public override void React(float actionRate)
	{
		if (reactionProgress < 1)
		{
			base.React(actionRate);
			ApplyTint();
		}
	}

	private void ApplyTint()
	{
		Color tint = (unfilledTint * (1 - reactionProgress)) + (filledTint * reactionProgress);
		for (int i = 0; i < fillTargetRenderers.Count && i < baseColors.Count; i++)
		{
			fillTargetRenderers[i].material.color = baseColors[i] * tint;
		}
	}
}
