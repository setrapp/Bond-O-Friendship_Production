using UnityEngine;
using System.Collections;

public class StreamColliderResetReaction : StreamReaction {

	public Collider resetteeCollider;
	private bool colliderEnabled;
	private bool resetting = false;
	private bool waitToReset = false;

	void Update()
	{
		if (resetting)
		{
			resetteeCollider.enabled = colliderEnabled;
			resetting = false;
		}
		if (waitToReset)
		{
			resetting = true;
			waitToReset = false;
		}
	}

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			if (actionRate >= 0 && reactionProgress >= 1)
			{
				colliderEnabled = resetteeCollider.enabled;
				resetteeCollider.enabled = !colliderEnabled;
				waitToReset = true;
				reactable = false;
			}
		}
		return reacted;
	}
}
