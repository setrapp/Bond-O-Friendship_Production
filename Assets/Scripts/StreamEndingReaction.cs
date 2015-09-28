using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamBlockingReaction : StreamReaction {

	public Island completedLevel;

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			if (actionRate >= 0 && reactionProgress >= 1 && completedLevel != null)
			{
				if (Globals.Instance != null && Globals.Instance.levelsCompleted != null)
				{
					Globals.Instance.levelsCompleted
				}
			}
		}
		return reacted;
	}
}
