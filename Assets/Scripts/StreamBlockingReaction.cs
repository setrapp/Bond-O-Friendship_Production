using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamBlockingReaction : StreamReaction {

	public bool collidersEnabled = false;
	[SerializeField]
	public List<Collider> reactionColliders;
	public bool bodiesKinematic = false;
	[SerializeField]
	public List<Rigidbody> reactionBodies;

	public override void React(float actionRate)
	{
		if (reactionProgress < 1)
		{
			base.React(actionRate);

			if (reactionProgress >= 1)
			{
				for (int i = 0; i < reactionColliders.Count; i++)
				{
					if (reactionColliders[i] != null)
					{
						reactionColliders[i].enabled = collidersEnabled;
					}
				}

				for (int i = 0; i < reactionBodies.Count; i++)
				{
					if (reactionBodies[i] != null)
					{
						reactionBodies[i].isKinematic = bodiesKinematic;
					}
				}
			}
		}
	}
}
