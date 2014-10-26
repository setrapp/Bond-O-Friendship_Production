using UnityEngine;
using System.Collections;

public class SteeringBehaviors : MonoBehaviour {
	public SimpleMover mover;
	/*TODO actual movement should not be done here.*/

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
	}


	public void Seek(Vector3 seekTarget, bool forceFullAcceleration = true)
	{
		mover.Accelerate((seekTarget - transform.position) - mover.velocity, forceFullAcceleration);
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

	public void Arrive(Vector3 target, float slowDistance)
	{
		if ((transform.position - target).sqrMagnitude > Mathf.Pow(slowDistance, 2))
		{
			Seek(target);
		}
		else// if ((transform.position - target).sqrMagnitude > Mathf.Pow(mover.dampeningThreshold, 2))
		{
			Vector3 toTarget = target - transform.position;
			float toTargetMagnitude = toTarget.magnitude;
			Vector3 desiredVelocity = (toTarget / toTargetMagnitude) * mover.maxSpeed * (toTargetMagnitude / slowDistance);
			Vector3 velocityChange = desiredVelocity - mover.velocity;
			//Seek(velocityChange);
			mover.Accelerate(velocityChange, false);
		}
		//else
		//{
		//	mover.Stop();
		//}
		//Debug.Log(mover.velocity.magnitude);
	}
}
