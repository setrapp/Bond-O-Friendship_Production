using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffStickRoot : MonoBehaviour {

	public bool allChildSticks = true;
	[SerializeField]
	public List<FluffStick> attachedSticks;
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

		if (allChildSticks)
		{
			FluffStick[] childSticks = GetComponentsInChildren<FluffStick>();
			for (int i = 0; i < childSticks.Length; i++)
			{
				if (!attachedSticks.Contains(childSticks[i]))
				{
					attachedSticks.Add(childSticks[i]);
				}
			}
		}

		for (int i = 0; i < attachedSticks.Count; i++)
		{
			/*TODO set this as the root of sticks and make fluffs ignore a list of colliders*/
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
