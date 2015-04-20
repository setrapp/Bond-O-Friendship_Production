using UnityEngine;
using System.Collections;

public class KinematicAndFixed : MonoBehaviour {

	public Rigidbody body1;
	public Rigidbody body2;
	public bool willKinematic = false;
	public WaitPad triggerPad;
	private bool triggered = false;

	public void Update()
	{
		if (!triggered)
		{
			bool padTriggered = triggerPad.activated;

			if (padTriggered)
			{
				triggered = true;
				body1.isKinematic = body2.isKinematic = willKinematic;
				FixedJoint bodyJoint = body2.gameObject.AddComponent<FixedJoint>();
				bodyJoint.connectedBody = body1;
			}
		}
		
	}
}
