using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamAnimateReaction : StreamReaction {

	public bool toAnimate = true;
	[SerializeField]
	public List<Animator> reactionAnimators;
	public bool startOpposite = true;

	protected override void Start()
	{
		base.Start();
		if (startOpposite)
		{
			for (int i = 0; i < reactionAnimators.Count; i++)
			{
				if (reactionAnimators[i] != null)
				{
					reactionAnimators[i].enabled = !toAnimate;
				}
			}
		}
	}

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			if (actionRate >= 0 && reactionProgress >= 1)
			{
				for (int i = 0; i < reactionAnimators.Count; i++)
				{
					if (reactionAnimators[i] != null)
					{
						reactionAnimators[i].enabled = toAnimate;
					}
				}
			}
			else if (actionRate < 0 && reactionProgress < 1)
			{
				for (int i = 0; i < reactionAnimators.Count; i++)
				{
					if (reactionAnimators[i] != null)
					{
						reactionAnimators[i].enabled = !toAnimate;
					}
				}
			}
		}
		return reacted;
	}
}
