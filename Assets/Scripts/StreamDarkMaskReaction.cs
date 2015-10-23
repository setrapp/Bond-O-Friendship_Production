using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamDarkMaskReaction : StreamReaction {

	public SetShaderData_DarkAlphaMasker targetMask = null;
	public bool fadeIn = false;

	protected override void Start()
	{
		base.Start();
		if (targetMask == null && Globals.Instance != null)
		{
			targetMask = Globals.Instance.darknessMask;
		}
	}

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			if (reactionProgress >= 1 && actionRate > 0)
			{
				if (targetMask.trigger != null)
				{
					targetMask.trigger.enabled = fadeIn;
				}
				else
				{
					targetMask.fadeIn = fadeIn;
				}
			}
		}
		return reacted;
	}
}
