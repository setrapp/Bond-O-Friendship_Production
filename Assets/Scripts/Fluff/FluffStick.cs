using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffStick : MonoBehaviour {
	public FluffStickRoot root;
	public Fluff stuckFluff;
	public Vector3 stickOffset = Vector3.zero;
	public Vector3 stickDirection = Vector3.forward;
	public bool moveToCollision;

	public void AddPullForce(Vector3 pullForce, Vector3 position)
	{
		if (root != null && !root.fluffsDetachable)
		{
			root.AddPullForce(pullForce, position);
		}
	}

	public bool CanStick()
	{
		return (stuckFluff == null || (root != null && !root.trackStuckFluffs));
	}

	public void FluffDetached(Fluff fluff)
	{
		if (fluff != null && fluff == stuckFluff)
		{
			stuckFluff = null;
			if (root != null)
			{
				root.SendMessage("FluffStickEmpty", this, SendMessageOptions.DontRequireReceiver);
			}
		}
		
	}
}
