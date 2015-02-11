using UnityEngine;
using System.Collections;

public class FluffStick : MonoBehaviour {
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
