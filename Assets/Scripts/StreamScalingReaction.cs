using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamScalingReaction : StreamReaction {

	[SerializeField]
	public List<StreamScalingStats> scalees;

	void Start()
	{
		for (int i = 0; i < scalees.Count; i++)
		{
			scalees[i].scalee.localScale = scalees[i].unscaled;
		}
	}

	public override void React(float actionRate)
	{
		if (reactionProgress < 1)
		{
			base.React(actionRate);

			for (int i = 0; i < scalees.Count; i++)
			{
				scalees[i].scalee.localScale = (scalees[i].unscaled * (1 - reactionProgress)) + (scalees[i].scaled * reactionProgress);
			}
		}
	}
}

[System.Serializable]
public class StreamScalingStats
{
	public Transform scalee;
	public Vector3 unscaled;
	public Vector3 scaled;
}
