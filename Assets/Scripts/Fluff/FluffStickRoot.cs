using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffStickRoot : MonoBehaviour {

	public bool allChildSticks = true;
	[SerializeField]
	public List<FluffStick> attachedSticks;
	public bool trackStuckFluffs = true;
	public bool fluffsDetachable = true;
	public Rigidbody pullableBody;
	public bool noKinematicOnPull;
	public bool allowSway = true;
	public float bodyMassFactor = 1;
	public float pullMass = -1;
	public float maxPullForce = 0;
	private float currentPullForce = 0;

	void Start()
	{
		if (pullableBody == null)
		{
			pullableBody = GetComponent<Rigidbody>();
		}

		if (pullableBody == null)
		{
			fluffsDetachable = true;
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
			attachedSticks[i].root = this;
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
