using UnityEngine;
using System.Collections;

public class KinematicAndFixed : MonoBehaviour {

	public Rigidbody body1;
	public Rigidbody body2;
	public bool willKinematic = false;
	public WaitPad triggerPad;
	private bool triggered = false;
	public SpinPad.SpinLimit triggerSpinLimit = SpinPad.SpinLimit.PULL_END;

	public void Update()
	{
		if (!triggered)
		{
			bool padTriggered = triggerPad.activated;
			if (triggerPad as SpinPad != null)
			{
				padTriggered = ((SpinPad)triggerPad).IsAtLimit(triggerSpinLimit);
			}

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
