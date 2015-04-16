using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamBlockingReaction : StreamReaction {

	public bool collidersToEnabled = false;
	[SerializeField]
	public List<Collider> reactionColliders;
	public bool bodiesToKinematic = false;
	[SerializeField]
	public List<Rigidbody> reactionBodies;

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			if (reactionProgress >= 1)
			{
				for (int i = 0; i < reactionColliders.Count; i++)
				{
					if (reactionColliders[i] != null)
					{
						reactionColliders[i].enabled = collidersToEnabled;
					}
				}

				for (int i = 0; i < reactionBodies.Count; i++)
				{
					if (reactionBodies[i] != null)
					{
						reactionBodies[i].isKinematic = bodiesToKinematic;
					}
				}
			}
		}
		return reacted;
	}
}
