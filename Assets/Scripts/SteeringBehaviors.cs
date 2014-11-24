using UnityEngine;
using System.Collections;

public class SteeringBehaviors : MonoBehaviour {
	public SimpleMover mover;
	public Vector3 desiredVelocity;
	public Vector3 steeringForce;
	/*TODO actual movement should not be done here.*/

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
	}


	public void Seek(Vector3 seekTarget, bool arrive = false)
	{
		desiredVelocity = seekTarget - transform.position;
		if (!arrive)
		{
			desiredVelocity = desiredVelocity.normalized * mover.maxSpeed;
		}

		steeringForce = desiredVelocity - mover.velocity;

		mover.Accelerate(steeringForce, !arrive, false);
	}

	public void Flee(Vector3 fleeTarget)
	{
		if (transform.position == fleeTarget)
		{
			Seek(fleeTarget + -Vector3.right);
		}
		else
		{
			Seek(transform.position + ((transform.position - fleeTarget) * 2));
		}
	}

	public void Pursue(GameObject pursuee, bool arrive = false)
	{
		Pursue(pursuee, Vector3.zero, false, arrive);
	}

	public void Pursue(GameObject pursuee, float distance, bool acceptWithinProximity = true, bool arrive = false)
	{
		// Only seek if still far away || getting close is not good enough.
		Vector3 fromPursuee = transform.position - pursuee.transform.position;
		if (!acceptWithinProximity || fromPursuee.sqrMagnitude > Mathf.Pow(distance, 2))
		{
			Seek(pursuee.transform.position + (fromPursuee.normalized * distance), arrive);
		}
	}

	public void Pursue(GameObject pursuee, Vector3 offset, bool acceptWithinProximity = true, bool arrive = false)
	{
		Vector3 worldOffset = pursuee.transform.TransformDirection(offset);
		bool needSeek = true;

		// Only seek if still far away || getting close is not good enough.
		if (acceptWithinProximity)
		{
			Vector3 toPursuee = pursuee.transform.position - transform.position;
			if (toPursuee.sqrMagnitude <= worldOffset.sqrMagnitude && Vector3.Dot(-toPursuee, worldOffset) > 0)
			{
				needSeek = false;
			}
		}
		
		if (needSeek)
		{
			Seek(pursuee.transform.position + worldOffset, arrive);
		}
	}
}
