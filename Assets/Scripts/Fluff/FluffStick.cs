using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffStick : MonoBehaviour {
	public Rigidbody pullableBody;
	public bool noKinematicOnPull;
	public bool allowSway = true;
	public float bodyMassFactor = 1;
	public float pullMass = -1;
	public float maxPullForce = 0;
	private float currentPullForce = 0;

	/*TODO 
	 * split this up in to two classes, one that can be put on children and one put on parent to track children
	 * only objects with fluff sticks on them should let fluffs stick to them
	 * in the parent class, add a list of colliders for stuck fluffs to ignore
	 * add pull force should start at children but be delegated to parent for proper clamping
	 * fluff stick children should mask a niche in the stickable object to show that fluffs go there
	 * pullable body should not be required
	 */

	void Start()
	{
		if (pullableBody == null)
		{
			pullableBody = GetComponent<Rigidbody>();
		}
		if (pullMass < 0 && pullableBody != null)
		{
			pullMass = pullableBody.mass * bodyMassFactor;
		}
	}

	void Update()
	{
		currentPullForce = 0;
	}

	public void AddPullForce(Vector3 pullForce, Vector3 position)
	{
		if (pullableBody.isKinematic && noKinematicOnPull)
		{
			pullableBody.velocity = Vector3.zero;
			pullableBody.isKinematic = false;
		}

		if ((currentPullForce < maxPullForce || maxPullForce < 0) && pullableBody != null)
		{
			float pullForceMag = pullForce.magnitude;
			if (currentPullForce + pullForceMag > maxPullForce && maxPullForce >= 0)
			{
				pullForce = (pullForce / pullForceMag) * (maxPullForce - currentPullForce);
				pullForceMag += (maxPullForce - currentPullForce);
			}

			currentPullForce += pullForceMag;
			pullableBody.AddForceAtPosition(pullForce / pullMass, position, ForceMode.VelocityChange);
		}
		
	}
}
